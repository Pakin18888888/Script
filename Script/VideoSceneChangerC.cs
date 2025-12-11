using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoSceneChangerC : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string mainMenuSceneName = "HomeMenu"; // ← ซีนเริ่มเกมของคุณ

    void Start()
    {
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();

        // เมื่อวิดีโอเล่นจบ
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("Video ended — resetting game...");

        // ✔ 1. ลบทุกวัตถุ DontDestroyOnLoad
        CleanupDontDestroyOnLoad();

        // ✔ 2. โหลดซีนหลัก
        SceneManager.LoadScene(mainMenuSceneName);
    }

    void CleanupDontDestroyOnLoad()
    {
        // ดึง GameObject ทั้งหมดมาเช็ค
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.scene.name == null || obj.scene.name == "") // อยู่ใน DontDestroyOnLoad
            {
                Debug.Log("Removing: " + obj.name);
                Destroy(obj);
            }
        }
    }
}
