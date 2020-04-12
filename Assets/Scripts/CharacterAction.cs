using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Carry,
    Running,
    UnCarry,
    Smash,
}

public class PlayerStateStiring
{
    public const string isMove = "IsMove";
    public const string isCarry = "IsCarry";
    public const string isSmash = "IsSmash";
}

public class CharacterAction : MonoBehaviour
{
    public Animator animator;
    public float moveSpeed;
    
    private PlayerState _state;
    private bool _canMove;

    public void move(Point point)
    {
        if(_canMove)
        {
            run();
            turn(point);
            this.gameObject.transform.position += this.gameObject.transform.forward * moveSpeed;
        }
        else
        {
            return;
        }
    }

    public void turn(Point point)
    {
        var lookAtVector = this.transform.position + new Vector3(point.x, point.y, point.z);
        this.gameObject.transform.LookAt(lookAtVector);
    }

    public void run()
    {
        if (_state != PlayerState.Running)
        {
            changeAnimation(PlayerState.Running);
        }
    }
    public void action(bool action)
    {
        if (_state != PlayerState.Smash)
        {
            changeAnimation(PlayerState.Smash);
        }
    }

    public void stop()
    {
        if (_state != PlayerState.Idle)
        {
            changeAnimation(PlayerState.Idle);
        }
    }

    private void changeAnimation(PlayerState animation)
    {
        switch (animation)
        {
            case PlayerState.Idle :
                _canMove = true;
                animator.SetBool(PlayerStateStiring.isMove,false);
                animator.SetBool(PlayerStateStiring.isSmash, false);
                break;
            case PlayerState.Running :
                animator.SetBool(PlayerStateStiring.isMove,true);
                break;
            case PlayerState.Carry:
                animator.SetBool(PlayerStateStiring.isCarry,true);
                break;
            case PlayerState.UnCarry:
                animator.SetBool(PlayerStateStiring.isMove,false);
                break;
            case PlayerState.Smash:
                _canMove = false;
                animator.SetBool(PlayerStateStiring.isSmash,true);
                break;
        }

        _state = animation;
    }
}
