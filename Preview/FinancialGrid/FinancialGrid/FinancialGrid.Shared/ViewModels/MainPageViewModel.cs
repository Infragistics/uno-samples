using FinancialGrid.Business;
using FinancialGrid.Services;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;

namespace FinancialGrid.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
        readonly IFinancialDataService _service = new FinancialDataService();

        private bool _canUpdatePrices = true;
        public bool CanUpdatePrices
        {
            get { return _canUpdatePrices; }
            set { SetProperty(ref _canUpdatePrices, value); }
        }

        private List<FinancialRecord> _financialData;
        public List<FinancialRecord> FinancialData
        {
            get { return _financialData; }
            set { SetProperty(ref _financialData, value); }
        }

        private int _frequency = 250;
        public int Frequency
        {
            get { return _frequency; }
            set { SetProperty(ref _frequency, value); }
        }

        private int _numberOfRecords = 1000;
        public int NumberOfRecords
        {
            get { return _numberOfRecords; }
            set { SetProperty(ref _numberOfRecords, value, () => UpdateRecordCount()); }
        }

        private DelegateCommand _livePricesCommand;
        public DelegateCommand LivePricesCommand =>
            _livePricesCommand ?? (_livePricesCommand = new DelegateCommand(ExecuteLivePricesCommand)).ObservesCanExecute(() => CanUpdatePrices);

        private DelegateCommand _stopTimerCommand;
        public DelegateCommand StopTimerCommand =>
            _stopTimerCommand ?? (_stopTimerCommand = new DelegateCommand(ExecuteStopTimerCommand, CanExecuteStopTimerCommand)).ObservesProperty(() => CanUpdatePrices);

        public MainPageViewModel()
        {
            UpdateRecordCount();
        }

        void ExecuteLivePricesCommand()
        {
            CanUpdatePrices = false;
            _service.StartUpdatingPrices(FinancialData, Frequency);
        }

        void ExecuteStopTimerCommand()
        {
            CanUpdatePrices = true;
            _service.StopUpdatingPrices();
        }

        bool CanExecuteStopTimerCommand()
        {
            return !CanUpdatePrices;
        }

        async void UpdateRecordCount()
        {
            FinancialData = await _service.GetDataAsync(NumberOfRecords);
        }
    }
}
