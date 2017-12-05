using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using UnityEngine;
public static class CraftDataPatchHelper
{
    public static readonly Dictionary<TechType, TechDataWrapper> customTechData = new Dictionary<TechType, TechDataWrapper>();
    public static void Patch()
    {
        var actual = typeof(CraftData).GetField("techData", System.Reflection.BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
        var indexer = actual.GetType().GetProperty("Item");
        var add = actual.GetType().GetMethod("Add");
        foreach(var h in customTechData)
        {
            add.Invoke(actual, new object[] {h.Key,h.Value.ConvertToTechData() });
            //indexer.SetValue(actual, h.Value.ConvertToTechData(), new object[] { h.Key});
        }
    }
}

