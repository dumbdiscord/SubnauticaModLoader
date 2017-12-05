using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
public class TechDataWrapper : ITechData
{
    public int _craftAmount;

    public IngredientsWrapper _ingredients;
    public List<TechType> _linkedItems;
    public TechType _techType;
    public static readonly Type TechDataType = typeof(CraftData).GetNestedType("TechData", BindingFlags.NonPublic);
    private static readonly IIngredient nullIngredient=(IIngredient)new IngredientWrapper(TechType.None,0);
    public int craftAmount => _craftAmount;
    public int ingredientCount
    {
        get
        {
            if (_ingredients != null)
            {
                return this._ingredients.Count;
               
            }
            return 0;
        }
    }
    public TechDataWrapper()
    {
        _craftAmount = 1;
    }
    public static TechDataWrapper GetFromTechData(object techdata)
    {
        UnityEngine.Debug.Log("TechData is " + (TechDataType.FullName));

        var newobject = new TechDataWrapper();
        if (techdata == null)
        {
            return newobject;
        }
        
        newobject._ingredients = IngredientsWrapper.GetFromIngredients(TechDataType.GetField("_ingredients").GetValue(techdata));
        UnityEngine.Debug.Log("TechData is " + (techdata == null));
        newobject._craftAmount= (int)(TechDataType.GetField("_craftAmount").GetValue(techdata));
        UnityEngine.Debug.Log("TechData is " + (techdata == null));
        newobject._techType = (TechType)(TechDataType.GetField("_techType").GetValue(techdata));
        newobject._linkedItems=(List<TechType>)(TechDataType.GetField("_linkedItems").GetValue(techdata));
        return newobject;
    }
    public object ConvertToTechData()
    {
        var newobject = Activator.CreateInstance(TechDataType);
        (TechDataType.GetField("_craftAmount")).SetValue(newobject, _craftAmount);
        (TechDataType.GetField("_ingredients")).SetValue(newobject, _ingredients.ConvertToIngredients());
        (TechDataType.GetField("_techType")).SetValue(newobject, _techType);
        (TechDataType.GetField("_linkedItems")).SetValue(newobject, _linkedItems);
        return newobject;
    }
    public int linkedItemCount
    {
        get
        {
            if (this._linkedItems != null)
                return this._linkedItems.Count;
            return 0;
        }
    }

    public IIngredient GetIngredient(int index) { 
      if (this._ingredients == null || index >= this._ingredients.Count || index< 0)
        return nullIngredient;
      return (IIngredient) this._ingredients[index];
    }

    public TechType GetLinkedItem(int index)
    {
        if (this._linkedItems == null || index >= this._linkedItems.Count || index < 0)
            return TechType.None;
        return this._linkedItems[index];
    }
}
public class IngredientWrapper : IIngredient
{
    private TechType _techType;
    private int _amount;
    public TechType techType => _techType;
    public static readonly Type IngredientType = typeof(CraftData).GetNestedType("Ingredient", BindingFlags.NonPublic);
    public int amount => _amount;
    public IngredientWrapper(TechType techType, int amount =1)
    {
        _techType = techType;
        _amount = amount;
    }
    public object ConvertToIngredient()
    {
        return Activator.CreateInstance(IngredientType, _techType, _amount);
    }
    public static IngredientWrapper GetFromIngredient(object obj)
    {
        if (obj == null)
        {
            return null;
        }
        if (!obj.GetType().Equals(IngredientType))
        {
            throw new Exception();
        }

        return new IngredientWrapper((TechType)IngredientType.GetField("_techType", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(obj),(int)IngredientType.GetField("_amount", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(obj));
    }
}
public class IngredientsWrapper : List<IngredientWrapper>
{
    public static readonly Type IngredientsType = typeof(CraftData).GetNestedType("Ingredients", BindingFlags.NonPublic);
    public IngredientsWrapper():base()
    {
        
    }
    public void Add(TechType techType, int amount = 1)
    {
        this.Add(new IngredientWrapper(techType, amount));
    }
    public static IngredientsWrapper GetFromIngredients(object obj)
    {
        if (obj == null)
        {
            UnityEngine.Debug.Log("Found null Ingredients");
            return null;
        }
        UnityEngine.Debug.Log("Creating Ingredients...");
        var count  = (int)IngredientsType.GetProperty("Count").GetValue(obj,null);
        var indexprop = IngredientsType.GetProperty("Item");

        var newobj = (new IngredientsWrapper());
        for(int i = 0; i < count; i++)
        {
            var cur = IngredientWrapper.GetFromIngredient(indexprop.GetValue(obj, new object[] { i }));
            newobj.Add(cur);
        }
        return newobj;
    }
    public object ConvertToIngredients()
    {
        var newobj = Activator.CreateInstance(IngredientsType);
        var add = IngredientsType.GetMethod("Add", new Type[] { IngredientWrapper.IngredientType });
        foreach(var i in this)
        {
            add.Invoke(newobj, new object[] { i.ConvertToIngredient() });
        }
        return newobj;
    }
}