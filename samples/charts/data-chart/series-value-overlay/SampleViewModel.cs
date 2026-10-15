using System;
using System.Collections.Generic;

// View-model properties live on Sample itself (it is its own DataContext); this file holds the
// sample's inline data types.
namespace Sample
{
    public class LocalDataItem
    {
        public double Label { get; set; }
        public double Value { get; set; }
    }

    public class LocalData
        : List<LocalDataItem>
    {
        public LocalData()
        {
            this.Add(new LocalDataItem() { Label = 1, Value = 1 });
            this.Add(new LocalDataItem() { Label = 2, Value = 2 });
            this.Add(new LocalDataItem() { Label = 3, Value = 6 });
            this.Add(new LocalDataItem() { Label = 4, Value = 8 });
            this.Add(new LocalDataItem() { Label = 5, Value = 2 });
            this.Add(new LocalDataItem() { Label = 6, Value = 6 });
            this.Add(new LocalDataItem() { Label = 7, Value = 4 });
            this.Add(new LocalDataItem() { Label = 8, Value = 2 });
            this.Add(new LocalDataItem() { Label = 9, Value = 1 });
        }
    }
}
