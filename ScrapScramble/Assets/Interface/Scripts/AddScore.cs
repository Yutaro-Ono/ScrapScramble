using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddScore : MonoBehaviour
{

    [SerializeField]


    // 参照してきたスコアを格納用
    public Text scoreText = null;





    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ToStringScore();
    }

    void ToStringScore()
    {
        // "this"アタッチされたプレーヤーのスコアをセット
        scoreText.text = " " + ((int)this.GetComponent<PlayerStatus>().score).ToString() + "/1000";
    }
}
