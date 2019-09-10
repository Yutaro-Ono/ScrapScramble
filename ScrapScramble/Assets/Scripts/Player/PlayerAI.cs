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
        Player_WeaponAttack,
        Player_Escape,
        StopCommand
    }

    // 自分のコンポーネント類
    PlayerStatus status;
    PlayerMovement moveScript;
    SphereCollider detector;

    // ステージの隅っこ
    Vector3 cornerPos1, cornerPos2;

    // 対エネミーWaveであるか
    bool vsEnemyWave;

    // 検出半径
    float initialDetectorRadius;

    // 検出されたエネミー
    List<GameObject> detectedEnemy = new List<GameObject>();

    // 検出されたプレイヤー
    List<GameObject> detectedPlayer = new List<GameObject>(3);

    // 攻撃対象に選んだ敵プレイヤー
    GameObject targetPlayer;

    // 攻撃対象であるプレイヤーを変更する間隔
    // 全員同時にプレイヤーを切り替えると面白くないので、個体ごとにランダムに決める
    float targetPlayerChangeInterval;

    // 同じプレイヤーを狙っていた時間を記録するタイマー
    float targetPlayerTimer;

    // 他プレイヤーから逃げる行動をとるかのフラグ
    bool escapeBehaviorFlag;

    // 他プレイヤーから逃げる行動をとる確率（パーセンテージ）
    float escapeBehaviorPercentage;

    // 移動、体当たりの方向
    Vector3 targetVector;

    // ランダム移動目的地
    Vector3 randomWalkDestination;

    // ランダム移動中であるかのフラグ
    bool randomWalkingFlag;

    // ランダム移動を完了したとみなす距離
    const float randomWalkGoalMargin = 10.0f;

    // 体当たり操作フラグ
    bool chargeFlag;
    bool tackleFlag;

    // 武器操作フラグ
    bool weaponAttackFlag;

    // 現在装備している武器種
    Weapon weapon;

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

        initialDetectorRadius = detector.radius * transform.localScale.x;

        targetVector = new Vector3(0, 0, 0);

        weaponAttackFlag = chargeFlag = tackleFlag = false;

        timer = 0.0f;

        targetPlayer = null;

        targetPlayerChangeInterval = Random.Range(3.0f, 30.0f);

        targetPlayerTimer = 0.0f;

        escapeBehaviorPercentage = Random.Range(40.0f, 75.0f);
        //escapeBehaviorPercentage = 0.0f;

        escapeBehaviorFlag = false;

        Debug.Log("PlayerAI" + status.GetId() + " targetPlayerChangeInterval : " + targetPlayerChangeInterval);
    }

    private void FixedUpdate()
    {
        // 検出範囲を固定
        // プレイヤーが巨大化すると、コリジョンも同じ倍率で大きくなるので調節
        float size = initialDetectorRadius / transform.localScale.x;
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
        weapon = status.GetCurrentWeapon();
        
        // 対エネミーWave？
        vsEnemyWave = (status.GetWaveManager().wave == WaveManagement.WAVE_NUM.WAVE_1_PVE || status.GetWaveManager().wave == WaveManagement.WAVE_NUM.WAVE_3_PVE);
        
        // 距離がある程度近ければランダム移動を完了とする
        if (randomWalkingFlag)
        {
            CheckTargetDistance();

            // 移動方向の更新
            targetVector = randomWalkDestination - transform.position;
        }

        // 対エネミーWaveであれば
        if (vsEnemyWave)
        {
            OnVsEnemyWave();
        }

        // 対プレイヤーWave、インターバルWaveであれば
        else
        {
            // じゃあどのWave？
            bool vsPlayerWave = (status.GetWaveManager().wave == WaveManagement.WAVE_NUM.WAVE_2_PVP || status.GetWaveManager().wave == WaveManagement.WAVE_NUM.WAVE_4_PVP);

            // 対プレイヤーWaveの場合
            if (vsPlayerWave)
            {
                OnVsPlayerWave();
                /*
                // とりあえずランダム移動
                if (!randomWalkingFlag)
                {
                    SetRandomDestination();
                }
                */
            }

            // そうでない（インターバル、またはチュートリアルWave）場合
            else
            {
                // 動かない
                CancelAllAction();
            }
        }
    }

    private void LateUpdate()
    {
        if ((int)timer - prevTimer >= 1 && status.GetId() == 0)
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

    void OnVsEnemyWave()
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
            // ランダム移動の方向が決まっておらず、チャージがされていない場合
            if (!randomWalkingFlag && !chargeFlag)
            {
                // ランダム移動を開始
                SetRandomDestination();
            }
        }
    }

    void OnVsPlayerWave()
    {
        if (targetPlayer != null)
        {
            // タイマーの記録
            targetPlayerTimer += Time.deltaTime;
        }

        // 検出範囲内に他プレイヤーがいれば
        if (detectedPlayer.Count != 0)
        {
            // ターゲットの変更時間が来るか、ターゲットが未だ定められていないとき
            // ターゲットを再設定
            if (targetPlayerTimer >= targetPlayerChangeInterval || targetPlayer == null)
            {
                // リストを距離が近い順にソート
                SortPlayerList();

                // 何番目に近いプレイヤーを選ぶのかを設定
                int targetNearRank = Random.Range(0, detectedPlayer.Count);

                // ターゲットにするプレイヤーを決定
                targetPlayer = detectedPlayer[targetNearRank];

                // ターゲット維持タイマーの初期化
                targetPlayerTimer = 0.0f;

                // 戦うか逃げるかの選択
                // escapeBehaviorPercentage%の確率で逃げる行動をとる
                escapeBehaviorFlag = (escapeBehaviorPercentage > Random.Range(0.0f, 100.0f));
            }

            // ターゲットのいる方向から移動入力の方向を確定する
            // ただし、ランダム移動中の場合は除く
            if (!randomWalkingFlag)
            {
                // ターゲットから逃げない場合、ターゲットがいる方向を移動入力の方向とする
                if (!escapeBehaviorFlag)
                {
                    targetVector = targetPlayer.transform.position - gameObject.transform.position;
                }

                // ターゲットから逃げる場合、ターゲットがいる方向と反対方向を移動入力の方向とする
                else
                {
                    targetVector = gameObject.transform.position - targetPlayer.transform.position;
                }

                // Y方向は0に
                targetVector.y = 0.0f;
            }

            // 敵プレイヤーから逃げない場合
            if (!escapeBehaviorFlag)
            {
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
                    behavior = PlayerBehavior.Player_WeaponAttack;
                }

                // 体当たりが可能な場合
                // 一時無効
                else if (false && !moveScript.GetCoolTimeFlag())
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
                    behavior = PlayerBehavior.Player_Tackle;
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

            // 敵プレイヤーから逃げる場合
            else
            {
                behavior = PlayerBehavior.Player_Escape;
            }
        }

        // 検出範囲内にプレイヤーがいない場合
        else
        {
            // ターゲットの変更時間が来たとき
            if (targetPlayerTimer >= targetPlayerChangeInterval)
            {
                // いったん目標をnullに
                targetPlayer = null;
            }

            // ランダム移動
            if (!randomWalkingFlag)
            {
                SetRandomDestination();
            }
        }
    }

    void SortPlayerList()
    {
        // バブルソート
        // 合ってるか不安なので後ほど脳内シミュレートします
        for (int i = 0; i < detectedPlayer.Count - 1; ++i)
        {
            for (int j = 0; j < detectedPlayer.Count - 1 - i; ++j)
            {
                Vector3 vec1 = detectedPlayer[j].transform.position - gameObject.transform.position;
                Vector3 vec2 = detectedPlayer[j + 1].transform.position - gameObject.transform.position;

                // 通常のmagnitudeより高速で計算できるらしいので。
                // 二乗されていても比較には問題ないはず。
                if (vec2.sqrMagnitude > vec1.sqrMagnitude)
                {
                    GameObject tmp = detectedPlayer[j];
                    detectedPlayer[j] = detectedPlayer[j + 1];
                    detectedPlayer[j + 1] = tmp;
                }
            }
        }
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

        randomWalkDestination = new Vector3(moveX, 0, moveZ);
        targetVector = randomWalkDestination - transform.position;

        behavior = PlayerBehavior.RandomWalk;
    }

    void CheckTargetDistance()
    {
        Vector3 pos = transform.position;
        pos.y = 0;

        Vector3 distance = randomWalkDestination - pos;
        if (distance.magnitude <= randomWalkGoalMargin)
        {
            randomWalkingFlag = false;
        }
    }

    void CancelAllAction()
    {
        chargeFlag = false;
        tackleFlag = false;
        randomWalkingFlag = false;

        targetPlayer = null;
        targetPlayerTimer = 0.0f;

        randomWalkDestination = Vector3.zero;
        targetVector = Vector3.zero;

        behavior = PlayerBehavior.StopCommand;
    }

    public Vector3 GetTargetVector()
    {
        return targetVector.normalized;
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
}
