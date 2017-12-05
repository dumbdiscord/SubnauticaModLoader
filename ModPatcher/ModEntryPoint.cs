using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public abstract class ModEntryPoint
{
    public string Path;
    public virtual void Prepatch()
    {

    }
    public virtual void Patch()
    {

    }
    public virtual void PostPatch()
    {

    }
}

