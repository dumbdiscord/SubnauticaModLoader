using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public abstract class ModEntryPoint
{
    public string Path;
    /// <summary>
    ///  Use this to load any resources from asset bundles/initialize custom prefabs, and register custom techtypes
    /// </summary>
    public virtual void Prepatch()
    {

    }
    /// <summary>
    ///  Use ths to add custom haromny patches and other miscalaneous pathes such as custom serializers
    /// </summary>
    public virtual void Patch()
    {

    }
    public virtual void PostPatch()
    {

    }
}

