using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Harmony;

public static class TechTypePatcher
{
    public static readonly Dictionary<TechType, string> CustomTechTypes = new Dictionary<TechType, string>();
    public static void PostFix_GetValues(ref Array __result,Type enumType)
    {
        if (enumType.Equals(typeof(TechType)))
        {
            Array v = CustomTechTypes.Keys.ToArray();
            int length = __result.Length;
            var newarray = new TechType[length+v.Length];
            Array.Copy(__result, newarray, length);
            __result = newarray;
            for (int i = 0; i < v.Length; i++) {
                TechType propertype = (TechType)((v.GetValue(i)));
                __result.SetValue(propertype, i + length);

            }
        }
    }
    public static bool Prefix_ToString(Enum __instance,ref string __result)
    {
        if (__instance.GetType().Equals(typeof(TechType)))
        {
            TechType t = (TechType)__instance;
            if (CustomTechTypes.ContainsKey(t))
            {
                __result = CustomTechTypes[t];
                return false;
            }
        }
        return true;
    }
    public static bool Prefix_IsDefined(Type enumType, object value,ref bool __result)
    {
        if (enumType.Equals(typeof(TechType)))
        {
            var ret = CustomTechTypes.ContainsKey((TechType)value);
            
            if (ret)
            {
                __result = ret;
                return false;
            }
            
        }
        return true;
    }
    public static bool Prefix_Parse(Type enumType,string value,bool ignoreCase,ref object __result)
    {
        try
        {
            __result = null;
            if (enumType.Equals(typeof(TechType)))
            {

                if (ignoreCase)
                {

                    //__result = CustomTechTypes.First((x) => x.Value.ToLower() == value.ToLower());
                    foreach(var v in CustomTechTypes)
                    {
                        if (v.Value.ToLower() == value.ToLower())
                        {
                            __result = v.Key;
                        }
                    }
                }
                else
                {
                    foreach (var v in CustomTechTypes)
                    {
                        if (v.Value == value)
                        {
                            __result = v.Key;
                        }
                    }
                }
                if (__result != null)
                {
                    return false;
                }
                

            }
        }
        catch
        {

        }
        return true; 
    }
    public static void Patch(HarmonyInstance instance)
    {

        var target = typeof(Enum).GetMethod("GetValues", BindingFlags.Static | BindingFlags.Public);
        instance.Patch(target,null, new HarmonyMethod(typeof(TechTypePatcher).GetMethod("PostFix_GetValues")));
        var target2 = typeof(TechType).GetMethod("ToString",new Type[0]);
        instance.Patch(target2, new HarmonyMethod(typeof(TechTypePatcher).GetMethod("Prefix_ToString")),null);
        var target3 = typeof(Enum).GetMethod("Parse", new Type[] { typeof(Type), typeof(string), typeof(bool) });
        instance.Patch(target3, new HarmonyMethod(typeof(TechTypePatcher).GetMethod("Prefix_Parse")), null);
        var target4 = typeof(Enum).GetMethod("IsDefined", BindingFlags.Static|BindingFlags.Public);
        instance.Patch(target4, new HarmonyMethod(typeof(TechTypePatcher).GetMethod("Prefix_IsDefined")), null);
        var values = CustomTechTypes.Keys.ToArray();
        var type = typeof(TechTypeExtensions);
        var flags = BindingFlags.Static | BindingFlags.NonPublic;
        var stringsNormal = type.GetField("stringsNormal", flags).GetValue(null) as Dictionary<TechType, string>;
        var stringsLowerCase = type.GetField("stringsLowercase", flags).GetValue(null) as Dictionary<TechType, string>;
        var techTypesNormal = type.GetField("techTypesNormal", flags).GetValue(null) as Dictionary<string,TechType>;
        var techTypesIgnoreCase = type.GetField("techTypesIgnoreCase", flags).GetValue(null) as Dictionary<string, TechType>;
        var techTypeKeys = type.GetField("techTypeKeys", flags).GetValue(null) as Dictionary<TechType, string>;
        var keyTechTypes = type.GetField("keyTechTypes", flags).GetValue(null) as Dictionary<string, TechType>;
        for(int i = 0; i < values.Length; i++)
        {
            string key = values.GetValue(i).ToString();
            TechType value = (TechType)values.GetValue(i);
            
            stringsNormal[value] = key;
            stringsLowerCase[value] = key.ToLower();
            techTypesNormal[key] = value;
            techTypesIgnoreCase[key] = value;
            string key3 = ((int)value).ToString();
            techTypeKeys[value] = key3;
            keyTechTypes[key3] = value;
        }
    }
}
