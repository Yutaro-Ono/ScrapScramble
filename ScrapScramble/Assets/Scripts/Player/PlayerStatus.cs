using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    // 現在装備している武器
    Weapon currentWeapon;

    public int score;
    int prevScore;
    public int hp;
    public short atk;
    private float increase;
    public int armedStage = 0;
    const int armedStageLimit = 4;      // 巨大化の段階数
    // 次の巨大化までいくつの資源オブジェクトが必要か。
    const int armedStageUpResourceMass = 10;
    public short chargeAttackPower;
    bool getItem;
    bool nextBody;

    float initialScale;

    void Start()
    {
        currentWeapon = Weapon.None;

        // 元々のスケール値
        initialScale = transform.localScale.x;
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
    }

    private void LateUpdate()
    {
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
        currentWeapon = equipment;
    }
}
