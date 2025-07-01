using Nest.CSharp;
using Nest.Text;

// dotnet build -c Release && dotnet pack -c Release

namespace Nest.Demo
{
    internal class Program
    {
        static void Main()
        {
            var _ = new TextBuilder();

            _.L("using System.Text;");

            _.B("namespace MyProgram", _ =>
            {
                _.B("public class MyProgram", _ =>
                {
                    _.B("public static void Main(string[] args)", _ =>
                    {
                        _.B("if (count > 6)", _ =>
                        {
                            _.L("Console.WriteLine(`Hello World!`);");
                            _.L("Console.WriteLine(`Hello World!`);");
                        })
                        .B("else", _ =>
                        {
                            _.L("Console.WriteLine(`Hello World!`);");
                        });
                    });
                });
            });

            Console.WriteLine(_.ToString());
        }
    }
}
