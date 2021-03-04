using Tizen.Applications;
using Uno.UI.Runtime.Skia;

namespace FinancialGrid.Skia.Tizen
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new TizenHost(() => new FinancialGrid.App(), args);
            host.Run();
        }
    }
}
