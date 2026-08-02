using System;
using System.Collections.Generic;
public class CountryNamesItem
{
    public string Name { get; set; }
}

public class CountryNames
    : List<CountryNamesItem>
{
    public CountryNames()
    {
        this.Add(new CountryNamesItem() { Name = @"Canada" });
        this.Add(new CountryNamesItem() { Name = @"France" });
        this.Add(new CountryNamesItem() { Name = @"Poland" });
        this.Add(new CountryNamesItem() { Name = @"UK" });
        this.Add(new CountryNamesItem() { Name = @"USA" });
    }
}
