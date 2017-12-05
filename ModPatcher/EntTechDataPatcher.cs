using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using System.Reflection;
using UnityEngine;
using System.IO;
public static class EntTechDataPatcher
{
    static bool haspatched = false;
    public static readonly Dictionary<string, TechType> customTechData = new Dictionary<string, TechType>();
    public static void Patch(HarmonyInstance instance)
    {
        instance.Patch(TargetMethod(),null, new HarmonyMethod(typeof(EntTechDataPatcher).GetMethod("PostFix")));
    }
    public static MethodInfo TargetMethod()
    {
        return typeof(CraftData).GetMethod("PrepareEntTechCache", BindingFlags.NonPublic | BindingFlags.Static);
    }
    public static void Prefix(bool __state)
    {
        var val = typeof(CraftData).GetField("entTechMap", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as Dictionary<string, TechType>;
        __state = val.Count == 0;
    }
    public static void PostFix()
    {
        if (!haspatched)
        {
            
            var val = typeof(CraftData).GetField("entTechMap", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as Dictionary<string, TechType>;
            foreach (var v in customTechData)
            {
                Debug.Log("Adding Custom TechData " + v.Key + " " + v.Value.AsString());
                val[Path.GetFileName(v.Key.ToLowerInvariant())] = v.Value;
            }
            haspatched = true;
        }
    }
}

