using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    // コンポーネント類
    WaveManagement waveManager;

    // 検出されたエネミー
    List<GameObject> detectedEnemy = new List<GameObject>();

    Vector3 targetVector;

    private void Awake()
    {
        waveManager = GameObject.Find("WaveManager").GetComponent<WaveManagement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        targetVector = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (detectedEnemy.Count != 0)
        {
            // 敵との距離を比較
            Vector3 leastDistance = detectedEnemy[0].transform.position - gameObject.transform.position;
            leastDistance.y = 0;

            for (int i = 1; i < detectedEnemy.Count; ++i)
            {
                Vector3 compareDistance = detectedEnemy[i].transform.position - gameObject.transform.position;
                compareDistance.y = 0;

                if (leastDistance.magnitude > compareDistance.magnitude)
                {
                    leastDistance = compareDistance;
                }
            }

            targetVector = leastDistance;
        }
    }

    private void LateUpdate()
    {
        // リストをクリア
        detectedEnemy.Clear();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            // エネミー検出
            detectedEnemy.Add(other.gameObject);
        }
    }

    public Vector3 GetTargetVector()
    {
        return targetVector;
    }
}
