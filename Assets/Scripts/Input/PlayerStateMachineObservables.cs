using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerStateMachineObservables : StateMachineBehaviour
{
    #region OnStateEnter
    private Subject<AnimatorStateInfo> onStateEnterSubject = new Subject<AnimatorStateInfo>();
    public IObservable<AnimatorStateInfo> OnStateEnterObservable => onStateEnterSubject;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        onStateEnterSubject.OnNext(stateInfo);
    }
    #endregion

    #region OnStateExit
    private Subject<AnimatorStateInfo> onStateExitSubject = new Subject<AnimatorStateInfo>();
    public IObservable<AnimatorStateInfo> OnStateExitObservable => onStateExitSubject;
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        onStateExitSubject.OnNext(stateInfo);
    }
    #endregion

    /*
    #region OnStateUpdate
    private Subject<AnimatorStateInfo> onStateUpdateSubject = new Subject<AnimatorStateInfo>();
    public IObservable<AnimatorStateInfo> OnStateUpdateObservable => onStateUpdateSubject;
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        onStateUpdateSubject.OnNext(stateInfo);
    }
    #endregion

    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
    }

    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
    }*/
}
