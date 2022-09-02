using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCI.NSAPI;

/// <summary>
/// Base interface for plugins
/// </summary>
public interface IPlugin
{
    /// <summary>
    /// Name of plugin instance
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Version of plugin instance
    /// </summary>
    public Version Version { get; }

    /// <summary>
    /// Author of plugin instance
    /// </summary>
    public string Author { get; }

    /// <summary>
    /// Namespaces, that avaible in this plugin
    /// </summary>
    public Namespace[] Namespaces { get; }

    /// <summary>
    /// Executes provided namespace in plugin namespaces
    /// </summary>
    /// <param name="namespaceName">Namespace to run function from</param>
    /// <param name="functionName">Function to run</param>
    /// <param name="arguments">Arguments of function</param>
    //public void ExecuteNamespace(string namespaceName, string functionName, FunctionArgs arguments);
}