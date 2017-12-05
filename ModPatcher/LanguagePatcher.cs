using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LitJson;
using Harmony;
using System.Reflection;
public static class LanguagePatcher
{


    public static readonly Dictionary<string, string> customLanguageLines = new Dictionary<string, string>();
    public static void Patch(HarmonyInstance instance)
    {
        instance.Patch(TargetMethod(), null, new HarmonyMethod(typeof(LanguagePatcher).GetMethod("PostFix")));
    }
    public static MethodInfo TargetMethod()
    {
        return typeof(Language).GetMethod("LoadLanguageFile", BindingFlags.Instance | BindingFlags.NonPublic);
    }
    public static void PostFix(Language __instance)
        
    {
        var strings = typeof(Language).GetField("strings", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance) as Dictionary<string,string>;
        foreach(var a in customLanguageLines)
        {
            strings[a.Key] = a.Value;
        }
    }
}

