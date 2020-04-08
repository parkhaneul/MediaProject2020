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
    private bool canMove;

    public void move(Point point)
    {
        if (canMove)
        {
            run();
            turn(point);
            this.gameObject.transform.position += this.gameObject.transform.forward * moveSpeed;
        }
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

    public void stop()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f)
            changeAnimation(PlayerState.Idle);
    }

    private void changeAnimation(PlayerState animation)
    {
        switch (animation)
        {
            case PlayerState.Idle :
                canMove = true;
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
                canMove = false;
                animator.SetBool(PlayerStateStiring.isSmash,true);
                break;
        }
    }
}
