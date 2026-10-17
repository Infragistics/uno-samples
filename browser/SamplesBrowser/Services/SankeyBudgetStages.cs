using System;
using System.Collections.Generic;
public class SankeyBudgetStagesItem
{
    public string Stage { get; set; }
}

public class SankeyBudgetStages
    : List<SankeyBudgetStagesItem>
{
    public SankeyBudgetStages()
    {
        this.Add(new SankeyBudgetStagesItem() { Stage = @"Income" });
        this.Add(new SankeyBudgetStagesItem() { Stage = @"Category" });
        this.Add(new SankeyBudgetStagesItem() { Stage = @"Detail" });
    }
}
