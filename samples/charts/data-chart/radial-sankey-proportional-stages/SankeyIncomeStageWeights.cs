using System;
using System.Collections.Generic;
public class SankeyIncomeStageWeightsItem
{
    public string Stage { get; set; }
    public double Weight { get; set; }
}

public class SankeyIncomeStageWeights
    : List<SankeyIncomeStageWeightsItem>
{
    public SankeyIncomeStageWeights()
    {
        this.Add(new SankeyIncomeStageWeightsItem() { Stage = @"Income", Weight = 1 });
        this.Add(new SankeyIncomeStageWeightsItem() { Stage = @"Budget", Weight = 4 });
        this.Add(new SankeyIncomeStageWeightsItem() { Stage = @"Bucket", Weight = 2 });
        this.Add(new SankeyIncomeStageWeightsItem() { Stage = @"Detail", Weight = 3 });
    }
}
