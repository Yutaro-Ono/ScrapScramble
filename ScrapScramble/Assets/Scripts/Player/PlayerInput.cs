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
        return Input.GetAxis(horizontalInputName);
    }

    public float GetVerticalInput()
    {
        return Input.GetAxis(verticalInputName);
    }

    public bool GetTackleInput()
    {
        return Input.GetButton(tackleInputName);
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
