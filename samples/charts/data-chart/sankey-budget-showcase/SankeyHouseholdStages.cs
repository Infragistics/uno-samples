using System;
using System.Collections.Generic;
public class SankeyHouseholdStagesItem
{
    public string Stage { get; set; }
}

public class SankeyHouseholdStages
    : List<SankeyHouseholdStagesItem>
{
    public SankeyHouseholdStages()
    {
        this.Add(new SankeyHouseholdStagesItem() { Stage = @"Budget" });
        this.Add(new SankeyHouseholdStagesItem() { Stage = @"Split" });
        this.Add(new SankeyHouseholdStagesItem() { Stage = @"Bucket" });
        this.Add(new SankeyHouseholdStagesItem() { Stage = @"Detail" });
    }
}
