using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using UnityEngine;
using Harmony;
using UWE;
public static class PrefabDatabasePatcher
{
    public static readonly List<CustomResource> customPrefabs = new List<CustomResource>();
    public static MethodInfo TargetMethod()
    {
        return typeof(PrefabDatabase).GetMethod("LoadPrefabDatabase");
    }
    public static void Patch(HarmonyInstance instance)
    {
        instance.Patch(TargetMethod(), new HarmonyMethod(typeof(PrefabDatabasePatcher).GetMethod("Prefix")), new HarmonyMethod(typeof(PrefabDatabasePatcher).GetMethod("PostFix")));   
    }
    public static void Prefix()
    {

    }
    public static void PostFix()
    {
        foreach(var v in customPrefabs)
        {
            Debug.Log("Patching Custom Resource " + v.Path + " " + v.Key);
            PrefabDatabase.AddToCache(v.Path, v.LoadResource() as GameObject);
            PrefabDatabase.prefabFiles[v.Key]= v.Path;
            
        }

        
    }
}

