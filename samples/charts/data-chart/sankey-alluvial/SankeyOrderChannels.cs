using System;
using System.Collections.Generic;
public class SankeyOrderChannelsItem
{
    public string Region { get; set; }
    public string Product { get; set; }
    public string Channel { get; set; }
    public double Orders { get; set; }
}

public class SankeyOrderChannels
    : List<SankeyOrderChannelsItem>
{
    public SankeyOrderChannels()
    {
        this.Add(new SankeyOrderChannelsItem() { Region = @"North", Product = @"Laptops", Channel = @"Online", Orders = 412 });
        this.Add(new SankeyOrderChannelsItem() { Region = @"North", Product = @"Laptops", Channel = @"Retail", Orders = 187 });
        this.Add(new SankeyOrderChannelsItem() { Region = @"North", Product = @"Tablets", Channel = @"Online", Orders = 233 });
        this.Add(new SankeyOrderChannelsItem() { Region = @"North", Product = @"Phones", Channel = @"Partner", Orders = 146 });
        this.Add(new SankeyOrderChannelsItem() { Region = @"South", Product = @"Laptops", Channel = @"Partner", Orders = 129 });
        this.Add(new SankeyOrderChannelsItem() { Region = @"South", Product = @"Tablets", Channel = @"Retail", Orders = 274 });
        this.Add(new SankeyOrderChannelsItem() { Region = @"South", Product = @"Phones", Channel = @"Online", Orders = 361 });
        this.Add(new SankeyOrderChannelsItem() { Region = @"South", Product = @"Phones", Channel = @"Retail", Orders = 158 });
        this.Add(new SankeyOrderChannelsItem() { Region = @"West", Product = @"Laptops", Channel = @"Online", Orders = 297 });
        this.Add(new SankeyOrderChannelsItem() { Region = @"West", Product = @"Tablets", Channel = @"Partner", Orders = 92 });
        this.Add(new SankeyOrderChannelsItem() { Region = @"West", Product = @"Phones", Channel = @"Online", Orders = 218 });
        this.Add(new SankeyOrderChannelsItem() { Region = @"West", Product = @"Phones", Channel = @"Retail", Orders = 174 });
    }
}
