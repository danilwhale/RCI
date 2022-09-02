using System.Reflection;

namespace RCI.NSAPI;

/// <summary>
/// Root of all plugins
/// </summary>
public static class NSAPI_Core
{
    public const string PluginRoot = "Plugins/";
    public static bool Initialized { get; private set; }
    public static List<IPlugin?> Plugins { get; private set; } = new();


    public static void ImportPlugins()
    {
        if (Initialized)
            throw new AlreadyImportedException("Plugins already imported");

        try
        {
            string[] pluginsDirs = Directory.GetDirectories(PluginRoot);

            for (int i = 0; i < pluginsDirs.Length; i++)
            {
                string[] dlls = Directory.GetFiles(pluginsDirs[i], "*.dll");
                foreach (string s in dlls)
                {
                    try
                    {
                        Assembly plugAssembly = Assembly.LoadFile(Path.GetFullPath(s));

                        RCI_Core.WriteTip(
                            $"Founded plugin \"{plugAssembly.GetName().Name}\".\n");

                        Type[] classes = plugAssembly.GetExportedTypes()
                            .Where(t => t.IsAssignableTo(typeof(IPlugin)))
                            .ToArray();

                        if (classes.Length > 0)
                        {
                            foreach (Type t in classes)
                            {
                                var plg = (IPlugin?) Activator.CreateInstance(t);
                                Plugins.Add(plg);

                                RCI_Core.WriteSuccess($"Imported \"{plg?.Name}\" {plg?.Version} by \"{plg?.Author}\" (part of \"{plugAssembly.GetName().Name}\")");
                            }
                        }
                        else
                        {
                            RCI_Core.WriteColored(
                                $"Assembly \"{plugAssembly.GetName().Name}\" doesn't have required features to work.",
                                ConsoleColor.DarkYellow);
                        }
                    }
                    catch (Exception ex)
                    {
                        RCI_Core.WriteError(
                            $"Cant import \"{Path.GetFileNameWithoutExtension(s)}\":\n" +
                            $"{ex.Message}");
                    }

                }

            }
            Initialized = true;
        }
        catch (Exception ex)
        {
            RCI_Core.WriteError("Can't load plugins\nException: " + ex);
        }
    }
    public static Namespace? FindNamespace(string namespaceName)
    {
        foreach (IPlugin? plugin in Plugins)
        {
            if (plugin == null)
                continue;

            foreach (Namespace ns in plugin.Namespaces)
            {
                if (ns.Name == namespaceName)
                    return ns;
            }
        }

        return null;
    }
}