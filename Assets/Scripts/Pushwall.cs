using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

using Vector3 = UnityEngine.Vector3;

public class Pushwall : MonoBehaviour, Placable
{
    public Vector3 MeshOffset = new Vector3(-0.5f, 0f, -0.5f);
    public float PushThresholdTime = 1.0f;
    public float TimeToArriveAtDestination = 1.0f;

    static private GridManager gridManager;

    private Dictionary<CharacterAction, float> timers;
    private bool isMoving;
    private MovingAnimationBundle animationBundle;

    public void AdjustTransform(Grid grid)
    {
        throw new System.NotImplementedException();
    }

    public void OnDestroy() 
    {
        gridManager.UnoccupyPlacable(this);
        Destroy(gameObject);
    }

    /*
     * TODO :: 1. 여러명이 동시에 밀 경우 고려안함, 고려해야함
     *         2. Grid를 2개 이상 점유하고 있는 Cube 고려안함. 
     */

    void Start()
    {
        gridManager = GridManager.Instance;
        timers = new Dictionary<CharacterAction, float>();
    }

    void Update()
    {
        if(isMoving)
        {
            transform.position = Vector3.Lerp(animationBundle.origin, animationBundle.destination, 
                (Time.time - animationBundle.startTime / TimeToArriveAtDestination));
            if( (transform.position - animationBundle.destination).sqrMagnitude <= float.Epsilon )
            {
                isMoving = false;
                animationBundle = null;
                SoundManager.Instance.StopDraggingSound(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision other) 
    {  
        CharacterAction character = other.gameObject.GetComponent<CharacterAction>();
        if(character != null)
        {
            timers.Add(character, Time.time);
        }
    }

    private void OnCollisionStay(Collision other) 
    {
        CharacterAction character = other.gameObject.GetComponent<CharacterAction>();
        if(character != null)
        {
            if(timers.ContainsKey(character))
            {
                if(!character.hasMovedThisFrame)
                {
                    timers[character] = Time.time;
                }
                if(Time.time >= timers[character] + PushThresholdTime && !isMoving)
                {
                    timers[character] = Time.time;
                    GridBundle bundle = gridManager.GetGridBundles(this);

                    if (bundle.grids.Count > 1)
                        MoveToNextGrids(character.gameObject.transform.position);
                    else
                        MoveToNextGrid(character.gameObject.transform.position);
                }
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        CharacterAction character = other.gameObject.GetComponent<CharacterAction>();
        if(character != null)
        {
            timers.Remove(character);
        }
    }
    private void PlaySound()
    {
        SoundManager.Instance.PlayDraggingSound(this.gameObject);
    }
    private void MoveToNextGrid(Vector3 characterPos)
    {
        Vector3 dir = MoveDirection(characterPos);
        Grid neighborGrid = gridManager.GetNeighborGridFromDirection(this, dir);
        
        if(neighborGrid != null)
        {
            gridManager.Move(this, neighborGrid);
            isMoving = true;
            animationBundle = new MovingAnimationBundle(Time.time, gameObject.transform.position, neighborGrid.gridCenter + MeshOffset);
            PlaySound();
        }
    }

    private void MoveToNextGrids(Vector3 characterPos)
    {
        Vector3 dir = MoveDirection(characterPos);
        Grid[] neighborGrids = gridManager.GetNeighborGridsFromDirection(this, dir);

        if (neighborGrids != null)
        {
            gridManager.Move(this, neighborGrids);
            Vector3 gridCenter = neighborGrids.Select(i => i.gridCenter)
                .Aggregate((sum, var) => sum += var) / neighborGrids.Length;

            isMoving = true;
            animationBundle = new MovingAnimationBundle(Time.time, gameObject.transform.position, gridCenter + MeshOffset);
            PlaySound();
        }
    }

    private Vector3 MoveDirection(Vector3 pos)
    {
        Vector3 dir = (gameObject.transform.position - MeshOffset - pos).normalized;
        float f_cos = Vector3.Dot(dir, Vector3.forward);
        float b_cos = Vector3.Dot(dir, Vector3.back);
        float r_cos = Vector3.Dot(dir, Vector3.right);
        float l_cos = Vector3.Dot(dir, Vector3.left);

        float max = Mathf.Max(f_cos, b_cos, r_cos, l_cos);

        if (max - f_cos <= float.Epsilon)
            return Vector3.forward;
        else if (max - b_cos <= float.Epsilon)
            return Vector3.back;
        else if (max - r_cos <= float.Epsilon)
            return Vector3.right;
        else
            return Vector3.left;
    }

    private class MovingAnimationBundle
    {
        public float startTime;
        public Vector3 origin;
        public Vector3 destination;

        public MovingAnimationBundle(float startTime, Vector3 origin, Vector3 destination)
        {
            this.startTime = startTime;
            this.origin = origin;
            this.destination = destination;
        }
    }
}
