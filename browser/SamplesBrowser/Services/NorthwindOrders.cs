//begin data
#if !TESTING
using Infragistics.Controls.DataSource;
#endif

    public class NorthwindOrders: ODataVirtualDataSource
    {
#if !TESTING
        public NorthwindOrders()
        {
            //var vds = new ODataVirtualDataSource();
            this.BaseUri = "https://services.odata.org/V4/Northwind/Northwind.svc";
            this.EntitySet = "Orders";
            this.PageSizeRequested = 200;
        }
#endif
    }
    //end data
