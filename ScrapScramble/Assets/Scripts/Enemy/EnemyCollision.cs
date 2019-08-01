using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// エネミーの当たり判定を管理
public class EnemyCollision : MonoBehaviour
{
    // On/Offを切り替えるコライダー（ただしトリガーでない）
    BoxCollider notTriggerCollider;

    // Start is called before the first frame update
    void Start()
    {
        // 自身についているBoxColliderの中からトリガーでないものを探す
        BoxCollider[] tmpColliders = GetComponents<BoxCollider>();
        for (int i =0; i < tmpColliders.Length; ++i)
        {
            if (!tmpColliders[i].isTrigger)
            {
                notTriggerCollider = tmpColliders[i];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // 基本的に物理的コライダーは無効
        // FixedUpdateって本来、こんな風に物理関連の初期化をやるところだと思うの・・・。
        notTriggerCollider.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            // プレイヤーの行動情報を取得
            PlayerMovememt playerMove = other.GetComponent<PlayerMovememt>();

            // プレイヤーが体当たり中なら当たり判定を有効に
            if (playerMove.tacklingFlag)
            {
                notTriggerCollider.enabled = true;
            }
        }
    }
}
