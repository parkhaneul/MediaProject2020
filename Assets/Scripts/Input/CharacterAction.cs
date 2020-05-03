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
    public Tool equipment { get; private set; }
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
            .Subscribe(_ => { 
                //Logger.Log("Carry");
                animatorSet(AnimationStateString.isCarry, true);
            })
            .AddTo(this);

        //unCarry animation
        Observable.EveryUpdate()
            .Where(_ => animator.GetBool(AnimationStateString.isCarry))
            .TakeWhile(_ => pState.getItemCount() == 0)
            .Repeat()
            .DistinctUntilChanged()
            .Subscribe(_ =>
            {
                //Logger.Log("UnCarry");
                animatorSet(AnimationStateString.isCarry, false);
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
                //Logger.Log("Smash");
                animatorSet(AnimationStateString.isSmash,false);
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
                animatorSet(AnimationStateString.isMove,false);
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
                //Logger.Log("run");
                animatorSet(AnimationStateString.isMove, true);
                //animator.SetBool(AnimationStateString.isMove,true);
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

    public void animatorSet(string str, bool value)
    {
        Logger.Log(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name + "\n" +
            "IsCarry : " + animator.GetBool(AnimationStateString.isCarry) + "\n" +
                   "IsMove : " + animator.GetBool(AnimationStateString.isMove) + "\n" +
                   "IsSmash : " + animator.GetBool(AnimationStateString.isSmash));
        animator.SetBool(str,value);
    }
    
    public void interaction()
    {
        if(pState.hasItem())
            throwItem();
        else
            action();
    }

    public void throwItem()
    {
        var item = pState.putItem(0);

        if (item == null)
            return;

        var go = item.gameObject;
        go.transform.SetParent(transform.root.transform.parent);
        ObjectMovementSystem.Instance.shoot(go, this.gameObject.transform.forward,1, 1,
            () => { ObjectMovementSystem.Instance.turn(go, true); });
    }

    public void action()
    {
        if(animator.GetBool(AnimationStateString.isSmash) == false)
            animator.SetBool(AnimationStateString.isSmash,true);
    }
    public void unmount(bool value)
    {
        UnsetEquipment();
    }
    public void SetEquipment(Tool tool)
    {
        tool.transform.SetParent(toolSocket);
        tool.transform.localPosition = new Vector3(0,0,0);
        tool.transform.localRotation = Quaternion.identity;
        tool.transform.localScale = Vector3.one;

        equipment = tool;
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
        if(equipment != null)
        {
            equipment.transform.SetParent(null);
            equipment.transform.position = gameObject.transform.position;
            equipment.transform.rotation = Quaternion.identity;
            equipment.transform.localScale = Vector3.one;
            equipment.GroundMode();

            equipment = null;
        }
    }
}
