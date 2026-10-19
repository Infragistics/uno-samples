using System;
using System.Collections.Generic;
public class WebsiteVisitsByHourItem
{
    public string Hour { get; set; }
    public double Desktop { get; set; }
    public double Mobile { get; set; }
}

public class WebsiteVisitsByHour
    : List<WebsiteVisitsByHourItem>
{
    public WebsiteVisitsByHour()
    {
        this.Add(new WebsiteVisitsByHourItem() { Hour = @"00:00", Desktop = 120, Mobile = 200 });
        this.Add(new WebsiteVisitsByHourItem() { Hour = @"01:00", Desktop = 121, Mobile = 201 });
        this.Add(new WebsiteVisitsByHourItem() { Hour = @"02:00", Desktop = 125, Mobile = 206 });
        this.Add(new WebsiteVisitsByHourItem() { Hour = @"03:00", Desktop = 138, Mobile = 222 });
        this.Add(new WebsiteVisitsByHourItem() { Hour = @"04:00", Desktop = 171, Mobile = 268 });
        this.Add(new WebsiteVisitsByHourItem() { Hour = @"05:00", Desktop = 242, Mobile = 362 });
        this.Add(new WebsiteVisitsByHourItem() { Hour = @"06:00", Desktop = 371, Mobile = 503 });
        this.Add(new WebsiteVisitsByHourItem() { Hour = @"07:00", Desktop = 562, Mobile = 641 });
        this.Add(new WebsiteVisitsByHourItem() { Hour = @"08:00", Desktop = 787, Mobile = 701 });
        this.Add(new WebsiteVisitsByHourItem() { Hour = @"09:00", Desktop = 990, Mobile = 651 });
        this.Add(new WebsiteVisitsByHourItem() { Hour = @"10:00", Desktop = 1115, Mobile = 551 });
        this.Add(new WebsiteVisitsByHourItem() { Hour = @"11:00", Desktop = 1145, Mobile = 506 });
        this.Add(new WebsiteVisitsByHourItem() { Hour = @"12:00", Desktop = 1114, Mobile = 549 });
        this.Add(new WebsiteVisitsByHourItem() { Hour = @"13:00", Desktop = 1066, Mobile = 575 });
        this.Add(new WebsiteVisitsByHourItem() { Hour = @"14:00", Desktop = 1016, Mobile = 500 });
        this.Add(new WebsiteVisitsByHourItem() { Hour = @"15:00", Desktop = 942, Mobile = 393 });
        this.Add(new WebsiteVisitsByHourItem() { Hour = @"16:00", Desktop = 817, Mobile = 383 });
        this.Add(new WebsiteVisitsByHourItem() { Hour = @"17:00", Desktop = 646, Mobile = 520 });
        this.Add(new WebsiteVisitsByHourItem() { Hour = @"18:00", Desktop = 466, Mobile = 778 });
        this.Add(new WebsiteVisitsByHourItem() { Hour = @"19:00", Desktop = 316, Mobile = 1072 });
        this.Add(new WebsiteVisitsByHourItem() { Hour = @"20:00", Desktop = 215, Mobile = 1272 });
        this.Add(new WebsiteVisitsByHourItem() { Hour = @"21:00", Desktop = 159, Mobile = 1272 });
        this.Add(new WebsiteVisitsByHourItem() { Hour = @"22:00", Desktop = 134, Mobile = 1072 });
        this.Add(new WebsiteVisitsByHourItem() { Hour = @"23:00", Desktop = 124, Mobile = 777 });
    }
}
