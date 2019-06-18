using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 残念ながらまだ使えません
public class CommonFunction : MonoBehaviour
{
    public static bool GetAnyShiftPressed()
    {
        bool ret = false;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            ret = true;
        }

        return ret;
    }
}
