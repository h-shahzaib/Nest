using Nest.Text;

// dotnet build -c Release && dotnet pack -c Release

namespace Nest.Demo
{
    internal class Program
    {
        static void Main()
        {
            var a = new TextBuilder();
            a.Options.BlockStyle = BlockStyle.IndentOnly;

            a.L("Library:").B(b =>
            {
                b.L("name: `Nest`");
                b.L("uses: `Structured Text O`");

                b.L("features:").B(c =>
                {
                    c.L("- Automated Indentation");
                    c.L("- Easy To Use");
                    c.L("- Zero Dependency");
                });
            });

            Console.WriteLine(a.ToString());
        }
    }
}
