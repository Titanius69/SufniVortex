using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SufniVortex.Extensions
{
    public interface IBrowserExtension
    {
        void Initialize(Form browserForm);
    }

    public static class BrowserExtensionLoader
    {
        public static void LoadExtensions(Form browserForm)
        {
            string extensionsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "extensions");
            string configFile = Path.Combine(extensionsFolder, "extensions.txt");

            if (!Directory.Exists(extensionsFolder))
            {
                Directory.CreateDirectory(extensionsFolder);
            }

            if (!File.Exists(configFile))
            {
                File.WriteAllText(configFile, "");
            }

            var allowedExtensions = new HashSet<string>(
                File.ReadAllLines(configFile)
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrWhiteSpace(line) && !line.StartsWith("#"))
            );

            var dllFiles = Directory.GetFiles(extensionsFolder, "*.dll")
                                    .Where(dll => allowedExtensions.Contains(Path.GetFileName(dll)));

            foreach (var dll in dllFiles)
            {
                try
                {
                    Console.WriteLine($"[INFO] Loading extension: {dll}");
                    Assembly assembly = Assembly.LoadFrom(dll);
                    var extensionTypes = assembly.GetTypes()
                        .Where(t => typeof(IBrowserExtension).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

                    foreach (var type in extensionTypes)
                    {
                        IBrowserExtension extension = (IBrowserExtension)Activator.CreateInstance(type);
                        extension.Initialize(browserForm);
                        Console.WriteLine($"[SUCCESS] Loaded extension: {type.Name}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Failed to load extension from {dll}:\n{ex.Message}",
                        "Extension Load Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    Console.WriteLine($"[ERROR] Failed to load {dll}: {ex.Message}");
                }
            }
        }
    }
}
