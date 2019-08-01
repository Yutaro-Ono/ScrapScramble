using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkUI : MonoBehaviour
{
    private float blinkSpeed = 0.7f;        // 点滅速度
    private float time = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        time = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime * 3.0f * blinkSpeed;
    }

    // Alpha値を更新してColorを返す
    public Color GetAlphaColor(Color color)
    {
        color.a = Mathf.Sin(time) * 0.5f + 0.5f;

        return color;
    }
}
