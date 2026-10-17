using System;
using System.Collections.Generic;
public class SankeyBudgetNodesItem
{
    public string Id { get; set; }
    public string Label { get; set; }
    public double Layer { get; set; }
}

public class SankeyBudgetNodes
    : List<SankeyBudgetNodesItem>
{
    public SankeyBudgetNodes()
    {
        this.Add(new SankeyBudgetNodesItem() { Id = @"income", Label = @"Income", Layer = 0 });
        this.Add(new SankeyBudgetNodesItem() { Id = @"housing", Label = @"Housing", Layer = 1 });
        this.Add(new SankeyBudgetNodesItem() { Id = @"food", Label = @"Food", Layer = 1 });
        this.Add(new SankeyBudgetNodesItem() { Id = @"transport", Label = @"Transport", Layer = 1 });
        this.Add(new SankeyBudgetNodesItem() { Id = @"savings", Label = @"Savings", Layer = 1 });
        this.Add(new SankeyBudgetNodesItem() { Id = @"rent", Label = @"Rent", Layer = 2 });
        this.Add(new SankeyBudgetNodesItem() { Id = @"utilities", Label = @"Utilities", Layer = 2 });
        this.Add(new SankeyBudgetNodesItem() { Id = @"groceries", Label = @"Groceries", Layer = 2 });
        this.Add(new SankeyBudgetNodesItem() { Id = @"dining", Label = @"Dining Out", Layer = 2 });
        this.Add(new SankeyBudgetNodesItem() { Id = @"car", Label = @"Car", Layer = 2 });
        this.Add(new SankeyBudgetNodesItem() { Id = @"retirement", Label = @"Retirement", Layer = 2 });
        this.Add(new SankeyBudgetNodesItem() { Id = @"emergency", Label = @"Emergency Fund", Layer = 2 });
    }
}
