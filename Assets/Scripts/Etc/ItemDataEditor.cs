using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using Random = System.Random;

namespace Etc
{
    public class ItemDataEditor : EditorWindow
    {
        public static string filePath = "/Resources/ItemData.txt";
        private List<RecipeData> _recipes = new List<RecipeData>();
        private RecipeData selectedRecipe = null;
        
        private int recipeCount = 0;
        private List<int> index = new List<int>(6) { 0,0,0,0,0,0 };
        private Sprite _sprite;
        
        private int count
        {
            get { return recipeCount; }
            set
            {
                if(selectedRecipe != null)
                {
                    if(selectedRecipe.materials == null)
                        selectedRecipe.materials = new List<ItemData>();

                    if (selectedRecipe.materials.Count > value)
                    {
                        selectedRecipe.materials.RemoveRange(value,selectedRecipe.materials.Count - value);
                    }
                    else
                    {
                        for (int i = selectedRecipe.materials.Count; i < value; i++)
                        {
                            selectedRecipe.materials.Add(new ItemData("",0));
                        }
                    }
                }

                recipeCount = value;
            }
        }

        private static ItemDataEditor window;
        
        [MenuItem("Window/ItemDataEditor")]
        static void Open()
        {
            if (window == null)
                window = CreateWindow<ItemDataEditor>();
            
            window.Show();
        }

        public void OnEnable()
        {
            load();
        }

        public Vector2 scrollPosition;
        void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical("Item Data",GUILayout.Width(300));

            using(var scrollViewScope = new EditorGUILayout.ScrollViewScope(scrollPosition, GUILayout.Width(300),GUILayout.Height(position.height - 70)))
            {
                scrollPosition = scrollViewScope.scrollPosition;
                foreach (var recipe in _recipes)
                {
                    if(GUILayout.Button(recipe.result.itemName,GUILayout.Height(30)))
                    {
                        if (selectedRecipe != recipe)
                        {
                            selectedRecipe = recipe;

                            _sprite = null;
                            if(selectedRecipe.result.imageFilePath != null)
                                _sprite = AssetDatabase.LoadAssetAtPath<Sprite>(selectedRecipe.result.imageFilePath);

                            if (selectedRecipe.materials == null)
                            {
                                count = 0;
                            }
                            else
                            {
                                count = selectedRecipe.materials.Count;

                                for (int i = 0; i < count; i++)
                                {
                                    index[i] = _recipes.Select(_ => _.result.itemName).ToList().IndexOf(selectedRecipe.materials[i].itemName);
                                }
                            }
                        }
                    }
                }
            }
            
            if (GUILayout.Button("New Item", GUILayout.Height(30)))
            {
                _recipes.Add(new RecipeData(new ItemData("new Item " + UnityEngine.Random.Range(0,int.MaxValue),0),null));
                Repaint();
            }
            
            GUILayout.EndVertical();
            
            GUILayout.BeginVertical("Item Data",GUILayout.Width(position.width-300));

            if (selectedRecipe != null)
            {
                selectedRecipe.result.itemName =
                    EditorGUILayout.TextField("아이템 이름", selectedRecipe.result.itemName);
                selectedRecipe.result.itemCode =
                    EditorGUILayout.IntField("아이템 코드", selectedRecipe.result.itemCode, GUILayout.MinWidth(10.0f));
                _sprite =
                    (Sprite) EditorGUILayout.ObjectField("2D Sprite",_sprite, typeof(Sprite), true);
                
                if(_sprite != null)
                    selectedRecipe.result.imageFilePath = AssetDatabase.GetAssetPath(_sprite);

                count = EditorGUILayout.IntField("재료 갯수", count);

                for (int i = 0; i < (count <= _recipes.Count ? count : _recipes.Count); i++)
                {
                    index[i] = EditorGUILayout.Popup(index[i], _recipes.Select(_ => _.result.itemName).ToArray());

                    selectedRecipe.materials[i].itemCode = _recipes[index[i]].result.itemCode;
                    selectedRecipe.materials[i].itemName = _recipes[index[i]].result.itemName;
                    selectedRecipe.materials[i].imageFilePath = _recipes[index[i]].result.imageFilePath;
                }
            }
            
            GUILayout.EndVertical();
            
            GUILayout.EndHorizontal();
            
            if (GUILayout.Button("Save",GUILayout.Height(30)))
            {
                save();
            }
            
            if (Event.current.type == EventType.MouseMove)
                Repaint ();
        }

        private void load()
        {
            var reader = new StreamReader(Application.dataPath + filePath);
            var text = reader.ReadToEnd();
            reader.Close();

            _recipes = JsonConvert.DeserializeObject<List<RecipeData>>(text);
        }

        private void save()
        {
            var text = JsonConvert.SerializeObject(_recipes,Formatting.Indented);

            var sr = File.CreateText(Application.dataPath + filePath);
            sr.Write(text);
            sr.Close();
        }
    }
}