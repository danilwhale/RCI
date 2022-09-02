namespace RCI;

/// <summary>
/// Exception which raises when function is invalid (for now)
/// </summary>
public class InterpreterException : Exception
{
    public InterpreterException() { }
    public InterpreterException(string message) : base(message) { }
    public InterpreterException(string message, Exception inner) : base(message, inner) { }
}
