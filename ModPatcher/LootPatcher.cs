using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using System.Reflection;

public static class LootPatcher
{
    public static readonly Dictionary<string, LootDistributionData.SrcData> customLootTables = new Dictionary<string, LootDistributionData.SrcData>();
    public static void Patch(HarmonyInstance instance)
    {
        instance.Patch(TargetMethod(), null, new HarmonyMethod(typeof(LootPatcher).GetMethod("PostFix")));
    }
    public static MethodInfo TargetMethod()
    {
        return typeof(LootDistributionData).GetMethod("ParseJson", BindingFlags.Static | BindingFlags.Public);
    }
    public static void PostFix(Dictionary<string,LootDistributionData.SrcData> __result)
    {
        foreach(var c in customLootTables)
        {
            __result.Add(c.Key, c.Value);
        }
    }
}

