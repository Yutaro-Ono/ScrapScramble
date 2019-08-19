using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    // 自分のコンポーネント類
    PlayerStatus status;
    PlayerMovement moveScript;

    // Waveを見て動きを判定する
    WaveManagement waveManager;

    // 対エネミーWaveであるか
    bool vsEnemyWave;
    
    // 検出されたエネミー
    List<GameObject> detectedEnemy = new List<GameObject>();

    // 検出されたプレイヤー
    List<GameObject> detectedPlayer = new List<GameObject>();

    // 移動、体当たりの方向
    Vector3 targetVector;

    // ランダム移動中であるかのフラグ
    bool randomWalkingFlag;

    // 体当たり操作フラグ
    bool chargeFlag;
    bool tackleFlag;

    // 武器操作フラグ
    bool weaponAttackFlag;

    private void Awake()
    {
        waveManager = GameObject.Find("WaveManager").GetComponent<WaveManagement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        status = GetComponent<PlayerStatus>();
        moveScript = GetComponent<PlayerMovement>();

        targetVector = new Vector3(0, 0, 0);

        weaponAttackFlag = chargeFlag = tackleFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 体当たり実行フラグの更新
        tackleFlag = false;

        // 現在装備している武器情報の更新
        Weapon weapon = status.GetCurrentWeapon();
        
        // 対エネミーWave？
        vsEnemyWave = (waveManager.wave == WaveManagement.WAVE_NUM.WAVE_1_PVE || waveManager.wave == WaveManagement.WAVE_NUM.WAVE_3_PVE);

        // 対エネミーWaveであれば
        if (vsEnemyWave)
        {
            // 検出範囲内にエネミーがいれば
            if (detectedEnemy.Count != 0)
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

                targetVector = leastDistance;

                // 行動判断
                // 武器を持っている場合
                if (weapon != Weapon.None)
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

                    
                }

                // 攻撃手段がない場合
                else
                {

                }
            }

            // 検出範囲内にエネミーがいない場合
            else
            {

            }
        }

        // 対プレイヤーWave、インターバルWaveであれば
        else
        {
            // じゃあどのWave？
            bool vsPlayerWave = (waveManager.wave == WaveManagement.WAVE_NUM.WAVE_2_PVP || waveManager.wave == WaveManagement.WAVE_NUM.WAVE_4_PVP);
            
            // 対プレイヤーWaveの場合
            if (vsPlayerWave)
            {

            }

            // そうでない（インターバルWave）場合
            else
            {

            }
        }
    }

    private void LateUpdate()
    {
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
}
