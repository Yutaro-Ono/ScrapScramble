using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultToTitle : MonoBehaviour
{
    public void GameStart()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
