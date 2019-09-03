using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    PlayerStatus status;
    PlayerInput input;
    PlayerAI AI;
    Rigidbody myRigidbody;

    Vector3 prevPos;

    // 初期位置
    Vector3 initialPosition;
    
    // 移動関連
    float speed = 50.0f;
    float moveX = 0.0f;
    float moveZ = 0.0f;
    public bool moveFlag;
    // 体当たり関連
    public bool chargeFlg = false;
    float chargeTimer = 0.0f;
    [SerializeField] bool tacklingFlag;
    public int tacklePower = 0;
    bool coolTimeFlag = false;
    float coolTimer = 0.0f;
    const float CoolTime = 5.0f;

    const float tackleForceScalar = 100.0f;

    // ドロップする資源オブジェクトのプレハブデータ
    GameObject resourcePrefab;

    // ドロップ時に資源を放る強さ
    const float dropAddForceStlength = 500.0f;

    // ドロップ時にどのくらいの距離に資源を生成するか
    const float dropDistance = 15.0f;

    // 通常時のレイヤー名
    public const string normalLayerName = "PlayerLayer";

    // 体当たり中のレイヤー名
    public const string tacklingLayerName = "TacklingPlayerLayer";
    
    private void Start()
    {
        status = GetComponent<PlayerStatus>();
        input = GetComponent<PlayerInput>();
        myRigidbody = GetComponent<Rigidbody>();

        if (status.AIFlag)
        {
            AI = gameObject.AddComponent<PlayerAI>();
        }
        else
        {
            AI = null;
            transform.Find("Detector").GetComponent<SphereCollider>().enabled = false;
        }

        //最初の時点でのプレイヤーのポジションを取得
        prevPos = GetComponent<Transform>().position;
        initialPosition = prevPos;

        tacklingFlag = false;

        resourcePrefab = (GameObject)Resources.Load("Prefabs/Item/Resource/Resource");
        if (resourcePrefab == null)
        {
            Debug.Log("プレイヤー：資源プレハブの読み込みに失敗");
        }
    }

    private void Update()
    {
        // インターバルWaveまたはチュートリアル表示中でないときだけ操作を受け付ける
        WaveManagement.WAVE_NUM currentWave = status.GetWaveManager().wave;
        if (currentWave != WaveManagement.WAVE_NUM.WAVE_INTERVAL && currentWave != WaveManagement.WAVE_NUM.WAVE_TUTORIAL)
        {
            // 武器攻撃の受付
            CheckAttackCommand();

            // 移動操作受付
            CheckMoveCommand();

            // 体当たり操作受付
            CheckTackleCommand();
        }
        // インターバルWaveでは動けないようにする
        else
        {
            // 初期位置に戻す
            transform.position = initialPosition;
            transform.rotation = Quaternion.Euler(Vector3.zero);

            // 移動を封じる
            moveX = 0.0f;
            moveZ = 0.0f;

            // チャージを封じ、体当たりをキャンセル
            chargeFlg = false;
            chargeTimer = 0.0f;
            tacklingFlag = false;
            tacklePower = 0;
            gameObject.layer = LayerMask.NameToLayer(normalLayerName);

            // クールタイムを終わらせる
            coolTimeFlag = false;
            coolTimer = 0.0f;
        }

        // レイヤーの更新
        string layerName = tacklingFlag ? tacklingLayerName : normalLayerName;
        gameObject.layer = LayerMask.NameToLayer(layerName);

        // スコアがマイナスの値をとらないよう調節
        if (status.score < 0)
        {
            status.score = 0;
        }

        // 進行方向を向く
        Vector3 positionDifference = transform.position - prevPos;
        if (positionDifference.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(positionDifference);
        }

        // 現フレームでの座標を記録
        prevPos = transform.position;
    }

    private void FixedUpdate()
    {
        CheckCoolTime();
        
        // 移動
        Vector3 force = new Vector3(moveX * speed, 0.0f, moveZ * speed);
        myRigidbody.AddForce(force);

        // 静止処理
        Brake();
    }

    public bool GetCoolTimeFlag()
    {
        return coolTimeFlag;
    }

    void CheckAttackCommand()
    {
        // AIではなく、武器攻撃操作がされている or AIであり、武器攻撃の判断がなされた
        bool attackCommand = (!status.AIFlag && input.GetWeaponAttackInput()) || (status.AIFlag && AI.GetWeaponAttackFlag());

        if (attackCommand)
        {
            Attack();
        }
    }

    void CheckMoveCommand()
    {
        if (status.AIFlag)
        {
            Vector3 AIMove = AI.GetTargetVector();

            moveX = AIMove.x;
            moveZ = AIMove.z;
        }
        else
        {
            moveX = input.GetHorizontalInput();
            moveZ = input.GetVerticalInput();
        }
    }

    void CheckTackleCommand()
    {
        // クールタイム中なら関数を抜ける
        if (coolTimeFlag)
        {
            return;
        }

        // AIではなく、チャージ操作がされている or AIであり、チャージ判断がなされている
        chargeFlg = (!status.AIFlag && input.GetTackleInput()) || (status.AIFlag && AI.GetChargeFlag());
        if (chargeFlg)
        {
            // チャージタイマー加算
            chargeTimer += Time.deltaTime;

            // 0.5秒につき1ダメージ増加し、3秒で最大ダメージ値に
            for (int i = 0; i < (3.0f / 0.5f); i++)
            {
                if (chargeTimer >= 0.5f * (i + 1))
                {
                    tacklePower = i + 1;
                }
                else
                {
                    break;
                }
            }
        }

        if ((!status.AIFlag && input.GetTackleInputUp()) || (status.AIFlag && AI.GetTackleFlag()))
        {
            // 体当たり実行
            myRigidbody.AddForce(transform.TransformDirection(Vector3.forward) * chargeTimer * tackleForceScalar, ForceMode.Impulse);

            // ダメージを与えられるレベルのチャージがされていた場合
            // 体当たり中フラグとクールタイム中フラグを真に
            tacklingFlag = coolTimeFlag = (chargeTimer >= 0.5f);

            // タイマー初期化
            chargeTimer = 0.0f;
        }
    }

    void CheckCoolTime()
    {
        // クールタイム処理
        if (coolTimeFlag)
        {
            // クールタイムのタイマー計算
            coolTimer += Time.deltaTime;

            // 体当たり中フラグの更新
            if (coolTimer >= 2.0f)
            {
                tacklingFlag = false;
                tacklePower = 0;
            }

            // クールタイム終了判定＆処理
            if (coolTimer >= CoolTime)
            {
                coolTimeFlag = false;
                coolTimer = 0.0f;
            }
        }
    }

    void Brake()
    {
        moveFlag = (moveX != 0 || moveZ != 0);

        if ((!moveFlag && !tacklingFlag) || chargeFlg)
        {
            myRigidbody.velocity = Vector3.zero;
        }
    }

    // dropMass->排出する資源オブジェクトの個数
    // 返却値->最終的にドロップしたオブジェクトの数
    public uint DropResource(uint dropMass)
    {
        // もし指定数の資源オブジェクトを排出できるだけのスコアがない場合
        // 排出数をいじる
        if (status.score < dropMass * ResourceCollision.pointAddition)
        {
            // 1個でも持っているなら排出できるだけする
            if (status.score >= 1 * ResourceCollision.pointAddition)
            {
                dropMass = (uint)status.score / ResourceCollision.pointAddition;
            }
            // 1個も排出できないなら関数を抜ける
            else
            {
                return 0;
            }
        }

        // 1つ目の資源を飛ばす方向をランダムに設定
        Vector3 forceDirectionBase;
        {
            // X, Z成分を-100～100でランダムに決定
            float dirX = Random.Range(-100.0f, 100.0f);
            float dirZ = Random.Range(-100.0f, 100.0f);

            // ベクトル設定・正規化
            forceDirectionBase = new Vector3(dirX, 0.0f, dirZ);
            forceDirectionBase.Normalize();
        }

        // 角度の差を算出
        float angle = 360.0f / (float)dropMass;

        // 排出する
        for (int i = 0; i < dropMass; ++i)
        {
            // その資源が飛ばされるベクトル
            Vector3 forceDirection = forceDirectionBase;
            forceDirection = Quaternion.Euler(0, angle * i, 0) * forceDirection;

            // 巨大化段階から、オブジェクトの座標からどのくらい距離をとるかを計算
            float distanceRate = status.GetInitialScale() + (PlayerStatus.armedStageUpScaleIncrease * status.armedStage);

            // 生成点を巨大化段階によって調節
            float distance = dropDistance * distanceRate;

            // 生成点を計算
            Vector3 instantiatePoint = gameObject.transform.position + forceDirection * distance;
            instantiatePoint.y = dropDistance;

            // 資源のインスタンス生成
            GameObject resource = GameObject.Instantiate(resourcePrefab, instantiatePoint, Quaternion.identity);

            // 物理的に飛ばすため、資源のリジッドボディを取得
            Rigidbody resRigidbody = resource.GetComponent<Rigidbody>();

            // 飛ばす
            resRigidbody.AddForce(forceDirection * dropAddForceStlength);
        }

        // 排出する(現在コメントアウト中)
        {
            /*
            //資源のインスタンス生成
            GameObject[] resource = new GameObject[dropMass];
            for (int i = 0; i < dropMass; i++)
            {
                resource[i] = GameObject.Instantiate(resourcePrefab, gameObject.transform.position + new Vector3(0, 50, 0), Quaternion.identity);
            }

            //資源を物理的に飛ばすため、資源ゲームオブジェクトのリジッドボディを取得
            Rigidbody[] resourceRigidbody = new Rigidbody[dropMass];
            for (int i = 0; i < dropMass; i++)
            {
                resourceRigidbody[i] = resource[i].GetComponent<Rigidbody>();
            }

            //資源を物理的に飛ばす
            for (int i = 0; i < dropMass; i++)
            {
                //飛ばす角度を計算
                float angle = Mathf.PI * 2 / dropMass * i;

                //飛ばす強さを設定
                //この時各方向の成分は取りうる最大値である
                //Vector3 dropDirection = new Vector3(1, Mathf.Sin(Mathf.PI / 4), 1) * dropAddForceStlength;
                Vector3 dropDirection = new Vector3(1, 0.001f, 1) * dropAddForceStlength;

                //角度に応じた値にx,z成分を設定する
                dropDirection.x *= Mathf.Sin(angle);
                dropDirection.z *= Mathf.Cos(angle);

                //ここでやっと飛ばす
                resourceRigidbody[i].AddForce(dropDirection);
            }
            */
        }

        // 資源の減算
        status.score -= (int)(dropMass * ResourceCollision.pointAddition) * 2;

        Debug.Log("Player" + (status.GetId() + 1) + " : " + dropMass + "個の資源を排出");

        return dropMass;
    }

    public uint OnHitGatlingBullet(int power, uint dropMass)
    {
        uint ret = 0;

        status.gatlingDamage += power;

        if (status.gatlingDamage > PlayerStatus.gatlingPatience)
        {
            ret = DropResource(dropMass);

            status.gatlingDamage = 0;
        }

        return ret;
    }

    // 武器による攻撃関数
    void Attack()
    {
        // 現在装備している武器を取得
        // 装備していないなら直ちに関数を抜ける
        // できれば今後、継承を使った効率的なコードに書き換えたい。
        Weapon currentWeapon = status.GetCurrentWeapon();
        if (currentWeapon == Weapon.None)
        {
            return;
        }

        // ゲームオブジェクトを取得
        GameObject currentWeaponObj = status.GetWeaponObjectFromEnum(currentWeapon);
        if (currentWeaponObj == null)
        {
            Debug.Log("ゲームオブジェクトの取得に失敗");
            return;
        }

        // それぞれのスクリプトを取得して、攻撃関数を呼び出す
        // めんどくさいし初心者っぽい書き方だしメモリも食うでしょ？
        // だから今度時間ができたときに継承を使った書き方を試みます。
        if (currentWeapon == Weapon.Hammer)
        {
            HammerControl hammer = currentWeaponObj.GetComponent<HammerControl>();
            hammer.Attack();
        }
        else if (currentWeapon == Weapon.Gatling)
        {
            GatlingControl gatling = currentWeaponObj.GetComponent<GatlingControl>();
            gatling.Attack();
        }
        else if (currentWeapon == Weapon.Railgun)
        {
            RailgunControl railgun = currentWeaponObj.GetComponent<RailgunControl>();
            railgun.Attack();
        }
    }
}