// All basic namespaces
namespace Retro_Command_Interpretator.Namespaces;

/// <summary>
/// Namespace to work with files
/// </summary>
public static class FileStreamNamespace
{
    /// <summary>
    /// Associating actions
    /// </summary>
    private static Function.FuncAction[] funcacts =
    {
        (object path) => read(Convert.ToString(path)),
        (object pathcontent) => write(Convert.ToString(pathcontent)),
        (object path) => create(Convert.ToString(path)),
        (object path) => remove(Convert.ToString(path)),
        (object path) => isexists(Convert.ToString(path)),
        (object path) => info(Convert.ToString(path))
    };
    /// <summary>
    /// Associating actions to functions
    /// </summary>
    private static Function[] funcs =
    {
        new(funcacts[0], "readFile"),
        new(funcacts[1], "writeFile"),
        new(funcacts[2], "createFile"),
        new(funcacts[3], "removeFile"),
        new(funcacts[4], "isExists"),
        new(funcacts[5], "info")
    };
    public static readonly Namespace Namespace = new(funcs, "File");

    private static void read(string path)
    {
        string[] lns = File.ReadAllLines(path);
        foreach (string line in lns)
        {
            Console.WriteLine($"{Array.IndexOf(lns, line)} || {line}");
        }
    }
    private static void write(string pact)
    {
        string[] strings = pact.Split(';');
        strings[0] = strings[0].StartsWith(' ') ? strings[0].Remove(0, 1) : strings[0];
        File.WriteAllText(strings[0], strings[1].Replace(@"\n", "\n"));
        RCI_Core.WriteSuccess("Sucessfully writed content to file");
    }
    private static void create(string path)
    {
        File.Create(path).Close();
        RCI_Core.WriteSuccess("Sucessflly created file");
    }
    private static void remove(string path)
    {
        File.Delete(path);
        RCI_Core.WriteSuccess("Sucessfully removed file");
    }
    private static void isexists(string path)
    {
        if (File.Exists(path))
            Console.WriteLine($"File \"{path}\" exists");
        else
            Console.WriteLine($"File \"{path}\" doesn't exists");
    }
    private static void info(string path)
    {
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
public static class TimeNamespace
{
    private static Function.FuncAction[] funcacts =
    {
        (object ph) => nowDate(ph),
        (object ph) => nowTime(ph)
    };
    private static Function[] funcs =
    {
        new(funcacts[0], "nowDate"),
        new(funcacts[1], "nowTime")
    };

    public static readonly Namespace Namespace = new(funcs, "Time");

    private static void nowDate(object placeholder) =>
        Console.WriteLine(DateTime.Today.ToLongDateString());
    private static void nowTime(object placeholder) =>
        Console.WriteLine(DateTime.Now.ToLongTimeString());
}