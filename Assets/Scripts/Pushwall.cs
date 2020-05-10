using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

using Vector3 = UnityEngine.Vector3;

public class Pushwall : MonoBehaviour, Placable
{
    public float PushThresholdTime = 1.0f;
    static private GridManager gridManager;
    private Dictionary<CharacterAction, float> timers;
    public void AdjustPosition(Grid grid)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        gridManager = GridManager.Instance;
        timers = new Dictionary<CharacterAction, float>();
    }

    /*
    *** 여러 사람이 밀 수 있다. 여러 사람이 밀면 어떡할꺼? 
    일단 한 사람만 밀 수 있는 걸로 생각하고 짜보자.

        1. Character와 Object가 밀접히 접촉한 체 n초가 지나야한다.
            이걸 collider로 판단해야하나? (해보는 중)
        2. n초 동안 캐릭터가 이동키를 누르고 있어야한다. (n초가 지나기전에 멈추면 취소)
            hasMovedThisFrame 으로 시도해보자
        3. 이동키를 누른 방향으로 object를 움직이려고 시도한다. 실패하면 1번으로 돌아간다.
        4. 이동하고자 하는 방향의 grid 점유 여부를 검사한다. 점유되어 있으면 실패
        5. 점유되어 있지 않다면 움직인다. 움직일 때 Collision 검사를 전부 중단하며, 체크하고 있던 타이머를 모두 파괴한다. 
    */

    private void OnCollisionEnter(Collision other) 
    {
        //Debug.Log("Collision Enter");    
        CharacterAction character = other.gameObject.GetComponent<CharacterAction>();
        if(character != null)
        {
            timers.Add(character, Time.time);
        }
    }

    private void OnCollisionStay(Collision other) 
    {
        //Debug.Log("Collision Stay");
        CharacterAction character = other.gameObject.GetComponent<CharacterAction>();
        if(character != null)
        {
            if(timers.ContainsKey(character))
            {
                if(!character.hasMovedThisFrame)
                {
                    Debug.Log("Reset");
                    timers[character] = Time.time;
                }
                if(Time.time >= timers[character] + PushThresholdTime)
                {
                    timers[character] = Time.time;
                    MoveToNextGrid(character.gameObject.transform.position);
                }
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        //Debug.Log("Collision Exit");   
        CharacterAction character = other.gameObject.GetComponent<CharacterAction>();
        if(character != null)
        {
            timers.Remove(character);
        }
    }

    private void MoveToNextGrid(Vector3 characterPos)
    {
        //Debug.Log("Move To Next Grid");
        Vector3 dir = MoveDirection(characterPos);
        Grid neighborGrid = gridManager.GetNeighborGridFromDirection(this, dir);
        if(neighborGrid != null)
        {

        }
    }

    private Vector3 MoveDirection(Vector3 pos)
    {
        Vector3 dir = gameObject.transform.position - pos;
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
}
