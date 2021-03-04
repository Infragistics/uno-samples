using Tizen.Applications;
using Uno.UI.Runtime.Skia;

namespace ColorExplorer.Skia.Tizen
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new TizenHost(() => new ColorExplorer.App(), args);
            host.Run();
        }
    }
}
