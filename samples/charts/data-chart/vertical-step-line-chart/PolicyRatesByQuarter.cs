using System;
using System.Collections.Generic;
public class PolicyRatesByQuarterItem
{
    public string Quarter { get; set; }
    public double Federal { get; set; }
    public double European { get; set; }
}

public class PolicyRatesByQuarter
    : List<PolicyRatesByQuarterItem>
{
    public PolicyRatesByQuarter()
    {
        this.Add(new PolicyRatesByQuarterItem() { Quarter = @"Q1 2022", Federal = 0.5, European = 0 });
        this.Add(new PolicyRatesByQuarterItem() { Quarter = @"Q2 2022", Federal = 1.75, European = 0 });
        this.Add(new PolicyRatesByQuarterItem() { Quarter = @"Q3 2022", Federal = 3.25, European = 1.25 });
        this.Add(new PolicyRatesByQuarterItem() { Quarter = @"Q4 2022", Federal = 4.5, European = 2.5 });
        this.Add(new PolicyRatesByQuarterItem() { Quarter = @"Q1 2023", Federal = 5, European = 3.5 });
        this.Add(new PolicyRatesByQuarterItem() { Quarter = @"Q2 2023", Federal = 5.25, European = 4 });
        this.Add(new PolicyRatesByQuarterItem() { Quarter = @"Q3 2023", Federal = 5.5, European = 4.5 });
        this.Add(new PolicyRatesByQuarterItem() { Quarter = @"Q4 2023", Federal = 5.5, European = 4.5 });
        this.Add(new PolicyRatesByQuarterItem() { Quarter = @"Q1 2024", Federal = 5.5, European = 4.5 });
        this.Add(new PolicyRatesByQuarterItem() { Quarter = @"Q2 2024", Federal = 5.5, European = 4.25 });
        this.Add(new PolicyRatesByQuarterItem() { Quarter = @"Q3 2024", Federal = 5, European = 3.65 });
        this.Add(new PolicyRatesByQuarterItem() { Quarter = @"Q4 2024", Federal = 4.5, European = 3.15 });
        this.Add(new PolicyRatesByQuarterItem() { Quarter = @"Q1 2025", Federal = 4.5, European = 2.65 });
        this.Add(new PolicyRatesByQuarterItem() { Quarter = @"Q2 2025", Federal = 4.5, European = 2.15 });
    }
}
