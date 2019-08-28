using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Weapon
{
    None,           //何も持たない状態
    Hammer,         //ハンマー
    Gatling,        //ガトリング
    Railgun,        //レールガン
    Invalid         //無効値
}

public class WeaponEnumDefine : MonoBehaviour
{
    // レイヤー名
    public const string TouchablePlayerLayerName = "PlayerTouchable";
    public const string UntouchablePlayerLayerName = "PlayerUntouchable";

    public const string StayingHammerLayerName = "StayingHammer";
    public const string WeaponLayerName = "Weapon";
}
