using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class CustomResource
{
    //WorldEntities / Eggs / BonesharkEgg
    public string Key;
    public string Path;
    protected UnityEngine.Object Resource;
    public virtual void Patch()
    {

    }
    public virtual UnityEngine.Object LoadResource()
    {
        return null;
    }
    public CustomResource(string Path, string Key)
    {
        this.Path = Path;
        this.Key = Key;
    }
}

