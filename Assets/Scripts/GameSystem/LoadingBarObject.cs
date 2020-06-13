using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.TestTools;

public class LoadingBarObject : MonoBehaviour, Placable
{
    static private GridManager gridManager;
    static private ToolSpawnerManager toolSpawnerManager;
    private MissionLogic mLogic;
    private float percent;

    /// <summary>
    /// basicPivot is (0,0,0) ,(0,0,0) is (-1,-1,-1), (1,1,1) is (1,1,1)
    /// </summary>
    public Vector3 pivot = Vector3.zero;
    private Vector3 initPosition;
    //(-10,0,0)
    public float interval = 0.1f;

    public void Start()
    {
        gridManager = GridManager.Instance;
        toolSpawnerManager = ToolSpawnerManager.Instance;

        initPosition = this.transform.position - pivot/2;
        
        if (mLogic == null)
            mLogic = MissionLogic.Instance;

        this.ObserveEveryValueChanged(_ => _.mLogic.getPercent())
            .Subscribe(percent =>
            {
                this.gameObject.transform.localScale = new Vector3(1, 1, 2) + Vector3.right * percent * interval + new Vector3(0.0f, 0.0f, 0.2f);
                this.gameObject.transform.position = initPosition + mulitple(Vector3.right/2,gameObject.transform.localScale - Vector3.one);
            })
            .AddTo(this);
        
        mLogic.OnAnimationDone += OccupyGrid;
    }

    public Vector3 mulitple(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }

    public void AdjustTransform(Grid grid)
    {
        return;
    }

    private void OccupyGrid()
    {
        Collider collider = GetComponent<Collider>();
        if(collider != null)
        {
            Grid[] grids = gridManager.GetAllGridsInBound(collider.bounds);
            gridManager.OccupyPlacable(this, grids);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Placable>() != null)
        {
            Building building = other.GetComponent<Building>();
            if(building != null)
            {
                building.OnDestroy();
                return;
            }

            Tool tool = other.GetComponent<Tool>();
            if(tool != null)
            {
                toolSpawnerManager.SpawnTool(tool.gameObject, tool.kind);
                return;
            }

            Pushwall pushwall = other.GetComponent<Pushwall>();
            if(pushwall != null)
            {
                pushwall.OnDestroy();
                return;
            }
        }
    }

}