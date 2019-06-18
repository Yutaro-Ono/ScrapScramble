using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerControl : MonoBehaviour
{
    Animator animator;
    HammerAttack attackAnimScript;

    public BoxCollider collider;

    //攻撃アニメーション、準備アニメーションが再生中かどうかのフラグ
    //これがfalseの時にアニメーションが再生可能とする
    bool attackingFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        
        attackAnimScript = animator.GetBehaviour<HammerAttack>();

        if (attackAnimScript == null)
        {
            Debug.Log("ハンマー攻撃アニメーションのスクリプト取得に失敗");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //とりあえずコマンド発動
        if (Input.GetKeyDown(KeyCode.H) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            Attack();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //if (animator.GetBool(attackAnimScript.collisionBoolName) && collision.gameObject.tag == "Player")
        if (collision.gameObject.tag == "Player")
        {
            //ここにプレイヤー被ダメージ時の関数を呼ぶ
            Debug.Log("ハンマー：プレイヤーにヒット");

            //コリジョンを無効にすることで一瞬だけ判定を行う
            collider.enabled = false;
        }
    }

    //プレイヤー側操作で発動できるようにする場合は、プレイヤースクリプト内でこの関数を呼ぶことでハンマー攻撃を発動できます
    public void Attack()
    {
        //ハンマー攻撃が作動中かどうかを判定。作動中でないときに攻撃モーションが発動
        if (attackingFlag == false)
        {
            //トリガーによって攻撃モーション発動
            animator.SetTrigger(attackAnimScript.attackTrigger);

            //攻撃中のフラグを設定
            //攻撃モーションが一通り終わったときに、アニメーター側でfalseを代入する
            attackingFlag = true;
        }
    }

    public bool SetAttackingFlag(bool value)
    {
        return (attackingFlag = value);
    }
}
