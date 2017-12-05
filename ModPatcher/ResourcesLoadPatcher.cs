using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using UnityEngine;
using System.Reflection;
public static class ResourcesLoadPatcher
{
    public static void Patch(HarmonyInstance instance)
    {
        foreach (var h in typeof(UnityEngine.Resources).GetMethods())
        {
            if (h.Name == "Load")
            {

                if (h.IsGenericMethod)
                {

                    if (h.GetParameters().Length == 1)
                    {

                        if (h.GetGenericArguments().Length == 1)
                        {
                            var target = h.MakeGenericMethod(typeof(UnityEngine.Object));
                            

                            instance.Patch(target, new HarmonyMethod(typeof(ResourcesLoadPatcher).GetMethod("Prefix")), null);

                            break;
                        }
                    }
                }
            }
        }
        var targ = typeof(ResourceRequest).GetProperty("asset").GetAccessors()[0];

        instance.Patch(targ,new HarmonyMethod(typeof(ResourcesLoadPatcher).GetMethod("PrefixNonGeneric")),null);
        foreach (var h in typeof(UnityEngine.Resources).GetMethods())
        {
            if (h.Name == "LoadAsync")
            {

                if (h.IsGenericMethod)
                {

                    if (h.GetParameters().Length == 1)
                    {

                        if (h.GetGenericArguments().Length == 1)
                        {
                            var target = h.MakeGenericMethod(typeof(UnityEngine.Object));


                            instance.Patch(target, new HarmonyMethod(typeof(ResourcesLoadPatcher).GetMethod("PrefixAsync")), null);

                            break;
                        }
                    }
                }
            }
        }
    }
    public static bool Prefix(ref UnityEngine.Object __result, string path)
    {
        CustomResource h = null;
        for (int i = 0; i < CustomResourceManager.customResources.Count; i++)
        {
            if(path.ToLowerInvariant() == CustomResourceManager.customResources[i].Path.ToLowerInvariant())
            {
                h = CustomResourceManager.customResources[i];
                break;
            }
        }
        if (h != null)
        {
            Debug.Log("Loading Custom Resource " + path);
             __result= h.LoadResource();
            return false;
        }
        return true;
    }
    public static ResourceRequest CreateRequest(string path, Type type)
    {
        ResourceRequest output = new ResourceRequest();
        typeof(ResourceRequest).GetField("m_Path", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(output, path);
        typeof(ResourceRequest).GetField("m_Type", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(output, type);
        return output;
    }
    public static bool PrefixNonGeneric(ref UnityEngine.Object __result,ResourceRequest __instance)
    {
        String path = (string)typeof(ResourceRequest).GetField("m_Path", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);
        CustomResource h = null;
        for (int i = 0; i < CustomResourceManager.customResources.Count; i++)
        {
            if (path.ToLowerInvariant() == CustomResourceManager.customResources[i].Path.ToLowerInvariant())
            {
                h = CustomResourceManager.customResources[i];
                break;
            }
        }
        if (h != null)
        {
            Debug.Log("Loading Custom Resource " + path);
            __result = h.LoadResource();
            return false;
        }
        return true;


    }
    public static bool PrefixAsync(ref ResourceRequest __result, string path)
    {
        CustomResource h = null;
        for (int i = 0; i < CustomResourceManager.customResources.Count; i++)
        {
            if (path.ToLowerInvariant() == CustomResourceManager.customResources[i].Path.ToLowerInvariant())
            {
                h = CustomResourceManager.customResources[i];
                break;
            }
        }
        if (h != null)
        {
            Debug.Log("Loading Custom Resource " + path);
            __result = CreateRequest(path, typeof(GameObject));
            return false;
        }
        return true;
    }
}

