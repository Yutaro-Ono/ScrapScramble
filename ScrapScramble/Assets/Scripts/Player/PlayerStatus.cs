using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    // 現在装備している武器
    Weapon currentWeapon;

    // プレイヤーID
    // 0から数えることに注意->例えばプレイヤー1のIDは0
    int id;

    // 自分の武器
    GameObject myHammer;
    GameObject myGatling;
    GameObject myRailgun;

    public int score;
    int prevScore;
    public int hp;
    public short atk;
    private float increase;
    public int armedStage = 0;
    const int armedStageLimit = 4;      // 巨大化の段階数
    // 次の巨大化までいくつの資源オブジェクトが必要か。
    public const int armedStageUpResourceMass = 10;
    public short chargeAttackPower;
    bool getItem;
    bool nextBody;

    // 次の巨大化段階までどのくらいのスコアが必要か
    public const int armedStageUpScore = armedStageUpResourceMass * ResourceCollision.pointAddition;

    float initialScale;

    void Start()
    {
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

    void ChargeAttack()
    {
        PlayerMovememt moveScript = GetComponent<PlayerMovememt>();
        if (moveScript.chargePower <= 100)
        {
            chargeAttackPower = moveScript.chargePower;
        }
        if (moveScript.chargeFlg==true)
        {
            atk = chargeAttackPower;
        }
       
    }

    void BodyBigger()
    {
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
                float scale = initialScale + (0.5f * i);

                gameObject.transform.localScale = new Vector3(scale, scale, scale);
            }
        }
    }

    void Update()
    {
        if (prevScore != score)
        {
            BodyBigger();
        }
        ChargeAttack();

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
}
