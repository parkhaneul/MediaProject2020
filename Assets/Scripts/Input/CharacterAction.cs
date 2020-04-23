using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Carry,
    Running,
    UnCarry,
    Smash,
}

public class PlayerStateString
{
    public const string isMove = "IsMove";
    public const string isCarry = "IsCarry";
    public const string isSmash = "IsSmash";
}

public class CharacterAction : MonoBehaviour
{
    public const string CONST_InteractionHitBox  = "InteractionHitBox";
    public const string CONST_CharacterBound = "CharacterBound";
    public HashSet<Interactable> interactables;
    public Animator animator;
    public float moveSpeed;

    public Inventory Inventory;
    
    private PlayerState _state;
    private PlayerStateMachineObservables _playerStateMachineObservables;
    
    public Tool equipment { get; private set; }
    private Transform toolSocket;
    private const string toolSocketName = "ToolSocket";

    private void initTool()
    {
        Transform[] ts = gameObject.transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts)
        {
            if(t.gameObject.name == toolSocketName) 
            {
                toolSocket = t;
                return;
            }
        }
        if(toolSocket == null)
             Debug.LogError("All Characters should have tool Socket");
    }
    private Vector3 movePointer = Vector3.zero;
    
    public void Start()
    {
        if (Inventory == null)
            Inventory = this.GetComponent<Inventory>();
        
        _playerStateMachineObservables = animator.GetBehaviour<PlayerStateMachineObservables>();

        //smash animation
        Observable.EveryUpdate()
            .SkipUntil(_playerStateMachineObservables.OnStateEnterObservable)
            .Where(_ => animator.GetBool(PlayerStateString.isSmash))
            .TakeUntil(_playerStateMachineObservables.OnStateExitObservable)
            .Repeat()
            .DistinctUntilChanged()
            .Throttle(TimeSpan.FromMilliseconds(400))
            .Subscribe(_ =>
            {
                animator.SetBool(PlayerStateString.isSmash,false);
                if(interactables.Count > 0)
                {
                    foreach(var interactable in interactables)
                    {
                        interactable.OnInteract(this);
                    }
                }
            })
            .AddTo(this);

        //running animation exit
        Observable.EveryUpdate()
            .SkipUntil(_playerStateMachineObservables.OnStateEnterObservable)
            .TakeWhile(_ => animator.GetBool(PlayerStateString.isMove))
            .Repeat()
            .DistinctUntilChanged()
            .Select(_ => movePointer)
            .Where(x => x == Vector3.zero)
            .Subscribe(_ =>
            {
                //Logger.Log("stop");
                animator.SetBool(PlayerStateString.isMove,false);
            })
            .AddTo(this);

        //running animation enter
        Observable.EveryUpdate()
            .SkipUntil(_playerStateMachineObservables.OnStateEnterObservable)
            //.Where(_ => animator.GetBool(PlayerStateString.isMove) == false)
            //.TakeUntil(_playerStateMachineObservables.OnStateExitObservable)
            .TakeWhile(_ => animator.GetBool(PlayerStateString.isMove) == false)
            .Repeat()
            .DistinctUntilChanged()
            .Select(_ => movePointer)
            .Where(x => x != Vector3.zero)
            .Subscribe(_ =>
            {
                //Logger.Log("run");
                animator.SetBool(PlayerStateString.isMove,true);
            })
            .AddTo(this);

        initTool();
    }

    public CharacterAction()
    {
        interactables = new HashSet<Interactable>();
    }

    public void move(Point point)
    {
        movePointer = new Vector3(point.x,point.y,point.z);
        
        if(movePointer != Vector3.zero){
            var lookAtVector = this.transform.position + movePointer;
            this.gameObject.transform.LookAt(lookAtVector);
            this.gameObject.transform.position += this.gameObject.transform.forward * moveSpeed;
        }
    }

    public void run()
    {
        _state = PlayerState.Running;
        animator.SetBool(PlayerStateString.isMove,true);
    }

    public void action(bool value)
    {
        if(animator.GetBool(PlayerStateString.isSmash) == false)
            animator.SetBool(PlayerStateString.isSmash,true);
    }

    public void SetEquipment(Tool tool)
    {
        _state = PlayerState.Carry;

        equipment = tool;
        
        tool.transform.SetParent(toolSocket);
        tool.transform.localPosition = new Vector3(0,0,0);
        tool.transform.localRotation = Quaternion.identity;
        tool.transform.localScale = Vector3.one;
    }

    public void UnsetEquipment()
    {

    }
}
