using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickPlayAnimationChangeScene : MonoBehaviour
{
    public string nextSceneName;

    private bool clicked = false;

    private void OnMouseDown()
    {
        if (clicked) return;
        clicked = true;

        Debug.Log("Clicked! Changing scene to: " + nextSceneName);
        SceneManager.LoadScene(nextSceneName);
    }
}
