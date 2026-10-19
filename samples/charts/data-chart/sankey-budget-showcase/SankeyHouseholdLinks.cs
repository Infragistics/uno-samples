using System;
using System.Collections.Generic;
public class SankeyHouseholdLinksItem
{
    public string Source { get; set; }
    public string Target { get; set; }
    public double Value { get; set; }
}

public class SankeyHouseholdLinks
    : List<SankeyHouseholdLinksItem>
{
    public SankeyHouseholdLinks()
    {
        this.Add(new SankeyHouseholdLinksItem() { Source = @"budget", Target = @"needs", Value = 3100 });
        this.Add(new SankeyHouseholdLinksItem() { Source = @"budget", Target = @"wants", Value = 1240 });
        this.Add(new SankeyHouseholdLinksItem() { Source = @"budget", Target = @"savings", Value = 1860 });
        this.Add(new SankeyHouseholdLinksItem() { Source = @"needs", Target = @"housing", Value = 1800 });
        this.Add(new SankeyHouseholdLinksItem() { Source = @"needs", Target = @"living", Value = 1300 });
        this.Add(new SankeyHouseholdLinksItem() { Source = @"wants", Target = @"lifestyle", Value = 1240 });
        this.Add(new SankeyHouseholdLinksItem() { Source = @"savings", Target = @"investing", Value = 1100 });
        this.Add(new SankeyHouseholdLinksItem() { Source = @"savings", Target = @"emergency", Value = 760 });
        this.Add(new SankeyHouseholdLinksItem() { Source = @"housing", Target = @"mortgage", Value = 1500 });
        this.Add(new SankeyHouseholdLinksItem() { Source = @"housing", Target = @"hoa", Value = 300 });
        this.Add(new SankeyHouseholdLinksItem() { Source = @"living", Target = @"groceries", Value = 650 });
        this.Add(new SankeyHouseholdLinksItem() { Source = @"living", Target = @"utilities", Value = 350 });
        this.Add(new SankeyHouseholdLinksItem() { Source = @"living", Target = @"transport", Value = 300 });
        this.Add(new SankeyHouseholdLinksItem() { Source = @"lifestyle", Target = @"dining", Value = 480 });
        this.Add(new SankeyHouseholdLinksItem() { Source = @"lifestyle", Target = @"travel", Value = 760 });
        this.Add(new SankeyHouseholdLinksItem() { Source = @"investing", Target = @"retire", Value = 700 });
        this.Add(new SankeyHouseholdLinksItem() { Source = @"investing", Target = @"roth", Value = 400 });
    }
}
