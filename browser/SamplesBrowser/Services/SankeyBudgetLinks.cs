using System;
using System.Collections.Generic;
public class SankeyBudgetLinksItem
{
    public string Source { get; set; }
    public string Target { get; set; }
    public double Value { get; set; }
    public double Highlighted { get; set; }
}

public class SankeyBudgetLinks
    : List<SankeyBudgetLinksItem>
{
    public SankeyBudgetLinks()
    {
        this.Add(new SankeyBudgetLinksItem() { Source = @"income", Target = @"housing", Value = 2137, Highlighted = 1461 });
        this.Add(new SankeyBudgetLinksItem() { Source = @"income", Target = @"food", Value = 812, Highlighted = 548 });
        this.Add(new SankeyBudgetLinksItem() { Source = @"income", Target = @"transport", Value = 468, Highlighted = 302 });
        this.Add(new SankeyBudgetLinksItem() { Source = @"income", Target = @"savings", Value = 1243, Highlighted = 726 });
        this.Add(new SankeyBudgetLinksItem() { Source = @"housing", Target = @"rent", Value = 1592, Highlighted = 1118 });
        this.Add(new SankeyBudgetLinksItem() { Source = @"housing", Target = @"utilities", Value = 545, Highlighted = 358 });
        this.Add(new SankeyBudgetLinksItem() { Source = @"food", Target = @"groceries", Value = 517, Highlighted = 372 });
        this.Add(new SankeyBudgetLinksItem() { Source = @"food", Target = @"dining", Value = 295, Highlighted = 166 });
        this.Add(new SankeyBudgetLinksItem() { Source = @"transport", Target = @"car", Value = 468, Highlighted = 305 });
        this.Add(new SankeyBudgetLinksItem() { Source = @"savings", Target = @"retirement", Value = 831, Highlighted = 523 });
        this.Add(new SankeyBudgetLinksItem() { Source = @"savings", Target = @"emergency", Value = 412, Highlighted = 234 });
    }
}
