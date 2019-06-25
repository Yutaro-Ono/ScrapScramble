//------------------------------------------------------//
// UI上の武装アイコン表示・非表示管理
// 各プレイヤーにアタッチを想定
//-----------------------------------------------------//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class weaponIcon : MonoBehaviour
{

    public Image hammerIcon;

    public Image GatlingIcon;

    public Image RailgunIcon;

    // ステータス格納用
    PlayerStatus status;

    // Start is called before the first frame update
    void Start()
    {
        hammerIcon.enabled = false;
        GatlingIcon.enabled = false;
        RailgunIcon.enabled = false;

        status = this.GetComponent<PlayerStatus>();

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(status.GetCurrentWeapon());

        // 表示・非表示の管理
        if (status.GetCurrentWeapon() == Weapon.Hammer)
        {
            hammerIcon.enabled = true;
            Debug.Log("ハンマー表示");
        }
        else
        {
            hammerIcon.enabled = false;
        }

        if (status.GetCurrentWeapon() == Weapon.Gatling)
        {
            GatlingIcon.enabled = true;
            Debug.Log("ガトリング表示");
        }
        else
        {
            GatlingIcon.enabled = false;
        }

        if (status.GetCurrentWeapon() == Weapon.Railgun)
        {
            RailgunIcon.enabled = true;
            Debug.Log("レールガン表示");
        }
        else
        {
            RailgunIcon.enabled = false;
        }

    }

}
