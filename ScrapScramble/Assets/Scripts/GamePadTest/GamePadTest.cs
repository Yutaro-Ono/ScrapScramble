
//=============================================================================
//	@file	GamePadTest.h
//	@brief	ゲームパッドテスト
//	@autor	相知 拓弥
//	@date	2018/10/6
//=============================================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-----------------------------------------------------------------------------
//	@brief	ゲームパッドテストクラス
//-----------------------------------------------------------------------------
public class GamePadTest : MonoBehaviour
{
    const int MIN_PAD_NUM = 0;      //  パッド番号の最小
    const int MAX_PAD_NUM = 4;      //  パッド番号の最大

    const float STICK_DEAD_ZONE = 0.3f;     //  スティックのデッドゾーン
    const float DPAD_DEAD_ZONE = 0.5f;      //  十字ボタンのデッドゾーン
    const float TRIGGER_DEAD_ZONE = 0.5f;   //  トリガーのデッドゾーン

    public int m_padNum = 0;      //  パッドの番号

    //-----------------------------------------------------------------------------
    //	@brief	初期化
    //-----------------------------------------------------------------------------
	void Start ()
    {
		
	}
	
    //-----------------------------------------------------------------------------
    //	@brief	更新
    //-----------------------------------------------------------------------------
	void Update ()
    {
        //  入力テスト
		GamePadInputTest(m_padNum);
	}
    
    //-----------------------------------------------------------------------------
    //	@brief	ゲームパッドの入力確認
    //-----------------------------------------------------------------------------
    void GamePadInputTest( int _padNum)
    {
        //  ゲームパッドの軸入力確認
        InputTestAxis(_padNum);

        //  ゲームパッドのボタン入力確認
        InputTestButton(_padNum);
    }

    //-----------------------------------------------------------------------------
    //	@brief	ゲームパッドの軸入力確認
    //-----------------------------------------------------------------------------
    void InputTestAxis( int _padNum)
    {
        //  範囲外検知
        bool isOutOfRange = _padNum < MIN_PAD_NUM || _padNum > MAX_PAD_NUM;
        if(isOutOfRange) { Debug.Log("パッドの[ " + _padNum + " ]番は範囲外です。"); return; }

        //  左ステック水平方向への入力
        float leftStickHorizontalValue = Input.GetAxis("GamePadLeftStickHorizontal" + _padNum);
        bool isLeftStickHorizontalRight = leftStickHorizontalValue >= STICK_DEAD_ZONE;
        bool isLeftStickHorizontalLeft = leftStickHorizontalValue <= -STICK_DEAD_ZONE;
        if(isLeftStickHorizontalRight) { Debug.Log("パッドの[ " + _padNum + " ]番の左スティックが右方向に入力されました。"); }
        if(isLeftStickHorizontalLeft) { Debug.Log("パッドの[ " + _padNum + " ]番の左スティックが左方向に入力されました。"); }

        //  左ステック垂直方向への入力
        float leftStickVerticalValue = Input.GetAxis("GamePadLeftStickVertical" + _padNum);
        bool isLeftStickVerticalUp = leftStickVerticalValue >= STICK_DEAD_ZONE;
        bool isLeftStickVerticalDown = leftStickVerticalValue <= -STICK_DEAD_ZONE;
        if(isLeftStickVerticalUp) { Debug.Log("パッドの[ " + _padNum + " ]番の左スティックが上方向に入力されました。"); }
        if(isLeftStickVerticalDown) { Debug.Log("パッドの[ " + _padNum + " ]番の左スティックが下方向に入力されました。"); }

        //  十字ボタン水平方向への入力
        float dpadHorizontallValue = Input.GetAxis("GamePadDpadHorizontal" + _padNum);
        bool isDpadHorizontallRight = dpadHorizontallValue >= DPAD_DEAD_ZONE;
        bool isDpadHorizontallLeft = dpadHorizontallValue <= -DPAD_DEAD_ZONE;
        if(isDpadHorizontallRight) { Debug.Log("パッドの[ " + _padNum + " ]番の十字ボタンが右方向に入力されました。"); }
        if(isDpadHorizontallLeft) { Debug.Log("パッドの[ " + _padNum + " ]番の十字ボタンが左方向に入力されました。"); }

        //  十字ボタン水平方向への入力
        float dpadVerticallValue = Input.GetAxis("GamePadDpadVertical" + _padNum);
        bool isDpadVerticalUp = dpadVerticallValue >= DPAD_DEAD_ZONE;
        bool isDpadVerticalDown = dpadVerticallValue <= -DPAD_DEAD_ZONE;
        if(isDpadVerticalUp) { Debug.Log("パッドの[ " + _padNum + " ]番の十字ボタンが上方向に入力されました。"); }
        if(isDpadVerticalDown) { Debug.Log("パッドの[ " + _padNum + " ]番の十字ボタンが下方向に入力されました。"); }

        //  左ステック水平方向への入力
        float rightStickHorizontalValue = Input.GetAxis("GamePadRightStickHorizontal" + _padNum);
        bool isRightStickHorizontalRight = rightStickHorizontalValue >= STICK_DEAD_ZONE;
        bool isRightStickHorizontalLeft = rightStickHorizontalValue <= -STICK_DEAD_ZONE;
        if(isRightStickHorizontalRight) { Debug.Log("パッドの[ " + _padNum + " ]番の右スティックが右方向に入力されました。"); }
        if(isRightStickHorizontalLeft) { Debug.Log("パッドの[ " + _padNum + " ]番の右スティックが左方向に入力されました。"); }

        //  左ステック垂直方向への入力
        float rightStickVerticalValue = Input.GetAxis("GamePadRightStickVertical" + _padNum);
        bool isRightStickVerticalUp = rightStickVerticalValue >= STICK_DEAD_ZONE;
        bool isRightStickVerticalDown = rightStickVerticalValue <= -STICK_DEAD_ZONE;
        if(isRightStickVerticalUp) { Debug.Log("パッドの[ " + _padNum + " ]番の右スティックが上方向に入力されました。"); }
        if(isRightStickVerticalDown) { Debug.Log("パッドの[ " + _padNum + " ]番の右スティックが下方向に入力されました。"); }

        //  トリガーへの入力
        float triggerValue = Input.GetAxis("GamePadTrigger" + _padNum);
        bool isRightTrigger = triggerValue <= -TRIGGER_DEAD_ZONE;
        bool isLeftTrigger = triggerValue >= TRIGGER_DEAD_ZONE;
        if (isRightTrigger) { Debug.Log("パッドの[ " + _padNum + " ]番の右トリガーが入力されました。"); }
        if (isLeftTrigger) { Debug.Log("パッドの[ " + _padNum + " ]番の左トリガーが入力されました。"); }
    }

