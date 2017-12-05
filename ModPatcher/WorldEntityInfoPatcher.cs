using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using System.Reflection;

public static  class WorldEntityInfoPatcher
{
    public static readonly List<UWE.WorldEntityInfo> customWorldEntityInfo = new List<UWE.WorldEntityInfo>();
    public static void Patch(HarmonyInstance instance)
    {
        instance.Patch(typeof(UWE.WorldEntityDatabase).GetConstructor(new Type[0]), null, new HarmonyMethod(typeof(WorldEntityInfoPatcher).GetMethod("PostFix")));
    }
    public static void PostFix(UWE.WorldEntityDatabase __instance)
    {
        foreach(var c in customWorldEntityInfo)
        {
            __instance.infos[c.classId] = c;
        }
    }
}

