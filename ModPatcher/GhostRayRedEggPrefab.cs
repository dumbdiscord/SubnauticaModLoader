using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using System.Reflection;
public class GhostRayRedEggPrefab : CustomResource
{
    public override void Patch()
    {
        PrefabDatabasePatcher.customPrefabs.Add(this);
        EntTechDataPatcher.customTechData.Add(System.IO.Path.GetFileName(Path), (TechType)6969);
        LanguagePatcher.customLanguageLines.Add("GhostRayRedEgg", "Crimson Ray Egg");
        LanguagePatcher.customLanguageLines.Add("Tooltip_GhostRayRedEgg", "The egg of a lava zone Crimson Ray");
        LanguagePatcher.customLanguageLines.Add("GhostRayRedEggUndiscovered", "Inactive Lava Zone egg");
        LanguagePatcher.customLanguageLines.Add("Tooltip_GhostRayRedEggUndiscovered", "Undiscovered Egg from the Inactive Lava Zone");
        CraftDataHelper.GetCraftDataDictionary<Dictionary<TechType, HarvestType>>("harvestTypeList").Add((TechType)6969, HarvestType.Pick);
        CraftDataHelper.GetCraftDataDictionary<Dictionary<TechType, string>>("pickupSoundList").Add((TechType)6969, "event:/loot/pickup_egg");
        CraftDataHelper.GetCraftDataDictionary<Dictionary<TechType, string>>("pickupSoundList").Add((TechType)6968, "event:/loot/pickup_egg");
        CraftDataHelper.GetCraftDataDictionary<Dictionary<TechType, Vector2int>>("itemSizes").Add((TechType)6968, new Vector2int(2, 2));
        CraftDataHelper.GetCraftDataDictionary<Dictionary<TechType, Vector2int>>("itemSizes").Add((TechType)6969, new Vector2int(2, 2));

        LootPatcher.customLootTables.Add(Key, new LootDistributionData.SrcData()
        {
            prefabPath = Path,
            distribution = new List<LootDistributionData.BiomeData>()
            {
                new LootDistributionData.BiomeData()
                {
                    biome=BiomeType.InactiveLavaZone_Chamber_Floor,
                    count=1,
                    probability=.1f
                }
            }
        });
        WorldEntityInfoPatcher.customWorldEntityInfo.Add(new UWE.WorldEntityInfo()
        {
            cellLevel = LargeWorldEntity.CellLevel.Near,
            classId = Key,
            techType = (TechType)6969,
            slotType = EntitySlot.Type.Small,
            localScale = Vector3.one,
            prefabZUp=true
        });
    }
    public override UnityEngine.Object LoadResource()
    {
        if (Resource == null)
        {
            //BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;

            GameObject original = Resources.Load<GameObject>("WorldEntities/Eggs/BonesharkEgg");
            GameObject newobject = Patcher.bund.LoadAsset("Capsule 1") as GameObject;
            SkinnedMeshRenderer rend = original.GetComponentInChildren<SkinnedMeshRenderer>();

            if (true)
            {

                newobject.name = "GhostRayRedEgg";
                var prefabid = newobject.AddComponent<PrefabIdentifier>();
                newobject.AddComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Near;
                prefabid.ClassId = Key;

                var rb = newobject.GetComponent<Rigidbody>();
                rb.isKinematic = true;
                var pickupable = newobject.AddComponent<Pickupable>();
                var egg = newobject.AddComponent<CreatureEgg>();
                egg.daysBeforeHatching = 1.5f;
                newobject.AddComponent<EntityTag>().slotType = EntitySlot.Type.Small;
                //newobject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
                var forces = newobject.AddComponent<WorldForces>();
                forces.handleGravity = true;
                forces.underwaterGravity = 2f;
                forces.useRigidbody = rb;
                forces.aboveWaterGravity = 9.81f;
                forces.underwaterDrag = 1;
                forces.aboveWaterDrag = .1f;
                forces.handleDrag = true;
                var skyapplier = newobject.AddComponent<SkyApplier>();
                var skin = newobject.GetComponent<MeshRenderer>();
                skyapplier.renderers = new Renderer[] { skin };
                skyapplier.anchorSky = Skies.Auto;
                var shared = skin.sharedMaterial.mainTexture;
                var texture = skin.material.mainTexture;
                //skin.material = rend.material;//= rend.material;
                skin.material.shader = rend.material.shader;
                skin.material.color = new Color(.75f, 0, 0);
                //skin.sharedMaterial = rend.sharedMaterial;// = rend.sharedMaterial;
                skin.sharedMaterial.shader = rend.sharedMaterial.shader;
                skin.material.SetFloat("_SpecInt", 16);
                skin.material.SetFloat("_Shininess", 7.5f);
                skin.material.SetTexture("_SpecText", texture);
                //skin.material.SetColor("_EmissionColor", new Color(1, 1, 1,1));
                //skin.sharedMaterial.SetColor("_EmissionColor", new Color(1, 1, 1,1));
                //skin.materials[0].SetTexture("_Illum", shared);
                //Debug.Log("Heck " + original.GetComponent<SkyApplier>().renderers[0].name);

                var living = newobject.AddComponent<LiveMixin>();
                living.health = 25;
                living.maxHealth = 25;
                living.knifeable = true;
                living.destroyOnDeath = true;
                living.explodeOnDestroy = true;
                var rt = newobject.AddComponent<ResourceTracker>();
                rt.overrideTechType = TechType.GenericEgg;
                rt.prefabIdentifier = prefabid;
                rt.rb = rb;
                rt.pickupable = pickupable;
                egg.overrideEggType = (TechType)6968;
                var anim = newobject.AddComponent<Animator>();
                egg.animator = anim;
                egg.hatchingCreature = TechType.GhostRayRed;
                //egg.explodeOnHatch = true;
                newobject.AddComponent<ScaleFixer>().scale = new Vector3(.5f, .5f, .5f);
            }
            else
            {
                newobject.name = "GhostRayRedEgg";

                var prefabid = newobject.GetComponent<PrefabIdentifier>();
                Component.Destroy(prefabid);
                prefabid = newobject.AddComponent<PrefabIdentifier>();
                Debug.Log("YEs " + prefabid.ClassId);
                prefabid.ClassId = Key;
                
                var egg = newobject.GetComponent<CreatureEgg>();
                egg.daysBeforeHatching = 1.5f;



                egg.overrideEggType = (TechType)6968;
                egg.hatchingCreature = TechType.GhostRayRed;
                egg.explodeOnHatch = true;
            }


            this.Resource = newobject;
        }
        return Resource;
    }
    public GhostRayRedEggPrefab(string path, string key) : base(path, key) { }
}
public class GhostRayRedEggSprite : CustomResource
{
    public override UnityEngine.Object LoadResource()
    {
        if (Resource == null)
        {
            //Resource = SpriteManager.defaultSprite.;
            //var sprite = Patcher.bund.LoadAsset("suspensionrailway",typeof(Sprite));
            //Debug.Log(sprite.name+ " "+sprite.GetType().Name);
            //sprite.name = "GhostRayBlueEggUndiscovered";
            //Resource = sprite;
        }
        return Resource;
    }
    public GhostRayRedEggSprite(string path, string key) : base(path, key) { }
}

