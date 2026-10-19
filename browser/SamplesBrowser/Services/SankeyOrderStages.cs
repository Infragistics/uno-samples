using System;
using System.Collections.Generic;
public class SankeyOrderStagesItem
{
    public string Stage { get; set; }
}

public class SankeyOrderStages
    : List<SankeyOrderStagesItem>
{
    public SankeyOrderStages()
    {
        this.Add(new SankeyOrderStagesItem() { Stage = @"Region" });
        this.Add(new SankeyOrderStagesItem() { Stage = @"Product" });
        this.Add(new SankeyOrderStagesItem() { Stage = @"Channel" });
    }
}
