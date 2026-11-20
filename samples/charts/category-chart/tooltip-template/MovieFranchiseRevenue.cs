using System;
using System.Collections.Generic;
public class MovieFranchiseRevenueItem
{
    public string Franchise { get; set; }
    public double TotalRevenue { get; set; }
    public double HighestGrossing { get; set; }
}

public class MovieFranchiseRevenue
    : List<MovieFranchiseRevenueItem>
{
    public MovieFranchiseRevenue()
    {
        this.Add(new MovieFranchiseRevenueItem() { Franchise = @"Marvel Universe All Films", TotalRevenue = 22.55, HighestGrossing = 2.8 });
        this.Add(new MovieFranchiseRevenueItem() { Franchise = @"Star Wars", TotalRevenue = 10.32, HighestGrossing = 2.07 });
        this.Add(new MovieFranchiseRevenueItem() { Franchise = @"Harry Potter", TotalRevenue = 9.19, HighestGrossing = 1.34 });
        this.Add(new MovieFranchiseRevenueItem() { Franchise = @"Avengers", TotalRevenue = 7.76, HighestGrossing = 2.8 });
        this.Add(new MovieFranchiseRevenueItem() { Franchise = @"Spider Man", TotalRevenue = 7.22, HighestGrossing = 1.28 });
        this.Add(new MovieFranchiseRevenueItem() { Franchise = @"James Bond", TotalRevenue = 7.12, HighestGrossing = 1.11 });
    }
}
