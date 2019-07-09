//----------------------------------------------------------//
// PlayerのWorld座標を取得する
//       ※動くUIの親オブジェクトにアタッチ
//--------------------------------------------------------//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetingInterface : MonoBehaviour
{
    public GameObject playerObj;

    private Vector3 pos;

    // 位置調整用
    private Vector3 offset = new Vector3(0, 30.0f, 18.0f);

    void Start()
    {
        pos.x = playerObj.transform.position.x;
        pos.z = playerObj.transform.position.z;
    }

    void Update()
    {
        pos.x = playerObj.transform.position.x;
        pos.z = playerObj.transform.position.z;

        gameObject.transform.position = pos + offset;

    }
}
