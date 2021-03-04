using Tizen.Applications;
using Uno.UI.Runtime.Skia;

namespace FinancialChart.Skia.Tizen
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new TizenHost(() => new FinancialChart.App(), args);
            host.Run();
        }
    }
}
