using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using JetBrains.Annotations;
using UnityEngine;
using Random = System.Random;

public class MapAutoGenerator : MonoBehaviour
{
    public int height;    //맵 높이
    public int width;    //맵 넓이
    public int max_cluster_size;    //한 군집 크기
    
    public List<int> object_kinds_Min_Count = new List<int>();    //오브젝트 종류와 그 갯수
    public List<Color> sampleColors;
    public GameObject sampleObject;

    public List<int> except_Height;
    
    private int[] sampleMap;
    
    public void Start()
    {
        generate();

        LoadBlock();
        
        Logger.Log("1 : " + searchCount(1));
        Logger.Log("2 : " + searchCount(2));
        Logger.Log("3 : " + searchCount(3));
    }

    public void LoadBlock()
    {
        for (int i = 0 ; i < sampleMap.Length; i++)
        {
            var tile = sampleMap[i];
            if (tile == 0 || tile == -1)
                continue;

            var x = i % width;
            var y = i / width;
            
            var go = GameObject.Instantiate(sampleObject);
            go.SetActive(true);
            go.transform.position = new Vector3(x,y,0);
            go.GetComponent<MeshRenderer>().material.color = sampleColors[tile - 1];
        }
    }

    public void generate()
    {
        createMap();
        Logger.Log("Generate Map");

        var value = true;
        
        foreach (var kind in object_kinds_Min_Count)
        {
            var t = searchCount(object_kinds_Min_Count.IndexOf(kind) + 1) >= kind;
            value = value && t;
        }
        
        if(value == false)
            generate();
    }

    public void createMap()
    {
        sampleMap = new int[width * height];

        for (int i = 0; i < width * height; i++)
        {
            int value = i/width;

            if (except_Height.Contains(value))
            {
                sampleMap[i] = -1;
            }
            else
                sampleMap[i] = 0;
        }

        var start_posList = random_set_k(object_kinds_Min_Count.Count);
        
        foreach (var start_pos in start_posList)
        {
            createCluster(start_pos,start_posList.IndexOf(start_pos) + 1,0);
        }
    }

    public List<int> random_set_k(int count)
    {
        var start_posList = new List<int>();

        while(true)
        {
            var randomValue = UnityEngine.Random.Range(0, height * width);
            
            if(sampleMap[randomValue] == 0)
                start_posList.Add(randomValue);

            if (start_posList.Count >= count)
                break;
        }

        return start_posList;
    }

    public int searchCount(int kind)
    {
        var returnValue = 0;
        foreach (var tile in sampleMap)
        {
            if (tile == kind)
                returnValue++;
        }

        return returnValue++;
    }

    public void createCluster(int value,int kind,int count)
    {
        var x = value % width;
        var y = value / width;
        createCluster(x,y,kind,count);
    }

    public void createCluster(int x, int y, int kind, int count)
    {
        setObject(x,y,kind);

        var temp = adjacencyCheck(x, y);

        if (count > max_cluster_size || temp.Count == 0)
        {
            if (searchCount(kind) < object_kinds_Min_Count[kind - 1])
            {
                var value = random_set_k(1);
                createCluster(value[0],kind,temp.Count + 1);
            }
            return;
        }

        foreach (var t in temp)
        {
            createCluster(t.Item1,t.Item2,kind,count + 1 + temp.Count/2);
        }
    }

    public List<(int, int)> adjacencyCheck(int value)
    {
        var x = value % width;
        var y = value / width;
        return adjacencyCheck(x, y);
    }

    public List<(int,int)> adjacencyCheck(int x, int y)
    {
        var returnValue = new List<(int, int)>();
        var tempValue = new Dictionary<(int,int),int>();
        
        tempValue[(x,y+1)] = getObject(x,y + 1);    //up
        tempValue[(x+1,y)] = getObject(x + 1,y);    //right
        tempValue[(x,y-1)] = getObject(x,y - 1);    //down
        tempValue[(x-1,y)] = getObject(x - 1,y);    //left
        
        if (x == 0)
            tempValue[(x-1,y)] = -1;

        if (x == width)
            tempValue[(x+1,y)] = -1;

        if (y == 0)
            tempValue[(x,y-1)] = -1;

        if (y == height)
            tempValue[(x,y+1)] = -1;

        foreach (var value in tempValue)
        {
            if(value.Value == 0)
                returnValue.Add(value.Key);
        }

        return returnValue;
    }

    public int getObject(int value)
    {
        var x = value % width;
        var y = value / width;
        return getObject(x, y);
    }
    
    public int getObject(int x, int y)
    {
        try
        {
            var returnValue = sampleMap[x + y * width];

            return returnValue;
        }
        catch(IndexOutOfRangeException e)
        {
            return -1;
        }
    }

    public void setObject(int value, int kind)
    {
        var x = value % width;
        var y = value / width;
        
        setObject(x,y,kind);
    }

    public void setObject(int x, int y, int kind)
    {
        sampleMap[x + y * width] = kind;
    }
}
