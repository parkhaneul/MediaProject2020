using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UniRx;
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
    public Animator animator;
    public float moveSpeed;

    private PlayerStateMachineObservables _playerStateMachineObservables;

    public void Start()
    {
        _playerStateMachineObservables = animator.GetBehaviour<PlayerStateMachineObservables>();

        _playerStateMachineObservables
            .OnStateEnterObservable
            .Throttle(TimeSpan.FromSeconds(1))
            .Where(x => x.IsName("Base Layer.Smash"))
            .Subscribe(_ =>
            {
                animator.SetBool(PlayerStateString.isSmash, false);
            })
            .AddTo(this);

        _playerStateMachineObservables
            .OnStateEnterObservable
            .Where(x => x.IsName("Base Layer.Running"))
            .Subscribe(_ =>
            {
                animator.SetBool(PlayerStateString.isMove,false);
            })
            .AddTo(this);
    }

    public void move(Point point)
    {
        run();
        turn(point);
        this.gameObject.transform.position += this.gameObject.transform.forward * moveSpeed;
    }

    public void turn(Point point)
    {
        var lookAtVector = this.transform.position + new Vector3(point.x, point.y, point.z);
        this.gameObject.transform.LookAt(lookAtVector);
    }

    public void run()
    {
        animator.SetBool(PlayerStateString.isMove,true);
    }

    public void action(bool value)
    {
        animator.SetBool(PlayerStateString.isSmash,true);
    }
}
