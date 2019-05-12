using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCollision : MonoBehaviour
{
    MeshRenderer meshRenderer;
    Color color;

    //プレイヤーが獲得した時の資源ポイントの増分
    public const int pointAddition = 100;

    //ドロップ時からの経過時間
    float timeFromDropped;

    //ドロップされてから点滅を開始するまでの時間
    const float startFlashingTime = 3.0f;

    //ドロップされてから消滅するまでの時間
    const float destroyTime = 6.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer = null)
        {
            Debug.Log("資源のメッシュレンダラーデータの取得に失敗");
        }

        color = meshRenderer.material.color;

        timeFromDropped = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //ドロップされてからの経過時間を記録
        timeFromDropped += Time.deltaTime;

        //ドロップから一定時間経過で消滅
        if (timeFromDropped >= destroyTime)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        //ドロップから一定時間経過で点滅を開始
        if (timeFromDropped >= startFlashingTime)
        {
            if (meshRenderer.material.color.a != 0.0f)
            {
                meshRenderer.material.color = new Color(color.r, color.g, color.b, 0.0f);
            }
            else
            {
                meshRenderer.material.color = new Color(color.r, color.g, color.b, 1.0f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //衝突物がプレイヤーであれば、その資源獲得数に追加する。
        if (other.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
