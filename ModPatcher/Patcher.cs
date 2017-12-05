using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using UnityEngine;
using System.Collections;
using System.Threading;
using System.Reflection;
using System.IO;
public static class Patcher
{
    public static Thread MainThread;
    // todo: patch lootdistribution jsonmapper to insert my own loot data, patch entitycell deserializer to insert new entities into new cells, patch worldentityinfo to make loot work correctly, patch PDAData to insert my own pda entries
    public static void Patch()
    {

        if (!Directory.Exists("Mods"))
        {
            Debug.Log("ModLoader: Folder \"Mods\" not found! Aborting Patch...");
            return;
        }
        
        MainThread = Thread.CurrentThread;
        var instance = HarmonyInstance.Create("com.railway.moreeggs");
        DLLFinder.FindDLLs("Mods");
        
        //aterParkCreature.waterParkCreatureParameters[TechType.ReaperLeviathan] = new WaterParkCreatureParameters(0.01f, 0.05f, 0.1f, 5f, false);
        //WaterParkCreature.waterParkCreatureParameters[TechType.GhostLeviathan] = new WaterParkCreatureParameters(0.01f, 0.03f, 0.1f, 5f, false);
        //WaterParkCreature.waterParkCreatureParameters[TechType.SeaDragon] = new WaterParkCreatureParameters(0.01f, 0.02f, 0.1f, 5f, false);
        //WaterParkCreature.waterParkCreatureParameters[TechType.Warper] = new WaterParkCreatureParameters(0.02f, 0.25f, 0.5f, 1f, false);
        //WaterParkCreature.waterParkCreatureParameters[TechType.SeaTreader] = new WaterParkCreatureParameters(0.03f, 0.2f, 0.5f, 1f, false);
        //WaterParkCreature.creatureEggs[TechType.GhostRayBlue] = (TechType)9999;
        //WaterParkCreature.creatureEggs[TechType.GhostRayRed] = (TechType)6969;


        Application.runInBackground = true;

        AlternativeSerializer.RegisterCustomSerializer(-5, typeof(ScaleFixer), new ScaleFixerSerializer());
        //var h = typeof(CraftData).GetField("blacklist",BindingFlags.Static|BindingFlags.NonPublic).GetValue(null) as HashSet<TechType>;
        //h.Clear();
        DLLFinder.PrepatchDLLs();

        //Debug.Log("doing " + CustomResourceManager.customResources[1].LoadResource().name);
        //var targ = typeof(UWE.PrefabDatabase).GetMethod("GetPrefabForFilenameAsyncImpl", BindingFlags.NonPublic | BindingFlags.Static);
        //instance.Patch(targ,null, new HarmonyMethod(typeof(Patcher).GetMethod("Prefix")));
        TechTypePatcher.Patch(instance);

        //Debug.Log("Heh "+(int)Enum.Parse(typeof(TechType),"GhostRayBlueEgg"));


        DLLFinder.PatchDLLs();


        CustomResourceManager.Patch(instance);
        ProtobufSerializerPatcher.Patch(instance);
        CustomSpriteManager.Patch(instance);
        CraftDataPatchHelper.Patch();
        LootPatcher.Patch(instance);
        WorldEntityInfoPatcher.Patch(instance);
        DLLFinder.PostPatchDLLs();
        //Debug.Log("Testing " + (bool)Resources.Load<GameObject>("WorldEntities/Eggs/GhostRayBlueEgg"));
        //instance.PatchAll(Assembly.GetExecutingAssembly());


    }
    public static void DumpType(Type t, object obj,StringBuilder s, string indenter="")
    {
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
        foreach (var f in t.GetFields(flags))
        {
            try
            {
                var h = f.GetValue(obj);
                var newtype = f.FieldType;
                
                if (h == null)
                {
                    s.AppendLine(indenter + newtype.Name + " " + f.Name);
                    continue;
                }
                if (!newtype.IsPrimitive)
                {
                    if (newtype.Equals(typeof(String)))
                    {
                        s.AppendLine(indenter + newtype.Name + " " + f.Name+" "+h.ToString());
                        continue;
                    }
                    //if (newtype.GetInterface("IEnumerable").Equals(null))
                    //{
                    s.AppendLine(indenter + newtype.Name + " " + f.Name);
                    DumpType(h.GetType(), h, s, indenter + "    ");

                    continue;
                    //}
                    
                    
                }
                else
                {
                    s.AppendLine(indenter + newtype.Name + " " + f.Name + " " + h.ToString());
                    
                    
                }
            }
            catch {
                //s.AppendLine();
            }
        }
        foreach (var f in t.GetProperties(flags))
        {
            
            try
            {
                if (f.Name == "Item")
                    continue;
                var h = f.GetValue(obj, new object[0]);
                
                var newtype = f.PropertyType;

                if (h == null)
                {
                    s.AppendLine(indenter + newtype.Name + " " + f.Name + " " + h.ToString());
                    continue;
                }
                if (!newtype.IsPrimitive)
                {
                    if (newtype.Equals(typeof(String)))
                    {
                        s.AppendLine(indenter + newtype.Name + " " + f.Name);
                        continue;
                    }
                    if (newtype.GetInterface("IEnumerable").Equals(null))
                    {
                        s.AppendLine(indenter + newtype.Name + " " + f.Name);
                        DumpType(newtype, h, s, indenter + "    ");
                        continue;
                    }
                }
                else
                {
                    s.AppendLine(indenter + newtype.Name + " " + f.Name + " " + h.ToString());
                }
            }
            catch { }
        }
        try
        {
            if (!t.BaseType.Equals(null) && !t.BaseType.Equals(typeof(System.Object)) && !t.BaseType.Equals(typeof(ValueType)))
            {
                DumpType(t.BaseType, obj, s, indenter);

            }
            
        }
        catch { }
    }
    public static void DumpGameObject(GameObject obj,StringBuilder s,string indenter="    ")
    {
        foreach(var c in obj.GetComponents<Component>())
        {
            s.AppendLine(c.GetType().Name);
            DumpType(c.GetType(), c, s, indenter);
        }
    }
    }
    
