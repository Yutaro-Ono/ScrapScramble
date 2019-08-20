using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    PlayerStatus status;
    Rewired.Player input;

    // Start is called before the first frame update
    void Start()
    {
        status = GetComponent<PlayerStatus>();

        // 操作入力値名の設定
        int id = status.GetId();

        // 参照の取得
        input = Rewired.ReInput.players.GetPlayer(id);
    }

    public float GetHorizontalInput()
    {
        return input.GetAxis("HorizontalMove");
    }

    public float GetVerticalInput()
    {
        return input.GetAxis("VerticalMove");
    }

    public bool GetTackleInput()
    {
        return input.GetButton("Tackle");
    }

    public bool GetTackleInputUp()
    {
        return input.GetButtonUp("Tackle");
    }

    public bool GetWeaponAttackInput()
    {
        return input.GetButton("WeaponAttack");
    }
}
