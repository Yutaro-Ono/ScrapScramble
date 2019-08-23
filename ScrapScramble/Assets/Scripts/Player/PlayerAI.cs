using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    enum PlayerBehavior
    {
        RandomWalk,
        Enemy_Tackle,
        Enemy_WeaponAttack,
        Player_Tackle,
        Player_WeaponAttack
    }

    // 自分のコンポーネント類
    PlayerStatus status;
    PlayerMovement moveScript;
    SphereCollider detector;

    // ステージの隅っこ
    Vector3 cornerPos1, cornerPos2;

    // 対エネミーWaveであるか
    bool vsEnemyWave;
    
    // 検出されたエネミー
    List<GameObject> detectedEnemy = new List<GameObject>();

    // 検出されたプレイヤー
    List<GameObject> detectedPlayer = new List<GameObject>();

    // 移動、体当たりの方向
    Vector3 targetVector;

    // ランダム移動目的地
    Vector3 destination;

    // ランダム移動中であるかのフラグ
    bool randomWalkingFlag;

    // 体当たり操作フラグ
    bool chargeFlag;
    bool tackleFlag;

    // 武器操作フラグ
    bool weaponAttackFlag;

    // 選択した行動
    PlayerBehavior behavior;

    // AI駆動時間
    float timer;

    // 前フレームにおけるタイマー
    // 小数点以下を切り捨て
    int prevTimer;

    private void Awake()
    {
        cornerPos1 = GameObject.Find("Corner Point1").transform.position;
        cornerPos2 = GameObject.Find("Corner Point2").transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        status = GetComponent<PlayerStatus>();
        moveScript = GetComponent<PlayerMovement>();
        detector = transform.Find("Detector").GetComponent<SphereCollider>();

        targetVector = new Vector3(0, 0, 0);

        weaponAttackFlag = chargeFlag = tackleFlag = false;

        timer = 0.0f;
    }

    private void FixedUpdate()
    {
        // 検出範囲を固定
        // プレイヤーが巨大化すると、コリジョンも同じ倍率で大きくなるので調節
        float size = 100.0f / transform.localScale.x;
        detector.radius = size;
    }

    // Update is called once per frame
    void Update()
    {
        // 駆動時間を記録
        prevTimer = (int)timer;
        timer += Time.deltaTime;

        // 体当たり実行フラグの更新
        tackleFlag = false;

        // 武器攻撃フラグの更新
        weaponAttackFlag = false;

        // 現在装備している武器情報の更新
        Weapon weapon = status.GetCurrentWeapon();
        
        // 対エネミーWave？
        vsEnemyWave = (status.GetWaveManager().wave == WaveManagement.WAVE_NUM.WAVE_1_PVE || status.GetWaveManager().wave == WaveManagement.WAVE_NUM.WAVE_3_PVE);

        // 距離がある程度近ければランダム移動を完了とする
        if (randomWalkingFlag)
        {
            CheckTargetDistance();

            // 移動方向の更新
            targetVector = destination - transform.position;
        }

        // 対エネミーWaveであれば
        if (vsEnemyWave)
        {
            // 検出範囲内にエネミーがいれば
            if (detectedEnemy.Count != 0)
            {
                if (!randomWalkingFlag)
                {
                    // 敵との距離を比較
                    Vector3 leastDistance = detectedEnemy[0].transform.position - gameObject.transform.position;
                    leastDistance.y = 0;

                    for (int i = 1; i < detectedEnemy.Count; ++i)
                    {
                        Vector3 compareDistance = detectedEnemy[i].transform.position - gameObject.transform.position;
                        compareDistance.y = 0;

                        if (leastDistance.magnitude > compareDistance.magnitude)
                        {
                            leastDistance = compareDistance;
                        }
                    }

                    targetVector = leastDistance - transform.position;
                }

                // 行動判断
                // 武器を持っている場合
                // かつ、体当たりのチャージをしていない場合
                if (weapon != Weapon.None && !chargeFlag)
                {
                    // ハンマー
                    if (weapon == Weapon.Hammer)
                    {
                        if (targetVector.magnitude < 15.0f)
                        {
                            weaponAttackFlag = true;
                        }
                    }

                    // それ以外
                    else
                    {
                        weaponAttackFlag = true;
                    }

                    randomWalkingFlag = false;
                    behavior = PlayerBehavior.Enemy_WeaponAttack;
                }

                // 体当たりが可能な場合
                else if (!moveScript.GetCoolTimeFlag())
                {
                    // 体当たりの威力が3を超えた後、ランダムなタイミングで体当たりを実行
                    if (moveScript.tacklePower >= 3 && Random.Range(0, 31) == 0)
                    {
                        chargeFlag = false;
                        tackleFlag = true;
                    }
                    else
                    {
                        // 実行まではチャージ
                        chargeFlag = true;
                    }

                    randomWalkingFlag = false;
                    behavior = PlayerBehavior.Enemy_Tackle;
                }

                // 攻撃手段がない場合
                else
                {
                    if (!randomWalkingFlag)
                    {
                        SetRandomDestination();
                    }
                }
            }

            // 検出範囲内にエネミーがいない場合
            else
            {
                // チャージ中なら行わない
                if (!randomWalkingFlag && !chargeFlag)
                {
                    SetRandomDestination();
                }
            }
        }

        // 対プレイヤーWave、インターバルWaveであれば
        else
        {
            // じゃあどのWave？
            bool vsPlayerWave = (status.GetWaveManager().wave == WaveManagement.WAVE_NUM.WAVE_2_PVP || status.GetWaveManager().wave == WaveManagement.WAVE_NUM.WAVE_4_PVP);

            // 対プレイヤーWaveの場合
            if (vsPlayerWave)
            {
                // とりあえずランダム移動
                if (!randomWalkingFlag)
                {
                    SetRandomDestination();
                }
            }

            // そうでない（インターバルWave）場合
            else
            {
                // 動かない
                CancelAllAction();
            }
        }
    }

    private void LateUpdate()
    {
        if ((int)timer - prevTimer >= 1 && status.GetId() == 1)
        {
            Debug.Log(behavior);
        }

        // リストをクリア
        detectedEnemy.Clear();
        detectedPlayer.Clear();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            // エネミー検出
            detectedEnemy.Add(other.gameObject);
        }

        if (!vsEnemyWave && other.tag == "Player")
        {
            // 他プレイヤー検出
            detectedPlayer.Add(other.gameObject);
        }
    }
    
    public Vector3 GetTargetVector()
    {
        Vector3 ret = targetVector.normalized;
        /*
        float largerValue;
        if (ret.x > ret.z)
        {
            largerValue = ret.x;
        }
        else
        {
            largerValue = ret.z;
        }

        if (largerValue != 0)
        {
            ret.x /= largerValue;
            ret.z /= largerValue;
        }
        */

        return ret;
    }

    public bool GetChargeFlag()
    {
        return chargeFlag;
    }

    public bool GetTackleFlag()
    {
        return tackleFlag;
    }

    public bool GetWeaponAttackFlag()
    {
        return weaponAttackFlag;
    }

    void SetRandomDestination()
    {
        randomWalkingFlag = true;

        Vector3 larger, smaller;
        if (cornerPos1.x > cornerPos2.x)
        {
            larger.x = cornerPos1.x;
            smaller.x = cornerPos2.x;
        }
        else
        {
            larger.x = cornerPos2.x;
            smaller.x = cornerPos1.x;
        }

        if (cornerPos1.z > cornerPos2.z)
        {
            larger.z    = cornerPos1.z;
            smaller.z   = cornerPos2.z;
        }
        else
        {
            larger.z    = cornerPos2.z;
            smaller.z   = cornerPos1.z;
        }

        float margin = 20.0f;
        larger.x -= margin;
        smaller.x += margin;
        larger.z -= margin;
        smaller.z += margin;

        float moveX, moveZ;
        moveX = Random.Range(smaller.x, larger.x);
        moveZ = Random.Range(smaller.z, larger.z);

        destination = new Vector3(moveX, 0, moveZ);
        targetVector = destination - transform.position;

        behavior = PlayerBehavior.RandomWalk;
    }

    void CheckTargetDistance()
    {
        Vector3 pos = transform.position;
        pos.y = 0;

        Vector3 distance = destination - pos;
        if (distance.magnitude <= 10.0f)
        {
            randomWalkingFlag = false;
        }
    }

    void CancelAllAction()
    {
        chargeFlag = false;
        tackleFlag = false;
        randomWalkingFlag = false;

        destination = Vector3.zero;
        targetVector = Vector3.zero;
    }
}
