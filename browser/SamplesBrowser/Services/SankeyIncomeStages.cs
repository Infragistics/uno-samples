using System;
using System.Collections.Generic;
public class SankeyIncomeStagesItem
{
    public string Stage { get; set; }
}

public class SankeyIncomeStages
    : List<SankeyIncomeStagesItem>
{
    public SankeyIncomeStages()
    {
        this.Add(new SankeyIncomeStagesItem() { Stage = @"Income" });
        this.Add(new SankeyIncomeStagesItem() { Stage = @"Budget" });
        this.Add(new SankeyIncomeStagesItem() { Stage = @"Bucket" });
        this.Add(new SankeyIncomeStagesItem() { Stage = @"Detail" });
    }
}
