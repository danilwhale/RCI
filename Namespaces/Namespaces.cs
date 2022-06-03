// All basic namespaces
namespace Retro_Command_Interpretator.Namespaces;

public static class BasicNamespaces
{
    public static readonly Namespace[] namespaces =
    {
        new FileStreamNamespace(),
        new TimeNamespace()
    };
}
/// <summary>
/// Namespace to work with files
/// </summary>
public class FileStreamNamespace : Namespace
{
    /// <summary>
    /// Associating actions
    /// </summary>
    private static readonly Function.FuncAction[] funcacts =
    {
        (object? path) => read(Convert.ToString(path)),
        (object? pathcontent) => write(Convert.ToString(pathcontent)),
        (object? path) => create(Convert.ToString(path)),
        (object? path) => remove(Convert.ToString(path)),
        (object? path) => isexists(Convert.ToString(path)),
        (object? path) => info(Convert.ToString(path))
    };
    /// <summary>
    /// Associating actions to functions
    /// </summary>
    private static readonly Function[] funcs =
    {
        new(funcacts[0], "readFile"),
        new(funcacts[1], "writeFile"),
        new(funcacts[2], "createFile"),
        new(funcacts[3], "removeFile"),
        new(funcacts[4], "isExists"),
        new(funcacts[5], "info")
    };

    public FileStreamNamespace()
    {
        Name = "File";
        Functions = funcs;
    }

    private static void read(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new InterpreterException("Argument is empty");
            
        string[] lns = File.ReadAllLines(path);
        foreach (string line in lns)
        {
            Console.WriteLine($"{Array.IndexOf(lns, line) + 1}   | {line}");
        }
    }
    private static void write(string? pact)
    {
        if (string.IsNullOrWhiteSpace(pact))
            throw new InterpreterException("Argument is empty");

        string[] strings = pact.Split(';');
        strings[0] = strings[0].StartsWith(' ') ? strings[0].Remove(0, 1) : strings[0];
        File.WriteAllText(strings[0], strings[1].Replace(@"\n", "\n").Replace(@"\t", "\n"));
        RCI_Core.WriteSuccess("Sucessfully writed content to file");

    }
    private static void create(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new InterpreterException("Argument is empty");
        File.Create(path).Close();
        RCI_Core.WriteSuccess("Sucessflly created file");
    }
    private static void remove(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new InterpreterException("Argument is empty");
        File.Delete(path);
        RCI_Core.WriteSuccess("Sucessfully removed file");
    }
    private static void isexists(string? path)
    {
        if (File.Exists(path))
            Console.WriteLine($"File \"{path}\" exists");
        else
            Console.WriteLine($"File \"{path}\" doesn't exists");
    }
    private static void info(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new InterpreterException("Argument is empty");

        FileInfo info = new(path);
        Console.WriteLine(
            $"Information about \"{Path.GetFileName(path)}\"\n" +
            $"Extension: {Path.GetExtension(path)}\n" +
            $"Size: {info.Length} bytes\n" +
            $"Created: {info.CreationTime}\n" +
            $"Last edited: {info.LastWriteTime}\n" +
            $"Last accessed: {info.LastAccessTime}\n" +
            $"Is readonly: {RCI_Core.YesNo(info.IsReadOnly)}\n" +
            $"Attributes: {info.Attributes}"
        );
    }
}
/// <summary>
/// Namespace to get date/time
/// </summary>
public class TimeNamespace : Namespace
{
    private static readonly Function.FuncAction[] funcacts =
    {
        nowDate,
        nowTime
    };
    private static readonly Function[] funcs =
    {
        new(funcacts[0], "nowDate"),
        new(funcacts[1], "nowTime")
    };

    public TimeNamespace()
    {
        Name = "Time";
        Functions = funcs;
    }

    private static void nowDate(object? placeholder) =>
        Console.WriteLine(DateTime.Today.ToLongDateString());
    private static void nowTime(object? placeholder) =>
        Console.WriteLine(DateTime.Now.ToLongTimeString());
}