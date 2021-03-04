using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialChart
{
    public class DataSettings : ObservableModel
    {
        public DataSettings()
        {
            DataPoints = 100;
        }
        public int DataPoints { get; set; }

    }
}
