using UnityEngine;
using UnityEngine.Video; // จำเป็นสำหรับการคุม Video
using UnityEngine.SceneManagement; // จำเป็นสำหรับการเปลี่ยน Scene

public class VideoSceneChanger : MonoBehaviour
{
    public VideoPlayer videoPlayer; // ลาก VideoPlayer มาใส่
    public string nextSceneName;    // พิมพ์ชื่อ Scene ที่จะไป

    void Start()
    {
        // ถ้าลืมลากมาใส่ ให้ลองหาจากตัวมันเอง
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();

        // ✅ สั่งให้ฟังเหตุการณ์: เมื่อเล่นจบ (loopPointReached) ให้เรียกฟังก์ชัน OnVideoFinished
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("Video Ended! Changing Scene...");
        SceneManager.LoadScene(nextSceneName);
    }
}