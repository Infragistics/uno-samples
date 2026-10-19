using System;
using System.Collections.Generic;
public class MonthlyRainfallByCityItem
{
    public string Month { get; set; }
    public double Seattle { get; set; }
    public double Madrid { get; set; }
}

public class MonthlyRainfallByCity
    : List<MonthlyRainfallByCityItem>
{
    public MonthlyRainfallByCity()
    {
        this.Add(new MonthlyRainfallByCityItem() { Month = @"January", Seattle = 140, Madrid = 37 });
        this.Add(new MonthlyRainfallByCityItem() { Month = @"February", Seattle = 100, Madrid = 35 });
        this.Add(new MonthlyRainfallByCityItem() { Month = @"March", Seattle = 95, Madrid = 26 });
        this.Add(new MonthlyRainfallByCityItem() { Month = @"April", Seattle = 70, Madrid = 47 });
        this.Add(new MonthlyRainfallByCityItem() { Month = @"May", Seattle = 48, Madrid = 52 });
        this.Add(new MonthlyRainfallByCityItem() { Month = @"June", Seattle = 40, Madrid = 25 });
        this.Add(new MonthlyRainfallByCityItem() { Month = @"July", Seattle = 15, Madrid = 15 });
        this.Add(new MonthlyRainfallByCityItem() { Month = @"August", Seattle = 22, Madrid = 10 });
        this.Add(new MonthlyRainfallByCityItem() { Month = @"September", Seattle = 40, Madrid = 28 });
        this.Add(new MonthlyRainfallByCityItem() { Month = @"October", Seattle = 90, Madrid = 49 });
        this.Add(new MonthlyRainfallByCityItem() { Month = @"November", Seattle = 165, Madrid = 56 });
        this.Add(new MonthlyRainfallByCityItem() { Month = @"December", Seattle = 150, Madrid = 56 });
    }
}
