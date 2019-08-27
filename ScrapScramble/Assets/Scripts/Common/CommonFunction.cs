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

    public static void RunLauncher(string path)
    {
        string fullPathexe = path + "Launcher.exe";

        
    }
}
