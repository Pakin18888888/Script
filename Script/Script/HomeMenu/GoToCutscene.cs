using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToCutscene : MonoBehaviour
{
    public void GoToCutScene()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("CutScene");
    }
}
