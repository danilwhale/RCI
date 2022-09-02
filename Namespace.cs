using RCI.Namespaces;

namespace RCI;
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
    public string Name { get; set; } = null!;

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
    public static void Execute(Namespace[] namespaces, string _namespace, string _function, FunctionArgs arguments)
    {
        foreach (Namespace ns in namespaces)
        {
            if (ns.Name == _namespace)
            {
                try
                {
                    Function? func = ns[_function];

                    if (func != null)
                        func?.Execute(arguments);
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
