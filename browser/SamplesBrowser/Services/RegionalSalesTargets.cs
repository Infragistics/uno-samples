using System;
using System.Collections.Generic;
public class RegionalSalesTargetsItem
{
    public string Region { get; set; }
    public double Actual { get; set; }
    public double Target { get; set; }
}

public class RegionalSalesTargets
    : List<RegionalSalesTargetsItem>
{
    public RegionalSalesTargets()
    {
        this.Add(new RegionalSalesTargetsItem() { Region = @"North America", Actual = 4.8, Target = 4.5 });
        this.Add(new RegionalSalesTargetsItem() { Region = @"Europe", Actual = 3.6, Target = 4 });
        this.Add(new RegionalSalesTargetsItem() { Region = @"Asia Pacific", Actual = 4.1, Target = 3.8 });
        this.Add(new RegionalSalesTargetsItem() { Region = @"Latin America", Actual = 1.7, Target = 2 });
        this.Add(new RegionalSalesTargetsItem() { Region = @"Middle East", Actual = 1.2, Target = 1.1 });
        this.Add(new RegionalSalesTargetsItem() { Region = @"Africa", Actual = 0.8, Target = 1 });
    }
}
