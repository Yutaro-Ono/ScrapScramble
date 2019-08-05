using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class PlayerInput : MonoBehaviour
{
    PlayerStatus status;
    
    GamepadState keyState;
    
    // Start is called before the first frame update
    void Start()
    {
        status = GetComponent<PlayerStatus>();

        // 操作入力値名の設定
        int id = status.GetId();
    }

    private void Update()
    {
        keyState = GamePad.GetState(status.GetControlIndex(), false);
    }

    public float GetHorizontalInput()
    {
        return keyState.LeftStickAxis.x;
    }

    public float GetVerticalInput()
    {
        return keyState.LeftStickAxis.y;
    }

    public bool GetTackleInput()
    {
        return GamePad.GetButton(GamePad.Button.A, status.GetControlIndex());
    }

    public bool GetTackleInputUp()
    {
        return GamePad.GetButtonUp(GamePad.Button.A, status.GetControlIndex());
    }

    public bool GetWeaponAttackInput()
    {
        return GamePad.GetButton(GamePad.Button.B, status.GetControlIndex());
    }
}
