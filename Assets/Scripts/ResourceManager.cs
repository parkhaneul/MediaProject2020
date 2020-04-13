using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ResourceManager
{
    private static ResourceManager _instance;
    public static ResourceManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new ResourceManager();
            return _instance;
        }
    }

    private static Dictionary<string, GameObject> _assetCache;
    
    public ResourceManager()
    {
        if (_instance == null)
            _instance = this;
        
        if(_assetCache == null)
            clear();
    }

    public void clear()
    {
        _assetCache = new Dictionary<string, GameObject>();
    }

    [CanBeNull]
    public GameObject getAsset(string fileName)
    {
        GameObject returnValue = null;
        _assetCache.TryGetValue(fileName, out returnValue);
        return returnValue;
    }

    public bool loadAll(string folderPath)
    {
        object[] values = Resources.LoadAll(folderPath);

        if (values.Length == 0)
        {
            Logger.LogWarning("ResourceManager Error",);
            return false;
        }

        foreach (var value in values)
        {
            var go = value as GameObject;
            _assetCache.Add(go.name,go);
        }

        return true;
    }
}