    //-----------------------------------------------------------------------------
    //	@brief	ゲームパッドのボタン入力確認
    //-----------------------------------------------------------------------------
    void InputTestButton( int _padNum)
    {
        //  範囲外検知
        bool isOutOfRange = _padNum < MIN_PAD_NUM || _padNum > MAX_PAD_NUM;
        if(isOutOfRange) { Debug.Log("パッドの[ " + _padNum + " ]番は範囲外です。"); return; }

        //  Aボタン
        bool isActiveButtonA = Input.GetButtonDown("GamePadButtonA" + _padNum);
        if(isActiveButtonA) { Debug.Log("パッドの[ " + _padNum + " ]番のAボタンが入力されました。"); }

        //  Bボタン
        bool isActiveButtonB = Input.GetButtonDown("GamePadButtonB" + _padNum);
        if(isActiveButtonB) { Debug.Log("パッドの[ " + _padNum + " ]番のBボタンが入力されました。"); }

        //  Xボタン
        bool isActiveButtonX = Input.GetButtonDown("GamePadButtonX" + _padNum);
        if(isActiveButtonX) { Debug.Log("パッドの[ " + _padNum + " ]番のXボタンが入力されました。"); }

        //  Yボタン
        bool isActiveButtonY = Input.GetButtonDown("GamePadButtonY" + _padNum);
        if(isActiveButtonY) { Debug.Log("パッドの[ " + _padNum + " ]番のYボタンが入力されました。"); }

        //  Leftボタン
        bool isActiveButtonLeft = Input.GetButtonDown("GamePadButtonLeft" + _padNum);
        if(isActiveButtonLeft) { Debug.Log("パッドの[ " + _padNum + " ]番のLeftボタンが入力されました。"); }

        //  Rightボタン
        bool isActiveButtonRight = Input.GetButtonDown("GamePadButtonRight" + _padNum);
        if(isActiveButtonRight) { Debug.Log("パッドの[ " + _padNum + " ]番のRightボタンが入力されました。"); }

        //  Backボタン
        bool isActiveButtonBack = Input.GetButtonDown("GamePadButtonBack" + _padNum);
        if(isActiveButtonBack) { Debug.Log("パッドの[ " + _padNum + " ]番のBackボタンが入力されました。"); }

        //  Startボタン
        bool isActiveButtonStart = Input.GetButtonDown("GamePadButtonStart" + _padNum);
        if(isActiveButtonStart) { Debug.Log("パッドの[ " + _padNum + " ]番のStartボタンが入力されました。"); }

        //  LeftStickボタン
        bool isActiveButtonLeftStick = Input.GetButtonDown("GamePadButtonLeftStick" + _padNum);
        if(isActiveButtonLeftStick) { Debug.Log("パッドの[ " + _padNum + " ]番のLeftStickボタンが入力されました。"); }

        //  RightStickボタン
        bool isActiveButtonRightStick = Input.GetButtonDown("GamePadButtonRightStick" + _padNum);
        if(isActiveButtonRightStick) { Debug.Log("パッドの[ " + _padNum + " ]番のRightStickボタンが入力されました。"); }
    }
}
