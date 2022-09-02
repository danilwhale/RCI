using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCI;
public class FunctionArgs
{
    public object[] Arguments { get; set; } = null!;

    public FunctionArgs() { Arguments = null!; }
    public FunctionArgs(object[] arguments)
    {
        Arguments=arguments;
    }

    public object this[int index] { get => Arguments[index]; }
    public string? String(int index) =>
        Convert.ToString(this[index]);
}
