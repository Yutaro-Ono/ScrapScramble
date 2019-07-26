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
    
    public float GetHorizontalInput()
    {
        float ret = Input.GetAxis(horizontalInputName);
        if (ret != 0)
        {
            Debug.Log(horizontalInputName + ":" + ret);
        }
        return ret;
    }

    public float GetVerticalInput()
    {
        float ret = Input.GetAxis(verticalInputName);
        if (ret != 0)
        {
            Debug.Log(verticalInputName + ":" + ret);
        }
        return ret;
    }

    public bool GetTackleInput()
    {
        bool ret = Input.GetButton(tackleInputName);
        Debug.Log(tackleInputName + ":" + ret);
        return ret;
    }

    public bool GetTackleInputUp()
    {
        return Input.GetButtonUp(tackleInputName);
    }

    public bool GetWeaponAttackInput()
    {
        bool ret = Input.GetButton(weaponAttackInputName);
        Debug.Log(weaponAttackInputName + ":" + ret);
        return ret;
    }
}
