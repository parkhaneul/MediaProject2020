using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class SpriteManager
{
    private static SpriteManager _instance;
    public static SpriteManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new SpriteManager();
            return _instance;
        }
    }

    private static Dictionary<string, Sprite> _SpriteCache;
    
    public SpriteManager()
    {
        if (_instance == null)
            _instance = this;
        
        if(_SpriteCache == null)
            clear();
    }

    public void clear()
    {
        _SpriteCache = new Dictionary<string, Sprite>();
    }

    [CanBeNull]
    public Sprite getAsset(string filePath)
    {
        if(_SpriteCache.ContainsKey(filePath))
            return _SpriteCache[filePath];
        
        Sprite returnValue = AssetDatabase.LoadAssetAtPath<Sprite>(filePath);

        _SpriteCache[filePath] = returnValue;

        return _SpriteCache[filePath];
    }
}
