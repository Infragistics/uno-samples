#define PCL

using Infragistics.Controls;
using Simple.OData.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using System.Xml;
using Infragistics.Controls.DataSource;
using System.Text;
#if PCL
using Infragistics.Core.Controls.DataSource;
#endif

#if DATA_PRESENTER
namespace Reference.DataSources.OData
#else
namespace Infragistics.Controls.DataSource
#endif
{
    internal class ODataVirtualDataSourceDataProviderWorkerSettings
        : AsyncVirtualDataSourceDataProviderWorkerSettings
    {
        public string BaseUri { get; set; }
        public string EntitySet { get; set; }

        public SortDescriptionCollection SortDescriptions { get; set; }

        public FilterExpressionCollection FilterExpressions { get; set; }

        public string[] PropertiesRequested { get; set; }
        public string[] SchemaIncludedProperties { get; set; }
        public SortDescriptionCollection GroupDescriptions { get; internal set; }
        public bool IsAggregationSupportedByServer { get; internal set; }

        public SummaryDescriptionCollection SummaryDescriptions { get; set; }
        public DataSourceSummaryScope SummaryScope { get; set; }
    }

    


    internal class ODataVirtualDataSourceProviderTaskDataHolder
        : AsyncVirtualDataSourceProviderTaskDataHolder
    {
        public ODataFeedAnnotations CompletedAnnotation { get; internal set; }
        public ODataFeedAnnotations[] CurrentAnnotations { get; set; }
    }

    
    internal class ODataVirtualDataSourceDataProviderWorker
        : AsyncVirtualDataSourceProviderWorker
    {
        private ODataClient _client;
        private string _baseUri;
        private string _entitySet;
        private SortDescriptionCollection _sortDescriptions;
        private FilterExpressionCollection _filterExpressions;
        private SummaryDescriptionCollection _summaryDescriptions;
        private string[] _desiredPropeties;
        private HashSet<string> _schemaIncludedProperties;

        protected SortDescriptionCollection SortDescriptions
        {
            get
            {
                return _sortDescriptions;
            }
        }

        protected FilterExpressionCollection FilterExpressions
        {
            get
            {
                return _filterExpressions;
            }
        }

        protected string[] DesiredProperties
        {
            get
            {
                return _desiredPropeties;
            }
        }

        private List<ODataFeedAnnotations> _annotations =
            new List<ODataFeedAnnotations>();

        protected override void Initialize()
        {
            // DoWork calls Initialize on every call, and the browser pump calls DoWork once per pass.
            if (_initialized)
            {
                return;
            }
            _initialized = true;

            base.Initialize();

            lock (SyncLock)
            {
                _client = new ODataClient(new ODataClientSettings()
                {
                    BaseUri = new Uri(_baseUri),
                    IgnoreUnmappedProperties = true
                });
            }

            Trace("Initialize");
            EnsureMetadataTask();
        }

        // TEMP diagnostics for the WASM remote-data issue.
        private static void Trace(string msg) => Console.WriteLine("[ODataWorker] " + msg);

        // The metadata document is fetched up front and every page request awaits it, so that by the
        // time ProcessCompletedTask resolves the schema the metadata task has already completed and
        // reading its Result never blocks. Blocking on an incomplete task (Task.Wait) throws on
        // single-threaded WebAssembly, which left the grid with no schema and no data.
        private Task<string> _metadataTask;

        private Task<string> EnsureMetadataTask()
        {
            lock (SyncLock)
            {
                if (_metadataTask == null || _metadataTask.IsFaulted || _metadataTask.IsCanceled)
                {
                    _metadataTask = _client.GetMetadataDocumentAsync();
                }
                return _metadataTask;
            }
        }

        private async Task<T> WithMetadata<T>(Task<T> request)
        {
            try
            {
                var result = await request.ConfigureAwait(false);
                Trace("request completed");
                await EnsureMetadataTask().ConfigureAwait(false);
                Trace("metadata completed");
                return result;
            }
            catch (Exception e)
            {
                Trace("request/metadata failed: " + e);
                throw;
            }
        }

        protected override AsyncVirtualDataSourceProviderTaskDataHolder GetTaskDataHolder()
        {
            ODataVirtualDataSourceProviderTaskDataHolder holder = new ODataVirtualDataSourceProviderTaskDataHolder();
            return holder;
        }

        protected override void GetCompletedTaskData(AsyncVirtualDataSourceProviderTaskDataHolder holder, int completed)
        {
            base.GetCompletedTaskData(holder, completed);

            var h = (ODataVirtualDataSourceProviderTaskDataHolder)holder;
            h.CompletedAnnotation = h.CurrentAnnotations[completed];
        }

        protected override void RemoveCompletedTaskData(AsyncVirtualDataSourceProviderTaskDataHolder holder, int completed)
        {
            base.RemoveCompletedTaskData(holder, completed);

            var h = (ODataVirtualDataSourceProviderTaskDataHolder)holder;
            _annotations.Remove(h.CompletedAnnotation);
        }

        protected override void GetTasksData(AsyncVirtualDataSourceProviderTaskDataHolder holder)
        {
            base.GetTasksData(holder);

            var h = (ODataVirtualDataSourceProviderTaskDataHolder)holder;
            h.CurrentAnnotations = _annotations.ToArray();
        }

        public ODataVirtualDataSourceDataProviderWorker(ODataVirtualDataSourceDataProviderWorkerSettings settings)
            :base(settings)
        {
            _baseUri = settings.BaseUri;
            _entitySet = settings.EntitySet;
            _sortDescriptions = settings.SortDescriptions;
            _filterExpressions = settings.FilterExpressions;
            _desiredPropeties = settings.PropertiesRequested;
            if (settings.SchemaIncludedProperties != null)
            {
                _schemaIncludedProperties = new HashSet<string>();
                for (int i = 0; i < settings.SchemaIncludedProperties.Length; i++)
                {
                    _schemaIncludedProperties.Add(settings.SchemaIncludedProperties[i]);
                }
            }
            _groupDescriptions = settings.GroupDescriptions;
            _summaryDescriptions = settings.SummaryDescriptions;
            _summaryScope = settings.SummaryScope;
            if (_groupDescriptions != null && _groupDescriptions.Count > 0)
            {
                _sortDescriptions = new SortDescriptionCollection();
                foreach (var sd in settings.SortDescriptions)
                {
                    _sortDescriptions.Add(sd);
                }

                for (var i = 0; i < _groupDescriptions.Count; i++)
                {
                    _sortDescriptions.Insert(i, _groupDescriptions[i]);
                }
            }
            _isAggregationSupportedByServer = settings.IsAggregationSupportedByServer;
            if (IsSingleThreaded)
            {
                Trace("ctor: starting browser pump");
                _ = PumpAsync();
            }
            else
            {
                Task.Factory.StartNew(() => DoWork(), TaskCreationOptions.LongRunning);
            }
        }

        // --- Single-threaded WebAssembly support -------------------------------------------------
        // The base class's DoWork is a while(true) loop that blocks in Task.WaitAny / Task.Wait until
        // a request completes. WASM (without threads) has only the UI thread, so blocking it means the
        // browser never gets to complete the HTTP request and the grid never receives data. In the
        // browser, DoWork is instead run one pass at a time from an async loop that yields to the
        // browser between passes (like the base class's JavaScript build, which polls via setTimeout).
        // Each pass ends by clearing IsRunning, which is the loop's only exit; the pump sets it again
        // before the next pass unless the worker has been shut down.
        private static readonly bool IsSingleThreaded = OperatingSystem.IsBrowser();
        private const int PumpIntervalMilliseconds = 16;
        private bool _initialized;

        private async Task PumpAsync()
        {
            while (!IsShutdown)
            {
                IsRunning = true;
                try
                {
                    DoWork();
                }
                catch (Exception e)
                {
                    Trace("DoWork pass threw: " + e);
                }
                if (IsShutdown)
                {
                    break;
                }
                await Task.Delay(PumpIntervalMilliseconds);
            }
            Trace("browser pump stopped");
        }

        protected override int CheckAndPotentiallyWaitOnTasks(AsyncDataSourcePageRequest[] currentTasks)
        {
            if (!IsSingleThreaded)
            {
                return base.CheckAndPotentiallyWaitOnTasks(currentTasks);
            }

            for (int i = 0; i < currentTasks.Length; i++)
            {
                if (currentTasks[i].IsDone || currentTasks[i].TaskHolder.Task.IsCompleted)
                {
                    // Keep running so DoWork processes this task; ProcessCompletedTask ends the pass.
                    return i;
                }
            }
            IsRunning = false;
            return -1;
        }

        protected override void AwaitChanges()
        {
            if (!IsSingleThreaded)
            {
                base.AwaitChanges();
                return;
            }
            IsRunning = false;
        }

        protected override void ProcessCompletedTask(AsyncDataSourcePageTaskHolder completedTask, int currentDelay, int pageIndex, AsyncVirtualDataSourceProviderTaskDataHolder taskDataHolder)
        {
            try
            {
                ProcessCompletedTaskCore(completedTask, currentDelay, pageIndex, taskDataHolder);
            }
            finally
            {
                if (IsSingleThreaded)
                {
                    IsRunning = false;
                }
            }
        }

        private void ProcessCompletedTaskCore(AsyncDataSourcePageTaskHolder completedTask, int currentDelay, int pageIndex, AsyncVirtualDataSourceProviderTaskDataHolder taskDataHolder)
        {
            ODataVirtualDataSourceProviderTaskDataHolder h = (ODataVirtualDataSourceProviderTaskDataHolder)taskDataHolder;
            IDataSourceSchema schema = null;
            IEnumerable<IDictionary<string, object>> result = null;
            ISectionInformation[] groupInformation = null;
            ISummaryResult[] summaryInformation = null;
            int schemaFetchCount = -1;
            bool isAggregationSupportedByServer = false;
            bool isGrouping = false;
            Trace("ProcessCompletedTask page " + pageIndex + " status " + completedTask.Task.Status);

            try
            {
                if (pageIndex == SchemaRequestIndex)
                {
                    Task<int> task = (Task<int>)completedTask.Task;

                    schemaFetchCount = task.Result;
                }
                else
                {
                    Task<IEnumerable<IDictionary<string, object>>> task = (Task<IEnumerable<IDictionary<string, object>>>)completedTask.Task;

                    result = task.Result;
                }                
            }
            catch (AggregateException e)
            {
                bool allCancels = true;
                foreach (var ex in e.Flatten().InnerExceptions)
                {
                    if (!(ex is TaskCanceledException))
                    {
                        allCancels = false;
                    }
                }
                if (!allCancels)
                {
                    RetryIndex(pageIndex, currentDelay);
                    return;
                }
                else
                {
                    RetryIndex(pageIndex, currentDelay);
                    return;
                }
            }
            catch (TaskCanceledException)
            {
                RetryIndex(pageIndex, currentDelay);
                //TODO: other exceptions? Is there a way to skip this state for canceled stuff?
                return;
            }

            IDataSourceExecutionContext executionContext;
            DataSourcePageLoadedCallback pageLoaded;
            
            lock (SyncLock)
            {
                if (schemaFetchCount >= 0)
                {
                    ActualCount = schemaFetchCount;
                }
                else
                {
                    var completedAnnotation = h.CompletedAnnotation;
                    if (completedAnnotation.Count.HasValue)
                    {
                        ActualCount = (int)completedAnnotation.Count.Value;
                    }
                }

                groupInformation = _groupInformation;
                summaryInformation = _summaryInformation;
                isAggregationSupportedByServer = _isAggregationSupportedByServer;
                schema = ActualSchema;
                isGrouping = _groupDescriptions != null && _groupDescriptions.Count > 0;
            }

            if (schema == null)
            {
                Trace("resolving schema");
                schema = ResolveSchema();
                Trace("schema resolved: " + (schema?.PropertyNames?.Length ?? -1) + " properties");

                lock (SyncLock)
                {
                    ActualSchema = schema;
                }
            }
            if (isAggregationSupportedByServer)
            {
                if (isGrouping && groupInformation == null)
                {
                    groupInformation = ResolveGroupInformation();
                }
                if (_summaryScope == DataSourceSummaryScope.Both || _summaryScope == DataSourceSummaryScope.Root)
                {
                    summaryInformation = ResolveSummaryInformation();
                }
            }    

            lock (SyncLock)
            {                
                ActualSchema = schema;
                _groupInformation = groupInformation;
                _summaryInformation = summaryInformation;
                executionContext = ExecutionContext;
                pageLoaded = PageLoaded;
            }

            ODataDataSourcePage page = null;

            if (result != null)
            {
                page = new ODataDataSourcePage(result, schema, groupInformation, summaryInformation, pageIndex);
                lock (SyncLock)
                {
                    if (!IsLastPage(pageIndex) && page.Count() > 0 && !PopulatedActualPageSize)
                    {
                        PopulatedActualPageSize = true;
                        ActualPageSize = page.Count();
                    }
                }
            }
            else
            {
                page = new ODataDataSourcePage(null, schema, groupInformation, summaryInformation, pageIndex);
            }

            Trace("page " + pageIndex + " ready, count " + ActualCount + ", PageLoaded " + (PageLoaded != null) + ", ExecutionContext " + (ExecutionContext != null));
            if (PageLoaded != null)
            {
                if (ExecutionContext != null)
                {
                    if (executionContext == null || pageLoaded == null)
                    {
                        Shutdown();
                        return;
                    }

                    executionContext.Execute(() =>
                        {
                            pageLoaded(page, ActualCount, ActualPageSize);
                        });              
                }
                else
                {
                    if (pageLoaded == null)
                    {
                        Shutdown();
                        return;
                    }

                    pageLoaded(page, ActualCount, ActualPageSize);
                }
            }
        }

        

        private IDataSourceSchema ResolveSchema()
        {
            // Already completed: every page task awaits the metadata before it completes.
            var metadataDocument = EnsureMetadataTask().GetAwaiter().GetResult();
            ODataSchemaProvider sp = new ODataSchemaProvider(metadataDocument);
            var schema = sp.GetODataDataSourceSchema(this._entitySet);
            if (this._schemaIncludedProperties != null)
            {
                List<string> propertyNames = new List<string>();
                List<DataSourceSchemaPropertyType> propertyTypes = new List<DataSourceSchemaPropertyType>();

                for (int i = 0; i < schema.PropertyNames.Length; i++)
                {
                    if (!_schemaIncludedProperties.Contains(schema.PropertyNames[i]))
                    {
                        continue;
                    }
                    propertyNames.Add(schema.PropertyNames[i]);
                    propertyTypes.Add(schema.PropertyTypes[i]);
                }
                schema = new ODataDataSourceSchema(propertyNames.ToArray(), propertyTypes.ToArray(), schema.PrimaryKey);
            }
            return schema;
        }
        private ISectionInformation[] ResolveGroupInformation()
        {
            //TODO: both this and the schema fetch should 
            //be async, and we may just hold up page delivery until present.
            //Rather than doing a sync wait.
            string orderBy = null;
            string groupBy = null;
            string filter = null;
            string summary = null;
            lock (SyncLock)
            {
                if (_groupDescriptions == null ||
                    _groupDescriptions.Count == 0)
                {
                    return null;
                }

                filter = _filterString;
                UpdateFilterString();

                StringBuilder gsb = new StringBuilder();
                if (_groupDescriptions != null)
                {
                    bool first = true;
                    foreach (var group in _groupDescriptions)
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            orderBy += ", ";
                            groupBy += ", ";
                        }

                        groupBy += group.PropertyName;

                        if (group.Direction ==
#if PCL
                            Infragistics.Core.Controls.DataSource.ListSortDirection.Descending
#else
                            System.ComponentModel.ListSortDirection.Descending
#endif
                            )
                        {
                            orderBy += group.PropertyName + " desc";
                        }
                        else
                        {
                            orderBy += group.PropertyName + " asc";
                        }
                    }
                }

                if (_summaryScope == DataSourceSummaryScope.Both || _summaryScope == DataSourceSummaryScope.Groups)
                {
                    var summaryQuery = GetSummaryQueryParameters(true);
                    if (!string.IsNullOrEmpty(summaryQuery))
                    {
                        summary = ", " + summaryQuery;
                    }
                }
            }

            var commandText = _entitySet + "?$orderby=" + orderBy + "&$apply=";
            if (!String.IsNullOrEmpty(filter))
            {
                commandText += "filter(" + filter + ")/";
            }
            commandText += "groupby((" + groupBy + "), aggregate($count as $__count" + summary + "))";

            lock (SyncLock)
            {
                try
                {
                    var annotations = new ODataFeedAnnotations();
                    var t = _client.FindEntriesAsync(commandText, annotations);
                    t.Wait();
                    var res = t.Result;
                    List<ISectionInformation> groupInformation = new List<ISectionInformation>();
                    List<string> groupNames = new List<string>();

                    foreach (var group in _groupDescriptions)
                    {
                        groupNames.Add(group.PropertyName);
                    }
                    var groupNamesArray = groupNames.ToArray();
                    int currentIndex = 0;
                    foreach (var group in res)
                    {
                        AddGroup(groupInformation, groupNames, 
                            groupNamesArray, currentIndex, 
                            group);
                    }
                    while (annotations.NextPageLink != null)
                    {
                        var link = _entitySet + "?" + annotations.NextPageLink.Query;
                        var t2 = _client.FindEntriesAsync(link, annotations);
                        t2.Wait();
                        var res2 = t2.Result;
                        foreach (var group in res2)
                        {
                            AddGroup(groupInformation, groupNames,
                                groupNamesArray, currentIndex,
                                group);
                        }
                    }

                    return groupInformation.ToArray();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        private void AddGroup(List<ISectionInformation> groupInformation, List<string> groupNames, string[] groupNamesArray, int currentIndex, IDictionary<string, object> group)
        {
            List<object> groupValues = new List<object>();
            foreach (var name in groupNames)
            {
                if (group.ContainsKey(name))
                {
                    groupValues.Add(group[name]);
                }
            }
            var groupCount = 0;
            if (group.ContainsKey("$__count"))
            {
                groupCount = Convert.ToInt32(group["$__count"]);
            }

            ISummaryResult[] summaryResults = null;
            if (_summaryScope == DataSourceSummaryScope.Both || _summaryScope == DataSourceSummaryScope.Groups)
            {
                summaryResults = CreateSummaryResults(group);
            }

            DefaultSectionInformation groupInfo = new DefaultSectionInformation(
                currentIndex,
                currentIndex + (groupCount - 1),
                groupNamesArray,
                groupValues.ToArray(),
                summaryResults);
            groupInformation.Add(groupInfo);
        }

        private ISummaryResult[] ResolveSummaryInformation()
        {
            string filter = null;
            string summary = null;
            lock (SyncLock)
            {
                if (_summaryDescriptions == null ||
                    _summaryDescriptions.Count == 0 ||
                    _summaryScope == DataSourceSummaryScope.Groups ||
                    _summaryScope == DataSourceSummaryScope.None)
                {
                    return null;
                }

                filter = _filterString;
                UpdateFilterString();

                summary = GetSummaryQueryParameters(false);
            }

            var commandText = _entitySet + "?$apply=";
            if (!String.IsNullOrEmpty(filter))
            {
                commandText += "filter(" + filter + ")/";
            }
            commandText += "aggregate(" + summary + ")";

            lock (SyncLock)
            {
                try
                {
                    var annotations = new ODataFeedAnnotations();
                    var t = _client.FindEntriesAsync(commandText, annotations);
                    t.Wait();
                    var res = t.Result;

                    var summaryData = res.FirstOrDefault();
                    if (summaryData == null)
                    {
                        return null;
                    }

                    return CreateSummaryResults(summaryData);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        private string GetSummaryQueryParameters(bool ignoreCount)
        {
            var query = "";
            if (_summaryDescriptions != null && _summaryDescriptions.Count > 0)
            {
                var first = true;
                var countExists = false;
                for (var i = 0; i < _summaryDescriptions.Count; i++)
                {
                    var summary = _summaryDescriptions[i];
                    if (summary.Operand == DataSourceSummaryOperand.Count && (ignoreCount || countExists))
                    {
                        continue;
                    }

                    if (!first)
                    {
                        query += ", ";
                    }

                    switch (summary.Operand)
                    {
                        case DataSourceSummaryOperand.Average:
                            query += summary.PropertyName + " with average as " + summary.PropertyName + "Average";
                            break;
                        case DataSourceSummaryOperand.Min:
                            query += summary.PropertyName + " with min as " + summary.PropertyName + "Min";
                            break;
                        case DataSourceSummaryOperand.Max:
                            query += summary.PropertyName + " with max as " + summary.PropertyName + "Max";
                            break;
                        case DataSourceSummaryOperand.Sum:
                            query += summary.PropertyName + " with sum as " + summary.PropertyName + "Sum";
                            break;
                        case DataSourceSummaryOperand.Count:
                            query += "$count as $__count";
                            countExists = true;
                            break;
                    }

                    first = false;
                }
            }
            return query;
        }

        private ISummaryResult[] CreateSummaryResults(IDictionary<string, object> data)
        {
            var summaryResults = new List<ISummaryResult>();
            for (var i = 0; i < _summaryDescriptions.Count; i++)
            {
                var summary = _summaryDescriptions[i];
                var propertyName = _summaryDescriptions[i].PropertyName;
                var summaryName = propertyName;
                switch (summary.Operand)
                {
                    case DataSourceSummaryOperand.Average:
                        summaryName += "Average";
                        break;
                    case DataSourceSummaryOperand.Min:
                        summaryName += "Min";
                        break;
                    case DataSourceSummaryOperand.Max:
                        summaryName += "Max";
                        break;
                    case DataSourceSummaryOperand.Sum:
                        summaryName += "Sum";
                        break;
                    case DataSourceSummaryOperand.Count:
                        summaryName = "$__count";
                        break;
                }

                if (!data.ContainsKey(summaryName))
                {
                    break;
                }

                object summaryValue = data[summaryName];
                // odata is returning strings to us so we need to convert it to the appropriate
                // type.
                if (summary.Operand == DataSourceSummaryOperand.Count)
                {
                    summaryValue = Convert.ToInt32(data[summaryName]);
                }
                else
                {
                    switch (ResolvePropertyType(propertyName))
                    {
                        case DataSourceSchemaPropertyType.BooleanValue:
                            summaryValue = Convert.ToBoolean(summaryValue);
                            break;
                        case DataSourceSchemaPropertyType.ByteValue:
                            summaryValue = Convert.ToByte(summaryValue);
                            break;
                        case DataSourceSchemaPropertyType.DateTimeValue:
                            summaryValue = Convert.ToDateTime(summaryValue);
                            break;
                        case DataSourceSchemaPropertyType.DecimalValue:
                            summaryValue = Convert.ToDecimal(summaryValue);
                            break;
                        case DataSourceSchemaPropertyType.DoubleValue:
                            summaryValue = Convert.ToDouble(summaryValue);
                            break;
                        case DataSourceSchemaPropertyType.IntValue:
                            summaryValue = (int)Convert.ToDouble(summaryValue);
                            break;
                        case DataSourceSchemaPropertyType.LongValue:
                            summaryValue = Convert.ToInt64(summaryValue);
                            break;
                        case DataSourceSchemaPropertyType.ShortValue:
                            summaryValue = Convert.ToInt16(summaryValue);
                            break;
                        case DataSourceSchemaPropertyType.SingleValue:
                            summaryValue = Convert.ToSingle(summaryValue);
                            break;
                        case DataSourceSchemaPropertyType.StringValue:
                            summaryValue = Convert.ToString(summaryValue);
                            break;
                    }
                }
                summaryResults.Add(new DefaultSummaryResult(summary.PropertyName, summary.Operand, summaryValue));
            }

            return summaryResults.ToArray();
        }

        private DataSourceSchemaPropertyType ResolvePropertyType(string propertyName)
        {
            var valueType = DataSourceSchemaPropertyType.ObjectValue;
            for (var j = 0; j < this.ActualSchema.PropertyNames.Length; j++)
            {
                if (this.ActualSchema.PropertyNames[j] == propertyName)
                {
                    valueType = this.ActualSchema.PropertyTypes[j];
                }
            }
            return valueType;
        }

        private string _filterString = null;
        public const int SchemaRequestIndex = -1;
        private SortDescriptionCollection _groupDescriptions;
        private bool _isAggregationSupportedByServer = false;
        private ISectionInformation[] _groupInformation = null;
        private ISummaryResult[] _summaryInformation = null;
        private DataSourceSummaryScope _summaryScope = DataSourceSummaryScope.None;

        protected override void MakeTaskForRequest(AsyncDataSourcePageRequest request, int retryDelay)
        {
            int actualPageSize = 0;
            SortDescriptionCollection sortDescriptions = null;
            lock (SyncLock)
            {
                actualPageSize = ActualPageSize;
                sortDescriptions = SortDescriptions;
            }

            ODataFeedAnnotations annotations = new ODataFeedAnnotations();

            var client = _client.For(_entitySet);

            lock (SyncLock)
            {
                UpdateFilterString();

                if (_filterString != null)
                {
                    client = client.Filter(_filterString);
                }

                if (SortDescriptions != null)
                {
                    foreach (var sort in SortDescriptions)
                    {
                        if (sort.Direction ==
#if PCL
							Infragistics.Core.Controls.DataSource.ListSortDirection.Descending
#else
                            System.ComponentModel.ListSortDirection.Descending
#endif
                            )
                        {
                            client = client.OrderByDescending(sort.PropertyName);
                        }
                        else
                        {
                            client = client.OrderBy(sort.PropertyName);
                        }
                    }
                }

                if (DesiredProperties != null && DesiredProperties.Length > 0)
                {
                    client = client.Select(DesiredProperties);
                }
            }

            Trace("MakeTaskForRequest index " + request.Index + " pageSize " + actualPageSize);
            Task task;
            if (request.Index == SchemaRequestIndex)
            {
                task = WithMetadata(client
                    .Count()
                    .FindScalarAsync<int>());
            }
            else
            {
                task = WithMetadata(client
                    .Skip(request.Index * actualPageSize)
                    .Top(actualPageSize)
                    .FindEntriesAsync(annotations));
            }

            request.TaskHolder = new AsyncDataSourcePageTaskHolder();
            request.TaskHolder.Task = task;

            lock (SyncLock)
            {
                Tasks.Add(request);
                _annotations.Add(annotations);
            }
        }

        private void UpdateFilterString()
        {
            if (FilterExpressions != null &&
                                FilterExpressions.Count > 0 &&
                                _filterString == null)
            {
                StringBuilder sb = new StringBuilder();
                bool first = true;
                foreach (var expr in FilterExpressions)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        sb.Append(" AND ");
                    }

                    ODataDataSourceFilterExpressionVisitor visitor = new ODataDataSourceFilterExpressionVisitor();

                    visitor.Visit(expr);

                    var txt = visitor.ToString();
                    if (FilterExpressions.Count > 1)
                    {
                        txt = "(" + txt + ")";
                    }
                    sb.Append(txt);
                }
                _filterString = sb.ToString();
            }
        }
    }

}