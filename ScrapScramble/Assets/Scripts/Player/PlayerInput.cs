using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    PlayerStatus status;

    // 各種操作の入力値の名前
    string horizontalInputName;
    string verticalInputName;
    string tackleInputName;
    string weaponAttackInputName;
    
    // Start is called before the first frame update
    void Start()
    {
        status = GetComponent<PlayerStatus>();

        // 操作入力値名の設定
        int id = status.GetId();
        horizontalInputName = "Horizontal" + id;
        verticalInputName = "Vertical" + id;
        tackleInputName = "Tackle" + id;
        weaponAttackInputName = "WeaponAttack" + id;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetHorizontalInput()
    {
        float ret = Input.GetAxis(horizontalInputName);

        if (ret != 0)
        {
            Debug.Log("プレイヤー" + (status.GetId() + 1) + "：縦方向操作入力");
        }

        return ret;
    }

    public float GetVerticalInput()
    {
        return Input.GetAxis(verticalInputName);
    }

    public bool GetTackleInput()
    {
        bool ret = Input.GetButton(tackleInputName);

        if (ret)
        {
            Debug.Log("プレイヤー" + (status.GetId() + 1) + "：体当たり操作入力");
        }

        return ret;
    }

    public bool GetTackleInputUp()
    {
        return Input.GetButtonUp(tackleInputName);
    }

    public bool GetWeaponAttackInput()
    {
        return Input.GetButton(weaponAttackInputName);
    }
}
