using System;
using System.Collections.Generic;

// View-model properties live on Sample itself (it is its own DataContext); this file holds the
// sample's inline data types.
namespace SamplesBrowser.Pages.Charts.PieChart.Selection {
    public class LocalDataItem
    {
        public double MarketShare { get; set; }
        public string Company { get; set; }
        public string Summary { get; set; }
    }

    public class LocalData
        : List<LocalDataItem>
    {
        public LocalData()
        {
            this.Add(new LocalDataItem() { MarketShare = 37, Company = @"Cooling", Summary = @"Cooling 37%" });
            this.Add(new LocalDataItem() { MarketShare = 25, Company = @"Residential", Summary = @"Residential 25%" });
            this.Add(new LocalDataItem() { MarketShare = 12, Company = @"Heating", Summary = @"Heating 12%" });
            this.Add(new LocalDataItem() { MarketShare = 8, Company = @"Lighting", Summary = @"Lighting 8%" });
            this.Add(new LocalDataItem() { MarketShare = 18, Company = @"Other", Summary = @"Other 18%" });
        }
    }
}
