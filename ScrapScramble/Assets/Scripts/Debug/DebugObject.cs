using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugObject : MonoBehaviour
{
    public static bool debugFlag = true;

    public bool[] AIFlag = new bool[4];

    // Start is called before the first frame update
    void Awake()
    {
        if (debugFlag)
        {
            int nonAI = 0;
            for (int i = 0; i < 4; ++i)
            {
                ChoiceMenuSceneController.getReady[i] = !AIFlag[i];

                if (ChoiceMenuSceneController.getReady[i])
                {
                    nonAI++;
                }
            }

            ChoiceMenuSceneController.playerNum = nonAI;
        }
    }
}
