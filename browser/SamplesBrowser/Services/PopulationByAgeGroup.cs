using System;
using System.Collections.Generic;
public class PopulationByAgeGroupItem
{
    public string AgeGroup { get; set; }
    public double Year1990 { get; set; }
    public double Year2025 { get; set; }
}

public class PopulationByAgeGroup
    : List<PopulationByAgeGroupItem>
{
    public PopulationByAgeGroup()
    {
        this.Add(new PopulationByAgeGroupItem() { AgeGroup = @"0-9", Year1990 = 36, Year2025 = 39.4 });
        this.Add(new PopulationByAgeGroupItem() { AgeGroup = @"10-19", Year1990 = 35, Year2025 = 42.6 });
        this.Add(new PopulationByAgeGroupItem() { AgeGroup = @"20-29", Year1990 = 40.5, Year2025 = 44.9 });
        this.Add(new PopulationByAgeGroupItem() { AgeGroup = @"30-39", Year1990 = 41.8, Year2025 = 46.2 });
        this.Add(new PopulationByAgeGroupItem() { AgeGroup = @"40-49", Year1990 = 31, Year2025 = 42 });
        this.Add(new PopulationByAgeGroupItem() { AgeGroup = @"50-59", Year1990 = 21.6, Year2025 = 41.1 });
        this.Add(new PopulationByAgeGroupItem() { AgeGroup = @"60-69", Year1990 = 20, Year2025 = 38.9 });
        this.Add(new PopulationByAgeGroupItem() { AgeGroup = @"70-79", Year1990 = 13.7, Year2025 = 26.8 });
        this.Add(new PopulationByAgeGroupItem() { AgeGroup = @"80+", Year1990 = 7, Year2025 = 13.9 });
    }
}
