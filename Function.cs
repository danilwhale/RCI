namespace Retro_Command_Interpretator;

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