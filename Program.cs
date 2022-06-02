using System.Reflection;

namespace Retro_Command_Interpretator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = $"Retro Command Interpreter, version {Assembly.GetEntryAssembly()?.GetName().Version}";

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

                if (input.StartsWith("sendOut"))
                {
                    if (input.Length < 8)
                        RCI_Core.WriteTip("Usage of \"sendOut\": sendOut <text>");
                    else
                    {
                        if (input[7] == ' ')
                            Console.WriteLine(input[(input.IndexOf(' ')+1)..]);
                        else
                            RCI_Core.WriteTip("Usage of \"sendOut\": sendOut <text>");
                    }
                    
                }
                else if (input.StartsWith("cleanScreen"))
                {
                    Console.Clear();
                }
                else if (input.StartsWith("help"))
                {
                    RCI_Core.SendHelp();
                }
                else if (input.StartsWith("whatsHere"))
                {
                    RCI_Core.PrintCurrentDirectory();
                }
                else if (input.StartsWith("changelog"))
                {
                    RCI_Core.Changelog();
                }
                else if (input.StartsWith("goIn"))
                {
                    if (input.Length < 6)
                        RCI_Core.WriteTip("Usage of \"goIn\": goIn <directoryName>");
                    else
                    {
                        if (input[4] != ' ')
                            RCI_Core.WriteTip("You need space before directory name");

                        if (Directory.Exists(input[5..]))
                            Environment.CurrentDirectory = input[5..];
                        else
                            RCI_Core.WriteError("Invalid directory");
                    }
                }
                else if (input.Contains("::"))
                {
                    RCI_Core.AccessFunction(input);
                }
                else if (string.IsNullOrWhiteSpace(input))
                {
                    RCI_Core.WriteError("Command is empty");
                }
                else
                {
                    RCI_Core.WriteError("Invalid command");
                }
            }
        }
    }
}
