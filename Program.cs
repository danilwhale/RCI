
using System.Reflection;

namespace Retro_Command_Interpretator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = $"Retro Command Interpreter, version {Assembly.GetEntryAssembly()?.GetName().Version}";

            if (args.Length == 0)
            {
                Console.WriteLine(
                    $"RCI version: {Assembly.GetEntryAssembly()?.GetName().Version}\n" +
                    $".NET version: {Environment.Version}\n" +
                    $"Windows version: {Environment.OSVersion}\n\n" +
                    "Enter \"help\" to get list of commands\n" +
                    "Enter \"changelog\" to see what changed in versions\n");

                while (true)
                {
                    Console.Write($"{Environment.CurrentDirectory} > ");
                    string? input = Console.ReadLine();
                    string dir = Environment.CurrentDirectory;

                    Parser.ParseScript(input!);
                }
            }
            else
            {
                Console.WriteLine("Script execute mode\n\n");
                Parser.ParseScriptFile(args[0]); 
                Console.ReadKey();
            }
        }
    }
}
