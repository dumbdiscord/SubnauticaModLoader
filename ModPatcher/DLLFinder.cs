using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
public static class DLLFinder
{
    public static List<ModEntryPoint> mods = new List<ModEntryPoint>();
    
    public static void FindDLLs(string path)
    {
        var files = Directory.GetFiles(path);
        foreach (var f in files)
        {
            try
            {
                var curassembly = Assembly.LoadFile(f);
                foreach (var h in curassembly.GetTypes())
                {
                    if (h.BaseType.Equals( typeof(ModEntryPoint)))
                    {
                        var mod = Activator.CreateInstance(h) as ModEntryPoint;
                        mod.Path = Path.GetDirectoryName(f);
                        mods.Add(mod);
                    }
                }
            }
            catch { }
        }
        foreach(var f in Directory.GetDirectories(path))
        {
            FindDLLs(f);
        }
    }
    public static void PrepatchDLLs()
    {
        foreach(var v in mods)
        {
            v.Prepatch();
        }
    }
    public static void PatchDLLs()
    {
        foreach (var v in mods)
        {
            v.Patch();
        }
    }
    public static void PostPatchDLLs()
    {
        foreach (var v in mods)
        {
            v.PostPatch();
        }
    }
}

