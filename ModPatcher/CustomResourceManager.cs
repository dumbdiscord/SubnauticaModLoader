using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using UnityEngine;
using System.Reflection;
public static class CustomResourceManager
{
    public static readonly List<CustomResource> customResources = new List<CustomResource>();
    public static void Patch(HarmonyInstance instance)
    {
        foreach(var v in customResources)
        {
            v.Patch();
        }
        PrefabDatabasePatcher.Patch(instance);
        EntTechDataPatcher.Patch(instance);
        LanguagePatcher.Patch(instance);

        ResourcesLoadPatcher.Patch(instance);
    }
   


}

