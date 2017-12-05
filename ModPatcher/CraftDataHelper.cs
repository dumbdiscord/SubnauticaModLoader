using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
public static class CraftDataHelper
{
    public static T GetCraftDataDictionary<T>(string name) where T: class
    {
        return typeof(CraftData).GetField(name, BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as T;
    }
}

