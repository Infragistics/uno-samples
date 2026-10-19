using System;
using System.Collections.Generic;
public class AtmosphereTemperatureByAltitudeItem
{
    public string Altitude { get; set; }
    public double Standard { get; set; }
    public double Tropical { get; set; }
}

public class AtmosphereTemperatureByAltitude
    : List<AtmosphereTemperatureByAltitudeItem>
{
    public AtmosphereTemperatureByAltitude()
    {
        this.Add(new AtmosphereTemperatureByAltitudeItem() { Altitude = @"0 km", Standard = 15, Tropical = 27 });
        this.Add(new AtmosphereTemperatureByAltitudeItem() { Altitude = @"1 km", Standard = 8.5, Tropical = 21.5 });
        this.Add(new AtmosphereTemperatureByAltitudeItem() { Altitude = @"2 km", Standard = 2, Tropical = 15.5 });
        this.Add(new AtmosphereTemperatureByAltitudeItem() { Altitude = @"3 km", Standard = -4.5, Tropical = 9.5 });
        this.Add(new AtmosphereTemperatureByAltitudeItem() { Altitude = @"4 km", Standard = -11, Tropical = 3 });
        this.Add(new AtmosphereTemperatureByAltitudeItem() { Altitude = @"5 km", Standard = -17.5, Tropical = -3.5 });
        this.Add(new AtmosphereTemperatureByAltitudeItem() { Altitude = @"6 km", Standard = -24, Tropical = -10 });
        this.Add(new AtmosphereTemperatureByAltitudeItem() { Altitude = @"7 km", Standard = -30.5, Tropical = -17 });
        this.Add(new AtmosphereTemperatureByAltitudeItem() { Altitude = @"8 km", Standard = -37, Tropical = -24.5 });
        this.Add(new AtmosphereTemperatureByAltitudeItem() { Altitude = @"9 km", Standard = -43.5, Tropical = -32.5 });
        this.Add(new AtmosphereTemperatureByAltitudeItem() { Altitude = @"10 km", Standard = -50, Tropical = -41 });
        this.Add(new AtmosphereTemperatureByAltitudeItem() { Altitude = @"11 km", Standard = -56.5, Tropical = -50 });
        this.Add(new AtmosphereTemperatureByAltitudeItem() { Altitude = @"12 km", Standard = -56.5, Tropical = -59 });
    }
}
