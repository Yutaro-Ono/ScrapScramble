//----------------------------------------------------------//
// World座標をScreen座標に置換する
//       ※Playerタグに紐付け
//--------------------------------------------------------//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetingInterface : MonoBehaviour
{
    public GameObject targetObj;        // ターゲットオブジェクト(Player)
    private Vector3 targetPos;       // ターゲットオブジェクトの座標

    private RectTransform myRect;     // タグのtransform
    private Vector3 offset = new Vector3(0, 20.5f, 0);    // 表示位置調整用オフセット(Playerの頭上に表示するため)




    void Start()
    {
        // GUI上のタグの座標を取得
        myRect = GetComponent<RectTransform>();

        targetPos = targetObj.transform.position;

    }

    void Update()
    {
        // ターゲット座標を更新
        targetPos = targetObj.transform.position;

        // ターゲットの移動に伴って、GUI上の座標を更新
        myRect.position
            = RectTransformUtility.WorldToScreenPoint(Camera.main, targetPos + offset);
    }
}
