using System.Text;
using RCI.Namespaces;
using RCI.NSAPI;

namespace RCI;
public static class RCI_Core
{
    public const string CommandSeparator = "::";
    public const string changelog =
        """
        <Changelog is from GitHub>

        RCI 1.4
        - Added plugin support! \o/
        - - See "Plugins" folder for more information
        - Separated File and Time namespaces from RCI to plugin
        - Added "plugins" command to see all installed plugins

        RCI 1.3
        - Added ability to run script files (with .rci extension)
        - Now to run file/program you don't need to use run command
        - run command associated to run scripts instead of files/programs
        - Classes in source code seperated into files, instead of having all classes in RCI_Core.cs

        RCI 1.2.1
        - Changed version from `1.1` to `1.2.1` in project files lol

        RCI 1.2
        - Added 'rm' and 'md' commands
        - - 'rm' for removing file/directories
        - - 'md' for creating directories
        - Added 'run' command to run/open files in system
        - Now 'help' command uses raw string literal instad of many strings

        RCI 1.1
        - Changed function calling (from namespace->function: argument to namespace::function argument)
        - Changed syntaxis from C#-like to C/C++-like (CleanScreen to cleanScreen)
        - Added 2 new functions to File namespace
        - Added on-start information with RCI, .NET, OS versions
        - Improved namespace code

        RCI 1.0
        - Initial release
        """;
    /// <summary>
    /// Accesses function in namespace (for interpreter loop)
    /// </summary>
    /// <param name="input">Input string with command to access function</param>
    internal static void AccessFunction(string input)
    {
        string[] namefunc = input.Split(CommandSeparator); // splits to namespace and function

        if (namefunc.Length < 2 || string.IsNullOrWhiteSpace(namefunc[1])) // checking for invalid namespace usage
            WriteError("Invalid namespace usage");
        else
        {
            if (namefunc[1].Contains(' ')) // if this a function which can have arguments
            {
                string args = namefunc[1][(namefunc[1].IndexOf(' ') + 1) .. ]; // seems bad coded, but works, it gets argument of command

                if (args[0] == ' ')
                    args = args.Remove(0, 1);
                if (args[args.IndexOf(';') + 1] == ' ')
                    args = args.Remove(args.IndexOf(';') + 1, 1);

                namefunc[1] = namefunc[1].Split(' ')[0];

                Namespace.Execute(AllNamespaces.namespaces, namefunc[0], namefunc[1], new(args.Split(';'))); // executing it
            }
            else // else if not
            {
                Namespace.Execute(AllNamespaces.namespaces, namefunc[0], namefunc[1], new()); // we just send "bla" argument to function (lol)
            }
        }

    }
    /// <summary>
    /// Method that sends help in console
    /// </summary>
    internal static void SendHelp()
    {
        StringBuilder pluginHelp = new();

        foreach (IPlugin? plug in NSAPI_Core.Plugins)
        {
            if (plug == null)
                continue;

            pluginHelp.AppendLine($"\n\"{plug.Name}\" by \"{plug.Author}\":");
            foreach (Namespace ns in plug.Namespaces)
            {
                pluginHelp.AppendLine($"- \"{ns.Name}\" Namespace: ");
                foreach (Function function in ns.Functions)
                {
                    pluginHelp.AppendLine($"--- {function.Name} {function.Usage} - {function.Description}");
                }
            }
        }

        RCI_Core.WriteTip(
            """
            - Default commands:
            --- send <message> - prints message
            --- help - prints this message
            --- cls - cleans screen
            --- dir - print current directory content
            --- go <directory> - goes in to specified directory
            --- changelog - prints changes
            --- run <script>.rci - runs script file in RCI (confirming only .rci extensions)
            --- rm <dirname/filename> - removes specified file/empty directory
            --- md <dirname> - creates new directory with specified name
            --- plugins - prints all installed plugins with its namespaces and function count
            --- stop - closes RCI
            """ + 
            "\nNamespaces (NAMESPACE::FUNCTION ARGUMENT):" + "\n" + pluginHelp.ToString()
        ); ;

    }
    /// <summary>
    /// Method sends error to console
    /// </summary>
    /// <param name="error">Error text</param>
    public static void WriteError(string error)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(error);
        Console.ResetColor();
    }
    /// <summary>
    /// Method sends successfull message to console
    /// </summary>
    /// <param name="text">Information text</param>
    public static void WriteSuccess(string text)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(text);
        Console.ResetColor();
    }
    public static void WriteColored(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }
    public static void WriteTip(string text)
    {
        WriteColored(text, ConsoleColor.Blue);
    }
    /// <summary>
    /// Method that prints current directory content
    /// </summary>
    internal static void PrintCurrentDirectory()
    {
        Console.Clear();

        Console.WriteLine($"[{Environment.CurrentDirectory}]");
        for (int i = 0; i < Environment.CurrentDirectory.Length; i++)
            Console.Write('-');

        Console.WriteLine();

        int filecount = 0;

        foreach (string dir in Directory.EnumerateDirectories(Environment.CurrentDirectory))
        {
            Console.WriteLine($"[DIR] {Path.GetFileName(dir)}");
            filecount++;
        }
        foreach (string file in Directory.EnumerateFiles(Environment.CurrentDirectory))
        {
            Console.WriteLine($"      {Path.GetFileName(file)}");
            filecount++;
        }

        for (int i = 0; i < Environment.CurrentDirectory.Length; i++)
            Console.Write('-');

        Console.WriteLine();

        Console.WriteLine("Elements: " + filecount);
        for (int i = 0; i < Environment.CurrentDirectory.Length; i++)
            Console.Write('-');

        Console.WriteLine();
    }

    /// <summary>
    /// Converts true/false to Yes/No
    /// </summary>
    /// <param name="boolean"></param>
    /// <returns></returns>
    public static string YesNo(bool boolean)
    {
        return boolean ? "Yes" : "No";
    }

    internal static void Changelog()
    {
        Console.WriteLine(changelog);
    }
    public static string GetSizeString(long size)
    {
        string[] sizes =
        {
            "Byte",
            "KB",
            "MB",
            "GB",
            "TB"
        };

        int vsize = 0;

        while (size >= 1024 && vsize < sizes.Length - 1)
        {
            vsize++;
            size /= 1024;
        }

        return $"{size} {sizes[vsize]}";
    }
    internal static void PrintPlugins()
    {
        foreach (var plugin in NSAPI_Core.Plugins)
        {
            if (plugin == null)
                continue;

            StringBuilder namespaces = new();

            foreach (var _namespace in plugin.Namespaces)
            {
                namespaces.AppendLine($"- \"{_namespace.Name}\", {_namespace.Functions.Length} function(s)");
            }

            Console.WriteLine($"\"{plugin.Name}\" {plugin.Version} by \"{plugin.Author}\"\n{namespaces}");
        }
    }
}


