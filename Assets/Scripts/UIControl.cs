using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIControl : MonoBehaviour
{
    public void onClickPlay()
    {
        SceneManager.LoadScene("Stage");
    }

    public void onClickOption()
    {
        Debug.Log("Option");
    }

    public void onClickQuit()
    {
        Application.Quit();
    }
}
