using System;
using System.Collections.Generic;
public class SankeyHouseholdNodesItem
{
    public string Id { get; set; }
    public string Label { get; set; }
    public double Layer { get; set; }
}

public class SankeyHouseholdNodes
    : List<SankeyHouseholdNodesItem>
{
    public SankeyHouseholdNodes()
    {
        this.Add(new SankeyHouseholdNodesItem() { Id = @"budget", Label = @"Monthly Budget $6,200", Layer = 0 });
        this.Add(new SankeyHouseholdNodesItem() { Id = @"needs", Label = @"Needs $3,100", Layer = 1 });
        this.Add(new SankeyHouseholdNodesItem() { Id = @"wants", Label = @"Wants $1,240", Layer = 1 });
        this.Add(new SankeyHouseholdNodesItem() { Id = @"savings", Label = @"Savings $1,860", Layer = 1 });
        this.Add(new SankeyHouseholdNodesItem() { Id = @"housing", Label = @"Housing $1,800", Layer = 2 });
        this.Add(new SankeyHouseholdNodesItem() { Id = @"living", Label = @"Living $1,300", Layer = 2 });
        this.Add(new SankeyHouseholdNodesItem() { Id = @"lifestyle", Label = @"Lifestyle $1,240", Layer = 2 });
        this.Add(new SankeyHouseholdNodesItem() { Id = @"investing", Label = @"Investing $1,100", Layer = 2 });
        this.Add(new SankeyHouseholdNodesItem() { Id = @"emergency", Label = @"Emergency Fund $760", Layer = 2 });
        this.Add(new SankeyHouseholdNodesItem() { Id = @"mortgage", Label = @"Mortgage $1,500", Layer = 3 });
        this.Add(new SankeyHouseholdNodesItem() { Id = @"hoa", Label = @"HOA $300", Layer = 3 });
        this.Add(new SankeyHouseholdNodesItem() { Id = @"groceries", Label = @"Groceries $650", Layer = 3 });
        this.Add(new SankeyHouseholdNodesItem() { Id = @"utilities", Label = @"Utilities $350", Layer = 3 });
        this.Add(new SankeyHouseholdNodesItem() { Id = @"transport", Label = @"Transport $300", Layer = 3 });
        this.Add(new SankeyHouseholdNodesItem() { Id = @"dining", Label = @"Dining $480", Layer = 3 });
        this.Add(new SankeyHouseholdNodesItem() { Id = @"travel", Label = @"Travel $760", Layer = 3 });
        this.Add(new SankeyHouseholdNodesItem() { Id = @"retire", Label = @"401k $700", Layer = 3 });
        this.Add(new SankeyHouseholdNodesItem() { Id = @"roth", Label = @"Roth IRA $400", Layer = 3 });
    }
}
