using Retro_Command_Interpretator.Namespaces;

namespace Retro_Command_Interpretator;
public static class RCI_Core
{
    static string changelog =
        """
        
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
    public static void AccessFunction(string input)
    {
        string[] namefunc = input.Split("::"); // splits to namespace and function

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

                Namespace.Execute(namefunc[0], namefunc[1], args); // executing it
            }
            else // else if not
            {
                Namespace.Execute(namefunc[0], namefunc[1], null); // we just send "bla" argument to function (lol)
            }
        }

    }
    /// <summary>
    /// Method that sends help in console
    /// </summary>
    public static void SendHelp()
    {
        Console.WriteLine(
            "Default commands:\n" +
            "-- sendOut: <message> - prints message\n" +
            "-- help - prints this message\n" +
            "-- cleanScreen - cleans screen\n" +
            "-- whatsHere - print current directory content\n" +
            "-- goIn <directory> - goes in to specified directory\n" +
            "-- changelog - prints changes\n" +
            "-- run <file> - runs file out of RCI" +
            "Namespaces (NAMESPACE::FUNCTION ARGUMENT):\n" +
            "- Time namespace:\n" +
            "-- nowDate - prints date\n" +
            "-- nowTime - prints time\n" +
            "- File namespace:\n" +
            "-- readFile <file> - reads file and prints it content\n" +
            "-- writeFile <file>; <text> - writes specified text to file\n" +
            "-- createFile <file> - creates file with specified name\n" +
            "-- removeFile <file> - removes specified file\n" +
            "-- isExists <file> - writes that file exists or doesn't exists\n" +
            "-- info <file> - writes info about file"  
        );

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
    public static void PrintCurrentDirectory()
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

    public static void Changelog()
    {
        Console.WriteLine(changelog);
    }
}
/// <summary>
/// Instance of Namespace
/// </summary>
public class Namespace
{
    /// <summary>
    /// Array of functions
    /// </summary>
    public Function[] Functions = Array.Empty<Function>();
    /// <summary>
    /// Name of namespace
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Indexator to get function in this namespace
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Function? this[string name]
    {
        get { return Array.Find(Functions, f => f.Name == name); }
    }
    /// <summary>
    /// Executes function in namespace
    /// </summary>
    /// <param name="_namespace">Name of namespace</param>
    /// <param name="_function">Name of function</param>
    /// <param name="argument">Argument</param>
    /// <exception cref="InterpreterException">If, function invalid</exception>
    public static void Execute(string _namespace, string _function, object? argument)
    {
        foreach (Namespace ns in BasicNamespaces.namespaces)
        {
            if (ns.Name == _namespace)
            {
                try
                {
                    Function? func = ns[_function];

                    if (func != null)
                        func?.Execute(argument);
                    else
                        throw new InterpreterException($"Invalid function");
                }
                catch (InterpreterException ex)
                {
                    RCI_Core.WriteError(ex.Message);
                }
                catch (Exception ex)
                {
                    RCI_Core.WriteError(ex.ToString());
                }
            }
        }
    }
}

/// <summary>
/// Instance of Function
/// </summary>
public class Function
{
    /// <summary>
    /// Action of function
    /// </summary>
    private readonly FuncAction action;

    /// <summary>
    /// Name of function
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Function ctor (constructor)
    /// </summary>
    /// <param name="action">Action to handle</param>
    /// <param name="name">Name of function</param>
    public Function(FuncAction action, string name)
    {
        this.action = action;
        Name = name;
    }

    /// <summary>
    /// Method that executes function's delegate
    /// </summary>
    /// <param name="argument">Argument to delegate</param>
    public void Execute(object? argument) => action(argument);

    /// <summary>
    /// Function action delegate
    /// </summary>
    /// <param name="argument">Argument to delegate</param>
    public delegate void FuncAction(object? argument);
}

/// <summary>
/// Exception which raises when function is invalid (for now)
/// </summary>
public class InterpreterException : Exception
{
    public InterpreterException() { }
    public InterpreterException(string message) : base(message) { }
    public InterpreterException(string message, Exception inner) : base(message, inner) { }
}