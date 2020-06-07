using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Etc;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class CraftSystem : MonoBehaviour
{
    public List<ItemData> materials;

    public Image[] slots;
    public Image result;
    public Text recipeCount;
    
    private static CraftSystem _instance;
    public static CraftSystem Instance
    {
        get
        {
            if (_instance == null)
                _instance = new CraftSystem();
            return _instance;
        }
    }

    public void OnEnable()
    {
        if (_instance == null)
            _instance = this;
    }

    public void Start()
    {
        loadData();
        
        materials = new List<ItemData>();
    }

    public void addItems(params ItemData[] _materials)
    {
        foreach (var material in _materials)
        {
            var count = materials.Count;
            materials.Add(material);
            slots[count].color = new Color(1,1,1,1);
            slots[count].sprite = SpriteManager.Instance.getAsset(material.imageFilePath);
        }
        
        showResult();
    }

    public void showResult()
    {
        var recipes = RecipeBook.Instance.searchMaterials(materials.Select(_ => _.itemCode).ToArray());
        
        Logger.Log(recipes.Count);
        
        if (recipes == null)
        {
            result.sprite = null;
            return;
        }else if(recipes.Count == 0)
        {
            result.sprite = null;
        }
        else
        {
            recipes.Sort(delegate(Recipe a, Recipe b)
            {
                if (a.materialCodes.Count < b.materialCodes.Count)
                    return -1;
                else if (a.materialCodes.Count > b.materialCodes.Count)
                    return 1;
                else
                    return 0;
            });
            
            var _spritePath = ItemPalette.Instance.searchItem(recipes[0].resultCode).imageFilePath;
            result.sprite = SpriteManager.Instance.getAsset(_spritePath);

            showRequiredItems(recipes[0]);
            
            if (recipes.Count <= 1)
                recipeCount.text = "";
            else
                recipeCount.text = "+" + (recipes.Count-1);
        }
    }

    public void showRequiredItems(Recipe recipe)
    {
        var codeList = materials.Select(_ => _.itemCode).ToList();
        var mList = recipe.materialCodes;
        
        foreach (var code in codeList)
        {
            var value = mList.Contains(code);

            if (value)
            {
                mList.Remove(code);
            }
        }

        var count = materials.Count;
        
        foreach (var m in mList)
        {
            var spritePath = ItemPalette.Instance.searchItem(m).imageFilePath;
            slots[count].color = new Color(1,1,1,0.5f);
            slots[count].sprite = SpriteManager.Instance.getAsset(spritePath);
            count++;
        }
    }
    
    private void loadData()
    {
        var reader = new StreamReader(Application.dataPath + ItemDataEditor.filePath);
        var text = reader.ReadToEnd();
        reader.Close();

        var recipeDatas = JsonConvert.DeserializeObject<List<RecipeData>>(text);
        
        foreach(var recipeData in recipeDatas){
            RecipeBook.Instance.addRecipe(recipeData.toRecipe());
        }
    }
}

public class RecipeBook
{
    private static RecipeBook _instnace;
    public static RecipeBook Instance
    {
        get
        {
            if (_instnace == null)
                _instnace = new RecipeBook();
            return _instnace;
        }
    }
    
    private List<Recipe> _recipes;

    public RecipeBook()
    {
        if(_recipes == null)
            _recipes = new List<Recipe>();
    }

    public void addRecipe(params Recipe[] recipes)
    {
        foreach(var recipe in recipes)
            _recipes.Add(recipe);
    }
    
    public List<Recipe> searchMaterials(params int[] itemCodes)
    {
        return _recipes.Where(_ => _.isContain(itemCodes)).ToList();
    }

    public int getRecipeCount()
    {
        return _recipes.Count;
    }
}

public class Recipe
{
    public List<int> materialCodes;
    public int resultCode;

    public Recipe(int _resultCode,params int[] _materialCodes)
    {
        this.resultCode = _resultCode;

        if(materialCodes == null)
            materialCodes = new List<int>();

        if (_materialCodes == null)
            return;
        
        foreach (var item in _materialCodes)
        {
            materialCodes.Add(item);
        }
    }
    
    public bool isContain(params int[] codeList)
    {
        var returnValue = true;
        foreach (var code in codeList)
        {
            returnValue &= materialCodes.Contains(code);

            if (returnValue == false)
                return returnValue;
        }

        return returnValue;
    }
}

public class RecipeData
{
    public List<ItemData> materials;
    public ItemData result;

    public RecipeData(ItemData _result,params ItemData[] _materials)
    {
        this.result = _result;

        if(materials == null)
            materials = new List<ItemData>();

        if (_materials == null)
            return;
        
        foreach (var item in _materials)
        {
            materials.Add(item);
        }
    }
    
    public bool isContain(params ItemData[] list)
    {
        var returnValue = true;
        foreach (var item in list)
        {
            returnValue &= materials.Contains(item);

            if (returnValue == false)
                return returnValue;
        }

        return returnValue;
    }

    public Recipe toRecipe()
    {
        var resultCode = ItemPalette.Instance.addItemData(result);
        List<int> materialsCode = new List<int>();

        foreach (var material in materials)
        {
            materialsCode.Add(ItemPalette.Instance.addItemData(material));
        }
        
        return new Recipe(resultCode,materialsCode.ToArray());
    }
}

public class ItemPalette
{
    private static ItemPalette _instance;
    public static ItemPalette Instance
    {
        get
        {
            if(_instance == null)
                _instance = new ItemPalette();
            return _instance;
        }
    }

    private Dictionary<int, ItemData> palette;
    
    public ItemPalette()
    {
        if(palette == null)
            palette = new Dictionary<int, ItemData>();
    }

    public int addItemData(ItemData item)
    {
        if (palette.ContainsKey(item.itemCode))
            return item.itemCode;
        
        palette[item.itemCode] = item;
        return item.itemCode;
    }

    [CanBeNull]
    public ItemData searchItem(int code)
    {
        if (palette.ContainsKey(code))
        {
            return palette[code];
        }

        return null;
    }
}

public class ItemData
{
    public string itemName;
    public int itemCode;
    public string imageFilePath;

    public ItemData(string name,int code)
    {
        itemName = name;
        itemCode = code;
    }
    
    public override bool Equals(object obj)
    {
        if (obj.GetType() != typeof(ItemData))
            return false;

        return itemCode == ((ItemData) obj).itemCode;
    }
}