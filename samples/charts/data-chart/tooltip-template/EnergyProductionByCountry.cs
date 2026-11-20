using System;
using System.Collections.Generic;
public class EnergyProductionByCountryItem
{
    public string Country { get; set; }
    public double Coal { get; set; }
    public double Oil { get; set; }
    public double Gas { get; set; }
    public double Nuclear { get; set; }
    public double Hydro { get; set; }
}

public class EnergyProductionByCountry
    : List<EnergyProductionByCountryItem>
{
    public EnergyProductionByCountry()
    {
        this.Add(new EnergyProductionByCountryItem() { Country = @"Canada", Coal = 400, Oil = 100, Gas = 175, Nuclear = 225, Hydro = 350 });
        this.Add(new EnergyProductionByCountryItem() { Country = @"China", Coal = 925, Oil = 200, Gas = 350, Nuclear = 400, Hydro = 625 });
        this.Add(new EnergyProductionByCountryItem() { Country = @"Russia", Coal = 550, Oil = 200, Gas = 250, Nuclear = 475, Hydro = 425 });
        this.Add(new EnergyProductionByCountryItem() { Country = @"Australia", Coal = 450, Oil = 100, Gas = 150, Nuclear = 175, Hydro = 350 });
        this.Add(new EnergyProductionByCountryItem() { Country = @"United States", Coal = 800, Oil = 250, Gas = 475, Nuclear = 575, Hydro = 750 });
        this.Add(new EnergyProductionByCountryItem() { Country = @"France", Coal = 375, Oil = 150, Gas = 350, Nuclear = 275, Hydro = 325 });
    }
}
