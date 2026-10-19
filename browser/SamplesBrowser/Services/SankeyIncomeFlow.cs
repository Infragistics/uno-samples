using System;
using System.Collections.Generic;
public class SankeyIncomeFlowItem
{
    public string Source { get; set; }
    public string Target { get; set; }
    public double Value { get; set; }
    public double Highlighted { get; set; }
}

public class SankeyIncomeFlow
    : List<SankeyIncomeFlowItem>
{
    public SankeyIncomeFlow()
    {
        this.Add(new SankeyIncomeFlowItem() { Source = @"Salary", Target = @"Budget", Value = 7642, Highlighted = 4239 });
        this.Add(new SankeyIncomeFlowItem() { Source = @"Bonus", Target = @"Budget", Value = 1873, Highlighted = 869 });
        this.Add(new SankeyIncomeFlowItem() { Source = @"Budget", Target = @"Housing", Value = 2914, Highlighted = 1927 });
        this.Add(new SankeyIncomeFlowItem() { Source = @"Budget", Target = @"Savings", Value = 4226, Highlighted = 2103 });
        this.Add(new SankeyIncomeFlowItem() { Source = @"Budget", Target = @"Discretionary", Value = 2375, Highlighted = 1078 });
        this.Add(new SankeyIncomeFlowItem() { Source = @"Housing", Target = @"Rent", Value = 2437, Highlighted = 1566 });
        this.Add(new SankeyIncomeFlowItem() { Source = @"Housing", Target = @"Utilities", Value = 477, Highlighted = 361 });
        this.Add(new SankeyIncomeFlowItem() { Source = @"Savings", Target = @"ETFs", Value = 2519, Highlighted = 1084 });
        this.Add(new SankeyIncomeFlowItem() { Source = @"Savings", Target = @"Emergency", Value = 1106, Highlighted = 683 });
        this.Add(new SankeyIncomeFlowItem() { Source = @"Savings", Target = @"Retirement", Value = 601, Highlighted = 336 });
        this.Add(new SankeyIncomeFlowItem() { Source = @"Discretionary", Target = @"Dining", Value = 1047, Highlighted = 512 });
        this.Add(new SankeyIncomeFlowItem() { Source = @"Discretionary", Target = @"Travel", Value = 869, Highlighted = 371 });
        this.Add(new SankeyIncomeFlowItem() { Source = @"Discretionary", Target = @"Entertainment", Value = 459, Highlighted = 195 });
    }
}
