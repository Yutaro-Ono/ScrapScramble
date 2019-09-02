using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public static void CheckEscapeForQuitApp()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            
#elif UNITY_STANDALONE
            UnityEngine.Application.Quit();
            
#endif
        }
    }
}
