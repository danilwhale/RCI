namespace RCI;

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
    /// Description of function
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Usage of function
    /// </summary>
    public string? Usage { get; set; }

    /// <summary>
    /// Function ctor
    /// </summary>
    /// <param name="action">Action to handle</param>
    /// <param name="name">Name of function</param>
    /// <param name="description">Description of function</param>
    /// <param name="usage">Usage of function (nullable)</param>
    public Function(FuncAction action, string name, string description, string? usage)
    {
        this.action = action;
        Name = name;
        Description=description;
        Usage=usage;
    }

    /// <summary>
    /// Method that executes function's delegate
    /// </summary>
    /// <param name="arguments">Arguments object</param>
    public void Execute(FunctionArgs arguments) => action(arguments);

    /// <summary>
    /// Function action delegate with arguments
    /// </summary>
    /// <param name="arguments">Arguments object</param>
    public delegate void FuncAction(FunctionArgs arguments);
}