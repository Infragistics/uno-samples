using System;
using System.Collections.Generic;
public class ProfitBridgeItem
{
    public string Step { get; set; }
    public double Total { get; set; }
}

public class ProfitBridge
    : List<ProfitBridgeItem>
{
    public ProfitBridge()
    {
        this.Add(new ProfitBridgeItem() { Step = @"Revenue", Total = 1200 });
        this.Add(new ProfitBridgeItem() { Step = @"Cost of sales", Total = 720 });
        this.Add(new ProfitBridgeItem() { Step = @"Operating expenses", Total = 410 });
        this.Add(new ProfitBridgeItem() { Step = @"Research", Total = 260 });
        this.Add(new ProfitBridgeItem() { Step = @"Other income", Total = 300 });
        this.Add(new ProfitBridgeItem() { Step = @"Taxes", Total = 230 });
    }
}
