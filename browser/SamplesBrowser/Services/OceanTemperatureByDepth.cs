using System;
using System.Collections.Generic;
public class OceanTemperatureByDepthItem
{
    public string Depth { get; set; }
    public double Summer { get; set; }
    public double Winter { get; set; }
}

public class OceanTemperatureByDepth
    : List<OceanTemperatureByDepthItem>
{
    public OceanTemperatureByDepth()
    {
        this.Add(new OceanTemperatureByDepthItem() { Depth = @"0 m", Summer = 24.1, Winter = 12.3 });
        this.Add(new OceanTemperatureByDepthItem() { Depth = @"10 m", Summer = 23.6, Winter = 12.2 });
        this.Add(new OceanTemperatureByDepthItem() { Depth = @"20 m", Summer = 22, Winter = 12.1 });
        this.Add(new OceanTemperatureByDepthItem() { Depth = @"30 m", Summer = 18.9, Winter = 12 });
        this.Add(new OceanTemperatureByDepthItem() { Depth = @"50 m", Summer = 15.2, Winter = 11.7 });
        this.Add(new OceanTemperatureByDepthItem() { Depth = @"75 m", Summer = 12.4, Winter = 11.2 });
        this.Add(new OceanTemperatureByDepthItem() { Depth = @"100 m", Summer = 10.8, Winter = 10.5 });
        this.Add(new OceanTemperatureByDepthItem() { Depth = @"150 m", Summer = 9.4, Winter = 9.3 });
        this.Add(new OceanTemperatureByDepthItem() { Depth = @"200 m", Summer = 8.3, Winter = 8.3 });
        this.Add(new OceanTemperatureByDepthItem() { Depth = @"300 m", Summer = 7.1, Winter = 7.1 });
    }
}
