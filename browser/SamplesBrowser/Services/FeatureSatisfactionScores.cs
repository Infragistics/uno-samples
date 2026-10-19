using System;
using System.Collections.Generic;
public class FeatureSatisfactionScoresItem
{
    public string Feature { get; set; }
    public double Year2024 { get; set; }
    public double Year2025 { get; set; }
}

public class FeatureSatisfactionScores
    : List<FeatureSatisfactionScoresItem>
{
    public FeatureSatisfactionScores()
    {
        this.Add(new FeatureSatisfactionScoresItem() { Feature = @"Search and filtering", Year2024 = 7.1, Year2025 = 8.3 });
        this.Add(new FeatureSatisfactionScoresItem() { Feature = @"Dashboard customization", Year2024 = 6.4, Year2025 = 7.9 });
        this.Add(new FeatureSatisfactionScoresItem() { Feature = @"Mobile app", Year2024 = 5.8, Year2025 = 7.2 });
        this.Add(new FeatureSatisfactionScoresItem() { Feature = @"Reporting and exports", Year2024 = 7.6, Year2025 = 7.8 });
        this.Add(new FeatureSatisfactionScoresItem() { Feature = @"Notifications", Year2024 = 6.9, Year2025 = 6.5 });
        this.Add(new FeatureSatisfactionScoresItem() { Feature = @"Integrations with other tools", Year2024 = 5.2, Year2025 = 6.8 });
        this.Add(new FeatureSatisfactionScoresItem() { Feature = @"Onboarding and help", Year2024 = 6.1, Year2025 = 7.4 });
        this.Add(new FeatureSatisfactionScoresItem() { Feature = @"Performance", Year2024 = 7.9, Year2025 = 8.6 });
    }
}
