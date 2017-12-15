using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public static class AssetBundleLoader
{
    public static AssetBundle LoadedBundle
    {
        get;
        private set;
    }
    public static void LoadAssetBundle(string path)
    {
        if (LoadedBundle)
        {
            LoadedBundle.Unload(false);
        }
        LoadedBundle = AssetBundle.LoadFromFile(path);
    }
    public static void Unload()
    {
        if (LoadedBundle)
        {
            LoadedBundle.Unload(false);
        }
    }
}

