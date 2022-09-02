using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCI.NSAPI;

/// <summary>
/// Exception, which happens, if NSAPI already initialized or in other situations
/// </summary>
[Serializable]
public class AlreadyImportedException : Exception
{
    public AlreadyImportedException() { }
    public AlreadyImportedException(string message) : base(message) { }
    public AlreadyImportedException(string message, Exception inner) : base(message, inner) { }
    protected AlreadyImportedException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}