﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerAttack : StateMachineBehaviour
{
    //アニメーター内のパラメーター名
    //この名前のbool型変数によってコリジョンの有効・無効を操作する
    //public string collisionBoolName = "CollisionActive";

    //攻撃トリガー名
    public string attackTrigger = "Attack";

    //ハンマー本体のスクリプト
    HammerControl control;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        control = animator.gameObject.GetComponent<HammerControl>();
        if (control == null)
        {
            Debug.Log("ハンマーコントロールの取得失敗");
        }

        //コリジョンを有効化
        control.hitCollider.enabled = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("ハンマー：攻撃アニメーション終了");
    }

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
