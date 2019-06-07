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
        //当たり判定を行うかのフラグを更新
        //Update関数はOnCollisionEnter関数より後に呼ばれるらしいので、ここで行って問題ないはず
        if (animator.GetBool(attackAnimScript.collisionBoolName))
        {
            animator.SetBool(attackAnimScript.collisionBoolName, false);
        }

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

            //フラグをfalseにすることで、ハンマーを振り下ろした一瞬だけ判定を有効にする
            animator.SetBool(attackAnimScript.collisionBoolName, false);

            collider.enabled = false;
        }
    }

    public void Attack()
    {
        if (attackingFlag == false)
        {
            animator.SetTrigger("Attack");
            attackingFlag = true;
        }
    }

    public bool SetAttackingFlag(bool value)
    {
        return (attackingFlag = value);
    }
}
