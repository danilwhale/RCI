// All basic namespaces
using System.Runtime.CompilerServices;
using RCI.NSAPI;

namespace RCI.Namespaces;

public static class AllNamespaces
{
    public static Namespace[] namespaces { get; private set; }

    static AllNamespaces()
    {
        List<Namespace> rawNamespaces = new();

        foreach (IPlugin? plugin in NSAPI.NSAPI_Core.Plugins)
        {
            // ignore empty plugins
            if (plugin == null)
                continue;

            // add plugin namespaces
            rawNamespaces.AddRange(plugin.Namespaces);
        }

        namespaces = rawNamespaces.ToArray();
    }
}