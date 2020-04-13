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
        changeAnimation(PlayerState.Running);
    }
    public void action(bool action)
    {
        changeAnimation(PlayerState.Smash);
    }

    private void changeAnimation(PlayerState animation)
    {
        switch (animation)
        {
            case PlayerState.Idle :
                break;
            case PlayerState.Running :
                animator.SetTrigger(PlayerStateStiring.isMove);
                break;
            case PlayerState.Carry:
                animator.SetTrigger(PlayerStateStiring.isCarry);
                break;
            case PlayerState.UnCarry:
                break;
            case PlayerState.Smash:
                animator.SetTrigger(PlayerStateStiring.isSmash);
                break;
        }

        _state = animation;
    }
}
