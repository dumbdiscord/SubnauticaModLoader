using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using System.Reflection;
using UnityEngine;

public static class CustomSpriteManager
{
    public static readonly List<CustomResource> customSprites = new List<CustomResource>();
    public static void Patch(HarmonyInstance instance)
    {
        foreach(var c in customSprites)
        {
            c.Patch();
        }
        PostFix();
        //instance.Patch(TargetMethod(), null, new HarmonyMethod(typeof(CustomSpriteManager).GetMethod("PostFix")));
    }
    public static MethodBase TargetMethod()
    {
        return typeof(SpriteManager).TypeInitializer;//GetMethod(".cctor", BindingFlags.Static | BindingFlags.NonPublic);
    }
    public static void PostFix()
    {

        var mapping = typeof(SpriteManager).GetField("mapping",  BindingFlags.NonPublic|BindingFlags.Static).GetValue(null) as Dictionary<SpriteManager.Group, string>;
        var groups = typeof(SpriteManager).GetField("groups", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null) as Dictionary<SpriteManager.Group, Dictionary<string, Atlas.Sprite>>;
        Array values = Enum.GetValues(typeof(SpriteManager.Group));
        for(int i = 0; i < values.Length; i++)
        {
            SpriteManager.Group key = (SpriteManager.Group)values.GetValue(i);
            bool slice9Grid = key == SpriteManager.Group.Background;
            var dictionary1 = groups[key];
            string str;
            if(mapping.TryGetValue(key,out str))
            {

                List<Sprite> spriteArray = new List<Sprite>();
                
                foreach(var c in customSprites)
                {
                    if(c.Path.Contains("Sprites/" + str))
                    {
                        spriteArray.Add(c.LoadResource() as Sprite);
                    }
                }
                foreach (var sprite in spriteArray)
                {
                    dictionary1[sprite.name] = new Atlas.Sprite(sprite,slice9Grid);
                    
                }
            }
            //Debug.Log("Could not find for " + Enum.GetName(typeof(SpriteManager.Group), key));
        }
    }
}

