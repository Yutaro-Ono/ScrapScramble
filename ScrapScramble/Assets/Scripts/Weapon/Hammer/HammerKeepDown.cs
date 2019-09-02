using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerKeepDown : StateMachineBehaviour
{
    //アニメーター内のパラメーター名
    public string floatParamName = "StayAttackingTime";

    //タイマー。ハンマーを振り下ろしたままにしてる時間を計る。
    float timer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //タイマーの初期化
        timer = 0.0f;

        // コリジョンの無効化
        HammerControl control = animator.gameObject.GetComponent<HammerControl>();
        control.hitCollider.enabled = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //経過時間の記録
        timer += Time.deltaTime;

        //代入
        animator.SetFloat(floatParamName, timer);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
