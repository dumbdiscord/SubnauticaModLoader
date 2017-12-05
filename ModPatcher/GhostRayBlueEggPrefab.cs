using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using System.Reflection;
public class GhostRayBlueEggPrefab:CustomResource
{
    public override void Patch()
    {
        PrefabDatabasePatcher.customPrefabs.Add(this);
        EntTechDataPatcher.customTechData.Add(System.IO.Path.GetFileName(Path), (TechType)9999);
        LanguagePatcher.customLanguageLines.Add("GhostRayBlueEgg", "Ghost Ray Egg");
        LanguagePatcher.customLanguageLines.Add("Tooltip_GhostRayBlueEgg", "The egg of a lost river Ghost Ray");
        LanguagePatcher.customLanguageLines.Add("GhostRayBlueEggUndiscovered", "Lost River Egg");
        LanguagePatcher.customLanguageLines.Add("Tooltip_GhostRayBlueEggUndiscovered", "Undiscovered Egg from the Lost River");
        CraftDataHelper.GetCraftDataDictionary<Dictionary<TechType,HarvestType>>("harvestTypeList").Add((TechType)9999,HarvestType.Pick);
        CraftDataHelper.GetCraftDataDictionary<Dictionary<TechType, string>>("pickupSoundList").Add((TechType)9999, "event:/loot/pickup_egg");
        CraftDataHelper.GetCraftDataDictionary<Dictionary<TechType, string>>("pickupSoundList").Add((TechType)9998, "event:/loot/pickup_egg");
        CraftDataHelper.GetCraftDataDictionary<Dictionary<TechType, Vector2int>>("itemSizes").Add((TechType)9998, new Vector2int(2,2));
        CraftDataHelper.GetCraftDataDictionary<Dictionary<TechType, Vector2int>>("itemSizes").Add((TechType)9999, new Vector2int(2, 2));
        
        LootPatcher.customLootTables.Add(Key, new LootDistributionData.SrcData()
        {
            prefabPath = Path,
            distribution = new List<LootDistributionData.BiomeData>()
            {
                new LootDistributionData.BiomeData()
                {
                    biome=BiomeType.TreeCove_LakeFloor,
                    count=1,
                    probability=.2f
                }
            }
        });
        WorldEntityInfoPatcher.customWorldEntityInfo.Add(new UWE.WorldEntityInfo() {
            cellLevel = LargeWorldEntity.CellLevel.Near,
            classId = Key,
            techType = (TechType)9999,
            slotType = EntitySlot.Type.Small,
            localScale = Vector3.one,
            prefabZUp = true
        });
    }
    public override UnityEngine.Object LoadResource()
    {
        if (Resource == null)
        {
            //BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;

            GameObject original =  Resources.Load<GameObject>("WorldEntities/Eggs/JellyrayEgg");
            GameObject newobject = Patcher.bund.LoadAsset("Capsule") as GameObject;
            SkinnedMeshRenderer rend = original.GetComponentInChildren<SkinnedMeshRenderer>();

            if (true)
            {

                newobject.name = "GhostRayBlueEgg";
                var prefabid = newobject.AddComponent<PrefabIdentifier>();
                newobject.AddComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Near;
                prefabid.ClassId = Key;

                var rb = newobject.GetComponent<Rigidbody>();
                rb.isKinematic = true;
                var pickupable = newobject.AddComponent<Pickupable>();
                var egg = newobject.AddComponent<CreatureEgg>();
                egg.daysBeforeHatching = 1.5f;
                newobject.AddComponent<EntityTag>().slotType = EntitySlot.Type.Small;
                var forces = newobject.AddComponent<WorldForces>();
                forces.handleGravity = true;
                forces.underwaterGravity = 2f;
                forces.useRigidbody = rb;
                forces.aboveWaterGravity = 9.81f;
                forces.underwaterDrag = 1;
                forces.aboveWaterDrag = .1f;
                forces.handleDrag = true;
                var skyapplier = newobject.AddComponent<SkyApplier>();
                //newobject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
                var skin = newobject.GetComponent<MeshRenderer>();
                skyapplier.renderers = new Renderer[] { skin };
                skyapplier.anchorSky = Skies.Auto;
                var shared = skin.sharedMaterial.mainTexture;
                var texture = skin.material.mainTexture;
                //skin.material = rend.material;//= rend.material;
                skin.material.shader = rend.material.shader;
                skin.material.color = new Color(0, 125f/255f, 125f/255f);
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
                egg.overrideEggType = (TechType)9998;
                var anim = newobject.AddComponent<Animator>();
                egg.animator = anim;
                egg.hatchingCreature = TechType.GhostRayBlue;
                //egg.explodeOnHatch = true;
                newobject.AddComponent<ScaleFixer>().scale= new Vector3(.5f, .5f, .5f);
            }
            else
            {
                newobject.name = "GhostRayBlueEgg";

                var prefabid = newobject.GetComponent<PrefabIdentifier>();
                Component.Destroy(prefabid);
                prefabid = newobject.AddComponent<PrefabIdentifier>();
                Debug.Log("YEs " + prefabid.ClassId);
                prefabid.ClassId = Key;
                Debug.Log("YEs " + prefabid.Id);
                var egg = newobject.GetComponent<CreatureEgg>();
                egg.daysBeforeHatching = 1.5f;



                egg.overrideEggType = (TechType)9998;
                egg.hatchingCreature = TechType.GhostRayBlue;
                egg.explodeOnHatch = true;
            }

            
            this.Resource = newobject;
        }
        return Resource;
    }
    public GhostRayBlueEggPrefab(string path, string key) : base(path, key) { }
}
public class GhostRayBlueEggSprite : CustomResource
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
    public GhostRayBlueEggSprite(string path, string key) : base(path, key) { }
}

