using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    private Text textObj = null;
    private Image spriteObj = null;

    GameObject UIManager;

    BlinkUI blink;

    // Start is called before the first frame update
    void Start()
    {
        UIManager = GameObject.Find("UIManager");
        blink = UIManager.GetComponent<BlinkUI>();

        textObj = this.GetComponentInParent<Text>();
        spriteObj = this.GetComponentInParent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (textObj != null)
        {
            textObj.color = blink.GetAlphaColor(textObj.color);
        }
        if (spriteObj != null)
        {
            spriteObj.color = blink.GetAlphaColor(spriteObj.color);
        }

    }
}
