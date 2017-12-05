using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public static class BasicWorldEntity
{
    public static void AddBasicComponents(GameObject obj,string uid)
    {
        obj.AddComponent<PrefabIdentifier>().ClassId = uid;
        obj.AddComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Near;
        var rend = obj.GetComponentInChildren<Renderer>();
        rend.material.shader = Shader.Find("MarmosetUBER");
        var applier = obj.AddComponent<SkyApplier>();
        applier.renderers = new Renderer[] { rend };
        applier.anchorSky = Skies.Auto;
        var forces = obj.AddComponent<WorldForces>();
        forces.useRigidbody = obj.GetComponent<Rigidbody>();
    }
}

