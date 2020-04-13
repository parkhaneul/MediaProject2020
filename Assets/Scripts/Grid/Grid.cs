using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour //TODO : Make This SingleTon
{
    //
    //This component should be attached to 'ground' like mesh (for example, plane)
    //
    //

    public int tileCount = 100;
    public bool debugMode = true;
    private List<Vector3> gridCenters;
    private const int planeOffset = 10; //Plane consists of 10 quads.

    void Start()
    {
        gridCenters = new List<Vector3>();
        float tileWidth = planeOffset * gameObject.transform.localScale.x / tileCount;
        float planeWidth = planeOffset * gameObject.transform.localScale.x;
       
        for (int i = 0; i < tileCount; i++)
        {
            for (int j = 0; j < tileCount; j++)
            {
                gridCenters.Add(gameObject.transform.position
                    - Vector3.forward * j * tileWidth + Vector3.forward * planeWidth / 2
                    - Vector3.right * i * tileWidth + Vector3.right * planeWidth / 2
                    - Vector3.right * tileWidth / 2 - Vector3.forward * tileWidth / 2);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if(debugMode)
        {
            foreach(var center in gridCenters)
            {
                Debug.DrawLine(center, center + Vector3.up, Color.red);
            }
            
        }
        
    }

    public void GenerateGrids()
    {

    }
    public Vector3 GetClosestGridCenter()
    {
        return Vector3.zero;
    }
}
