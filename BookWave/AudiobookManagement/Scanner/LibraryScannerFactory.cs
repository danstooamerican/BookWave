using BookWave.Desktop.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace BookWave.Desktop.AudiobookManagement.Scanner
{
    public class LibraryScannerFactory
    {
        private static readonly string DefaultScanner = "BookWave.Scanner.Default";

        private static IDictionary<string, LibraryScanner> scanners = new Dictionary<string, LibraryScanner>();

        public static LibraryScanner GetScanner(string key)
        {
            if (scanners.ContainsKey(key))
            {
                return scanners[key];
            }

            throw new ScannerNotFoundException(key, "not found");
        }

        public static LibraryScanner GetDefault()
        {
            return scanners[DefaultScanner];
        }

        public static ICollection<LibraryScanner> GetAllScanners()
        {
            return scanners.Values;
        }

        public static void LoadPlugins()
        {
            scanners.Add(DefaultScanner, new AudiobooksTopScanner());
            LoadPluginsFolder();
        }

        private static void LoadPluginsFolder()
        {
            string pluginFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "BookWave", "addons");

            string[] dllFileNames;
            if (Directory.Exists(pluginFolderPath))
            {
                dllFileNames = Directory.GetFiles(pluginFolderPath, "*.dll", SearchOption.AllDirectories);
            }
            else
            {
                return;
            }

            ICollection<Assembly> assemblies = new List<Assembly>(dllFileNames.Length);
            foreach (string dllFile in dllFileNames)
            {
                AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
                Assembly assembly = Assembly.Load(an);
                assemblies.Add(assembly);
            }

            Type pluginType = typeof(LibraryScanner);
            ICollection<Type> pluginTypes = new List<Type>();
            foreach (Assembly assembly in assemblies)
            {
                if (assembly != null)
                {
                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        if (type.IsInterface || type.IsAbstract)
                        {
                            continue;
                        }
                        else
                        {
                            if (type.IsSubclassOf(pluginType))
                            {
                                pluginTypes.Add(type);
                            }
                        }
                    }
                }
            }

            foreach (Type type in pluginTypes)
            {
                LibraryScanner plugin = (LibraryScanner)Activator.CreateInstance(type);
                if (!scanners.ContainsKey(plugin.GetIdentifier()))
                {
                    scanners.Add(plugin.GetIdentifier(), plugin);
                }                
            }
        }

    }
}
