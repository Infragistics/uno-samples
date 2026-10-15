using System;
using System.Collections.Generic;
public class EnergyRenewableGenerationItem
{
    public string Year { get; set; }
    public double Europe { get; set; }
    public double America { get; set; }
    public double China { get; set; }
}

public class EnergyRenewableGeneration
    : List<EnergyRenewableGenerationItem>
{
    public EnergyRenewableGeneration()
    {
        this.Add(new EnergyRenewableGenerationItem() { Year = @"2009", Europe = 31, America = 19, China = 21 });
        this.Add(new EnergyRenewableGenerationItem() { Year = @"2010", Europe = 43, America = 24, China = 26 });
        this.Add(new EnergyRenewableGenerationItem() { Year = @"2011", Europe = 66, America = 28, China = 29 });
        this.Add(new EnergyRenewableGenerationItem() { Year = @"2012", Europe = 69, America = 26, China = 32 });
        this.Add(new EnergyRenewableGenerationItem() { Year = @"2013", Europe = 58, America = 38, China = 47 });
        this.Add(new EnergyRenewableGenerationItem() { Year = @"2014", Europe = 40, America = 31, China = 46 });
        this.Add(new EnergyRenewableGenerationItem() { Year = @"2015", Europe = 78, America = 19, China = 50 });
        this.Add(new EnergyRenewableGenerationItem() { Year = @"2016", Europe = 13, America = 52, China = 90 });
        this.Add(new EnergyRenewableGenerationItem() { Year = @"2017", Europe = 78, America = 50, China = 132 });
        this.Add(new EnergyRenewableGenerationItem() { Year = @"2018", Europe = 40, America = 34, China = 134 });
        this.Add(new EnergyRenewableGenerationItem() { Year = @"2019", Europe = 80, America = 38, China = 96 });
    }
}
