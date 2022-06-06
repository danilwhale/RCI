using Retro_Command_Interpretator.Namespaces;

namespace Retro_Command_Interpretator;
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
