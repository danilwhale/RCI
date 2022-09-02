using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCI.NSAPI;

/// <summary>
/// Default NSAPI exception
/// </summary>
[Serializable]
public class NSAPIException : Exception
{
    public NSAPIException() { }
    public NSAPIException(string message) : base(message) { }
    public NSAPIException(string message, Exception inner) : base(message, inner) { }
    protected NSAPIException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}