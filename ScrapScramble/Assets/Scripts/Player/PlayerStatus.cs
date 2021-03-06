﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    // AI行動を行うかどうか
    public bool AIFlag;

    // ウェーブの情報
    static WaveManagement waveManager = null;

    // 現在装備している武器
    Weapon currentWeapon;

    // プレイヤーID
    int id;

    // 自分の武器
    GameObject myHammer;
    GameObject myGatling;
    GameObject myRailgun;

    public int score;
    int prevScore;
    public int hp;
    private float increase;
    public int armedStage = 0;
    const int armedStageLimit = 4;      // 巨大化の段階数
    // 次の巨大化までいくつの資源オブジェクトが必要か。
    public const int armedStageUpResourceMass = 25;
    // 巨大化したときのスケール値の増分
    public const float armedStageUpScaleIncrease = 0.5f;
    public short chargeAttackPower;
    bool getItem;
    bool nextBody;

    // 次の巨大化段階までどのくらいのスコアが必要か
    public const int armedStageUpScore = armedStageUpResourceMass * ResourceCollision.pointAddition;

    float initialScale;

    public int gatlingDamage = 0;
    public static int gatlingPatience = 5;

    // エフェクト再生タイミング(巨大化するとき)
    public bool becomeBiggerEffectPlayTiming = false;

    // エフェクト再生タイミング(小さくなる時)
    public bool becomeSmallerEffectPlayTiming = false;

    // エフェクト再生タイミング(資源を落とすとき)
    public bool dropResourceEffectPlayTiming = false;

    // エフェクト再生タイミング(資源を得たとき)
    public bool obtainResourceEffectPlayTiming = false;

    void Start()
    {
        // ウェーブ情報取得
        if (waveManager == null)
        {
            waveManager = PlayerManagement.GetWaveManager();
        }

        // なにも装備していない状態
        currentWeapon = Weapon.None;

        // 元々のスケール値
        initialScale = transform.localScale.x;

        // 各武器のオブジェクトを子から取得し、各々装備されるまで非アクティブ化
        if ((myHammer = transform.Find("Core").transform.Find("Hammer").gameObject) == null)
        {
            Debug.Log("プレイヤー：ハンマーのオブジェクト取得に失敗");
        }
        else
        {
            myHammer.SetActive(false);
        }

        if ((myGatling = transform.Find("Core").transform.Find("Gatling").gameObject) == null)
        {
            Debug.Log("プレイヤー：ガトリングのオブジェクト取得に失敗");
        }
        else
        {
            myGatling.SetActive(false);
        }

        if ((myRailgun = transform.Find("Core").transform.Find("Railgun").gameObject) == null)
        {
            Debug.Log("プレイヤー：レールガンのオブジェクト取得に失敗");
        }
        else
        {
            myRailgun.SetActive(false);
        }
    }
    
    void BodyBigger()
    {
        // チェック前の変数を保存
        int beforeArmedStage = armedStage;

        // 巨大化の段階チェック
        for (int i = 0; i < armedStageLimit; i++)
        {
            if (score >= i * armedStageUpResourceMass * ResourceCollision.pointAddition)
            {
                armedStage = i;
            }
        }

        // 巨大化を行う
        for (int i = 0; i < armedStageLimit; i++)
        {
            if (i == armedStage)
            {
                float scale = initialScale + (armedStageUpScaleIncrease * i);

                gameObject.transform.localScale = new Vector3(scale, scale, scale);
            }
        }

        // ビフォーアフター比較
        // 巨大化した時
        if (beforeArmedStage < armedStage)
        {
            // エフェクト再生指示
            becomeBiggerEffectPlayTiming = true;
        }
        // 小さくなったとき
        else if (beforeArmedStage > armedStage)
        {
            // エフェクト再生指示
            becomeSmallerEffectPlayTiming = true;
        }
    }

    void Update()
    {
        if (prevScore != score)
        {
            BodyBigger();

            // エフェクト指示
            // 資源を得たとき
            if (prevScore < score)
            {
                // エフェクト再生指示
                obtainResourceEffectPlayTiming = true;
            }
            // 資源を落としたとき
            else
            {
                dropResourceEffectPlayTiming = true;
            }
        }

        // デバッグ的にコマンドで装備変更
        if (CommonFunction.GetAnyShiftPressed())
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                EquipWeapon(Weapon.Hammer);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                EquipWeapon(Weapon.Gatling);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                EquipWeapon(Weapon.Railgun);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                EquipWeapon(Weapon.None);
            }
        }

        // スコア値を記録
        prevScore = score;
    }

    private void LateUpdate()
    {
        becomeBiggerEffectPlayTiming = false;
        becomeSmallerEffectPlayTiming = false;
        dropResourceEffectPlayTiming = false;
        obtainResourceEffectPlayTiming = false;
    }

    // 現在装備している武器を取得
    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    // 任意の武器を装備
    // equipment->装備したい武器
    public void EquipWeapon(Weapon equipment)
    {
        // 現在の装備を非アクティブ化
        {
            GameObject currentWeaponObj = GetWeaponObjectFromEnum(currentWeapon);
            if (currentWeaponObj != null)
            {
                currentWeaponObj.SetActive(false);
            }
        }

        // 新たな装備をアクティブ化
        {
            GameObject newWeaponObj = GetWeaponObjectFromEnum(equipment);
            if (newWeaponObj != null)
            {
                newWeaponObj.SetActive(true);
            }
        }

        currentWeapon = equipment;
    }

    // 武器のenumを入力すると対応するゲームオブジェクトを返す関数
    public GameObject GetWeaponObjectFromEnum(Weapon weapon)
    {
        GameObject ret = null;

        if (weapon == Weapon.Hammer)
        {
            ret = myHammer;
        }
        else if (weapon == Weapon.Gatling)
        {
            ret = myGatling;
        }
        else if (weapon == Weapon.Railgun)
        {
            ret = myRailgun;
        }

        return ret;
    }

    public int SetId(int in_id)
    {
        return (id = in_id);
    }

    public int GetId()
    {
        return id;
    }

    public WaveManagement GetWaveManager()
    {
        return waveManager;
    }

    public GamepadInput.GamePad.Index GetControlIndex()
    {
        return GamepadInput.GamePad.CastIntToIndex(id);
    }

    public float GetInitialScale()
    {
        return initialScale;
    }
}
