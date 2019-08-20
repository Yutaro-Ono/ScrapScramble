using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerControl : MonoBehaviour
{
    Animator animator;
    HammerAttack attackAnimScript;

    public BoxCollider hitCollider;

    [SerializeField]
    BoxCollider obtainCollider;

    public bool droppedMode = true;

    [SerializeField]
    Vector3 droppedModeScale;

    // 攻撃力（プレイヤーに対しては落とす資源の数）
    public short power = 3;

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

        if (droppedMode)
        {
            gameObject.transform.localScale = droppedModeScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // コライダーの適宜切り替え
        obtainCollider.enabled = droppedMode;
        if (droppedMode)
        {
            hitCollider.enabled = false;
        }
    }

    private void OnEnable()
    {
        // ハンマー本体の回転の初期化
        hitCollider.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }

    private void OnTriggerStay(Collider other)
    {
        if (droppedMode)
        {
            if (other.tag == "Player")
            {
                PlayerStatus status = other.GetComponent<PlayerStatus>();

                status.EquipWeapon(Weapon.Hammer);

                Debug.Log("ハンマー：プレイヤー" + (status.GetId()) + "が取得");

                Destroy(gameObject);
            }
        }
        else
        {
            //if (animator.GetBool(attackAnimScript.otherBoolName) && other.tag == "Player")
            if (other.tag == "Player")
            {
                // 当たったプレイヤーが装備者自身でなければダメージ処理
                if (other.gameObject != transform.parent.parent.gameObject)
                {
                    PlayerMovement move = other.GetComponent<PlayerMovement>();
                    move.DropResource((uint)power);
                    
                    Debug.Log("ハンマー：プレイヤーにヒット");
                }
            }

            // エネミーに対しての判定
            else if (other.tag == "Enemy")
            {
                // 対象のステータスを取得
                EnemyStatus status = other.GetComponent<EnemyStatus>();
                if (status == null)
                {
                    Debug.Log("ステータス取得失敗");
                }

                // 敵にダメージ
                status.hitPoint -= power;

                Debug.Log("ハンマー：エネミーにヒット、残りHP" + status.hitPoint);
            }

            //コリジョンを無効にすることで一瞬だけ判定を行う
            hitCollider.enabled = false;
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
