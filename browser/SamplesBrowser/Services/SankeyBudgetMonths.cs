using System;
using System.Collections.Generic;
public class SankeyBudgetMonthsItem
{
    public string Id { get; set; }
    public string Source { get; set; }
    public string Target { get; set; }
    public double July { get; set; }
    public double August { get; set; }
    public double September { get; set; }
}

public class SankeyBudgetMonths
    : List<SankeyBudgetMonthsItem>
{
    public SankeyBudgetMonths()
    {
        this.Add(new SankeyBudgetMonthsItem() { Id = @"income-housing", Source = @"income", Target = @"housing", July = 2137, August = 2151, September = 2129 });
        this.Add(new SankeyBudgetMonthsItem() { Id = @"income-food", Source = @"income", Target = @"food", July = 812, August = 1036, September = 774 });
        this.Add(new SankeyBudgetMonthsItem() { Id = @"income-transport", Source = @"income", Target = @"transport", July = 468, August = 735, September = 391 });
        this.Add(new SankeyBudgetMonthsItem() { Id = @"income-savings", Source = @"income", Target = @"savings", July = 1243, August = 806, September = 1597 });
        this.Add(new SankeyBudgetMonthsItem() { Id = @"housing-rent", Source = @"housing", Target = @"rent", July = 1592, August = 1592, September = 1592 });
        this.Add(new SankeyBudgetMonthsItem() { Id = @"housing-utilities", Source = @"housing", Target = @"utilities", July = 545, August = 559, September = 537 });
        this.Add(new SankeyBudgetMonthsItem() { Id = @"food-groceries", Source = @"food", Target = @"groceries", July = 517, August = 548, September = 532 });
        this.Add(new SankeyBudgetMonthsItem() { Id = @"food-dining", Source = @"food", Target = @"dining", July = 295, August = 488, September = 242 });
        this.Add(new SankeyBudgetMonthsItem() { Id = @"transport-car", Source = @"transport", Target = @"car", July = 468, August = 735, September = 391 });
        this.Add(new SankeyBudgetMonthsItem() { Id = @"savings-retirement", Source = @"savings", Target = @"retirement", July = 831, August = 612, September = 978 });
        this.Add(new SankeyBudgetMonthsItem() { Id = @"savings-emergency", Source = @"savings", Target = @"emergency", July = 412, August = 194, September = 619 });
    }
}
