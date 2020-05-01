using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public enum AnimationStateEnum
{
    Idle,
    Carry,
    Running,
    UnCarry,
    Smash,
    Tool,
}

public class AnimationStateString
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

    private PlayerStateMachineObservables _playerStateMachineObservables;
    public PlayerState pState;
    
    private AnimationStateEnum _aState;

    private Transform toolSocket;
    private const string toolSocketName = "ToolSocket";

    private Transform itemSocket;
    private const string itemSocketName = "ItemSocket";
    
    private Vector3 movePointer = Vector3.zero;

    private void initTransform()
    {
        Transform[] ts = gameObject.transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts)
        {
            if (t.gameObject.name == itemSocketName)
            {
                itemSocket = t;
            }
            if(t.gameObject.name == toolSocketName) 
            {
                toolSocket = t;
            }

            if (toolSocket != null && itemSocket != null)
                return;
        }
        if(toolSocket == null)
             Debug.LogError("All Characters should have tool Socket");
    }
    
    public void Start()
    {
        if (pState == null)
            pState = this.gameObject.GetComponent<PlayerState>();
        
        _playerStateMachineObservables = animator.GetBehaviour<PlayerStateMachineObservables>();
        
        //carry animation
        Observable.EveryUpdate()
            .Where(_ => animator.GetBool(AnimationStateString.isCarry) == false)
            .TakeWhile(_ => pState.getItemCount() > 0)
            .Repeat()
            .DistinctUntilChanged()
            .Subscribe(_ =>
            {
                animator.SetBool(AnimationStateString.isCarry,true);
            })
            .AddTo(this);
        
        
        //smash animation
        Observable.EveryUpdate()
            .SkipUntil(_playerStateMachineObservables.OnStateEnterObservable)
            .Where(_ => animator.GetBool(AnimationStateString.isSmash))
            .TakeUntil(_playerStateMachineObservables.OnStateExitObservable)
            .Repeat()
            .DistinctUntilChanged()
            .Throttle(TimeSpan.FromMilliseconds(400))
            .Subscribe(_ =>
            {
                animator.SetBool(AnimationStateString.isSmash,false);
                if(interactables.Count > 0)
                {
                    foreach(var interactable in interactables)
                    {
                        interactable.OnInteract(pState);
                    }
                }
            })
            .AddTo(this);

        //running animation exit
        Observable.EveryUpdate()
            .SkipUntil(_playerStateMachineObservables.OnStateEnterObservable)
            .TakeWhile(_ => animator.GetBool(AnimationStateString.isMove))
            .Repeat()
            .DistinctUntilChanged()
            .Select(_ => movePointer)
            .Where(x => x == Vector3.zero)
            .Subscribe(_ =>
            {
                //Logger.Log("stop");
                animator.SetBool(AnimationStateString.isMove,false);
            })
            .AddTo(this);

        //running animation enter
        Observable.EveryUpdate()
            .SkipUntil(_playerStateMachineObservables.OnStateEnterObservable)
            //.Where(_ => animator.GetBool(PlayerStateString.isMove) == false)
            //.TakeUntil(_playerStateMachineObservables.OnStateExitObservable)
            .TakeWhile(_ => animator.GetBool(AnimationStateString.isMove) == false)
            .Repeat()
            .DistinctUntilChanged()
            .Select(_ => movePointer)
            .Where(x => x != Vector3.zero)
            .Subscribe(_ =>
            {
                Logger.Log("run");
                animator.SetBool(AnimationStateString.isMove,true);
            })
            .AddTo(this);

        initTransform();
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
        _aState = AnimationStateEnum.Running;
        animator.SetBool(AnimationStateString.isMove,true);
    }

    public void action(bool value)
    {
        if(animator.GetBool(AnimationStateString.isSmash) == false)
            animator.SetBool(AnimationStateString.isSmash,true);
    }

    public void SetEquipment(Tool tool)
    {
        tool.transform.SetParent(toolSocket);
        tool.transform.localPosition = new Vector3(0,0,0);
        tool.transform.localRotation = Quaternion.identity;
        tool.transform.localScale = Vector3.one;
    }

    public void getItem(Item item)
    {
        item.transform.SetParent(itemSocket);
        item.transform.localPosition = new Vector3(0,0,0);
        item.transform.localRotation = Quaternion.identity;
        item.transform.localScale = Vector3.one;
    }

    public void UnsetEquipment()
    {

    }
}
