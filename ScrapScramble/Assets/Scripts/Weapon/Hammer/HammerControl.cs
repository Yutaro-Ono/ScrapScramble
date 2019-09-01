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

    // ウェーブ情報
    static WaveManagement wave = null;

    // 攻撃力（プレイヤーに対しては落とす資源の数）
    public short power = 3;

    //攻撃アニメーション、準備アニメーションが再生中かどうかのフラグ
    //これがfalseの時にアニメーションが再生可能とする
    bool animatingFlag = false;

    // 振り下ろしている間に接触したオブジェクト
    List<GameObject> hitObject = new List<GameObject>(4);

    // 振り下ろしているモーション中であるかのフラグ
    bool attackingFlag = false;

    // エフェクト再生タイミングでtrueになる変数
    public bool effectPlayTiming = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        
        attackAnimScript = animator.GetBehaviour<HammerAttack>();

        if (!droppedMode && wave == null)
        {
            wave = transform.parent.parent.GetComponent<PlayerStatus>().GetWaveManager();
        }

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

        // 再生中アニメーションの情報取得
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.nameHash == Animator.StringToHash("Base Layer.HammerAttack"))
        {
            attackingFlag = true;
        }
        else
        {
            // attackingFlagが真ならば真、偽ならば偽を代入
            effectPlayTiming = attackingFlag;

            attackingFlag = false;
        }

        // 攻撃中でない間リストをクリア
        if (!animatingFlag)
        {
            hitObject.Clear();
        }
    }

    private void LateUpdate()
    {
        // 更新
        effectPlayTiming = false;

        // レイヤーの切り替え
        if (attackingFlag)
        {
            bool vsEnemyWave = (wave.wave == WaveManagement.WAVE_NUM.WAVE_1_PVE || wave.wave == WaveManagement.WAVE_NUM.WAVE_3_PVE);
            gameObject.layer = LayerMask.NameToLayer(vsEnemyWave ? WeaponEnumDefine.UntouchablePlayerLayerName : WeaponEnumDefine.WeaponLayerName);
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer(WeaponEnumDefine.StayingHammerLayerName);
        }
    }

    private void OnEnable()
    {
        // ハンマー本体の回転の初期化
        hitCollider.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        // 各種フラグの初期化
        animatingFlag = false;
        effectPlayTiming = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (droppedMode)
        {
            if (other.tag == "Player")
            {
                PlayerStatus status = other.GetComponent<PlayerStatus>();

                status.EquipWeapon(Weapon.Hammer);

                Debug.Log("ハンマー：プレイヤー" + (status.GetId() + 1) + "が取得");

                Destroy(gameObject);
            }
        }
        else
        {
            bool vsEnemyWave = (wave.wave == WaveManagement.WAVE_NUM.WAVE_1_PVE || wave.wave == WaveManagement.WAVE_NUM.WAVE_3_PVE);
            if (other.tag == "Player" && !vsEnemyWave)
            {
                // 当たったプレイヤーが装備者自身でなければダメージ処理
                if (other.gameObject != transform.parent.parent.gameObject)
                {
                    // そのひと振りで当たっていない相手なら
                    if (!hitObject.Contains(other.gameObject))
                    {
                        PlayerMovement move = other.GetComponent<PlayerMovement>();
                        move.DropResource((uint)power);

                        hitObject.Add(other.gameObject);

                        Debug.Log("ハンマー：プレイヤーにヒット");
                    }
                }
            }

            // エネミーに対しての判定
            else if (other.tag == "Enemy")
            {
                // そのひと振りで当たっていない相手なら
                if (!hitObject.Contains(other.gameObject))
                {
                    // 対象のステータスを取得
                    EnemyStatus status = other.GetComponent<EnemyStatus>();
                    if (status == null)
                    {
                        Debug.Log("ステータス取得失敗");
                    }

                    // 敵にダメージ
                    status.hitPoint -= power;

                    hitObject.Add(other.gameObject);

                    Debug.Log("ハンマー：エネミーにヒット、残りHP" + status.hitPoint);
                }
            }
        }
    }

    //プレイヤー側操作で発動できるようにする場合は、プレイヤースクリプト内でこの関数を呼ぶことでハンマー攻撃を発動できます
    public void Attack()
    {
        //ハンマー攻撃が作動中かどうかを判定。作動中でないときに攻撃モーションが発動
        if (animatingFlag == false)
        {
            //トリガーによって攻撃モーション発動
            animator.SetTrigger(attackAnimScript.attackTrigger);

            //攻撃中のフラグを設定
            //攻撃モーションが一通り終わったときに、アニメーター側でfalseを代入する
            animatingFlag = true;
        }
    }

    public bool SetAttackingFlag(bool value)
    {
        return (animatingFlag = value);
    }
}
