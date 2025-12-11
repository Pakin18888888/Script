using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance;

    [Header("UI Panel")]
    public GameObject gameOverPanel;

    [Header("Sound")]
    public AudioSource gameOverSound;

    void Awake()
    {
        // ป้องกัน GameOverUI ซ้ำ
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    // ======================================================
    //  SHOW GAME OVER
    // ======================================================
    public void ShowGameOver()
    {
        Debug.Log("📌 GAMEOVER SHOWN");

        // ลบ UI ทั้งหมดใน Scene ปัจจุบัน (ยกเว้น GameOverUI)
        RemoveAllSceneUI();

        // ปิดเสียงทุกอันในเกมก่อน
        StopAllAudioSources();

        // เล่นเสียง GameOver
        if (gameOverSound != null)
            gameOverSound.Play();

        // แสดง UI
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // ปิดการควบคุม Player
        if (Player.Instance != null)
            Player.Instance.canMove = false;
    }

    // ======================================================
    //  รีทั้งเกมแบบ CLEAN RESET
    // ======================================================
    public void ResetEntireGame()
    {
        Debug.Log("🔄 RESET ENTIRE GAME");

        // ลบทุกวัตถุ DontDestroyOnLoad ยกเว้น GameOverUI
        DestroyAllDDOLExceptGameOver();

        // ล้าง Static references
        Player.Instance = null;
        sanitySystem.Instance = null;
        GameManager.Instance = null;
        Inventory.Instance = null;

        // โหลด Scene เริ่มต้น
        SceneManager.LoadScene("meroom");

        // ปิด UI GameOver
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    // ======================================================
    //  ฟังก์ชันลบ UI ทั้งหมดให้เหลือแค่ GameOverUI
    // ======================================================
    void RemoveAllSceneUI()
    {
        Canvas[] canvases = FindObjectsOfType<Canvas>();

        foreach (var cv in canvases)
        {
            if (cv.gameObject != this.gameOverPanel &&
                cv.gameObject != this.gameObject)
            {
                Debug.Log("🗑 ลบ UI ใน Scene: " + cv.gameObject.name);
                Destroy(cv.gameObject);
            }
        }
    }

    // ======================================================
    //  ปิดเสียงทั้งหมด
    // ======================================================
    void StopAllAudioSources()
    {
        AudioSource[] audios = FindObjectsOfType<AudioSource>();

        foreach (var a in audios)
        {
            if (a != gameOverSound)
            {
                a.Stop();
                Destroy(a.gameObject);
            }
        }
    }

    // ======================================================
    //  ลบ DDOL ทั้งหมด ยกเว้น GameOverUI
    // ======================================================
    void DestroyAllDDOLExceptGameOver()
    {
        var allObjects = FindObjectsOfType<GameObject>();

        foreach (var obj in allObjects)
        {
            if (obj.scene.name == null || obj.scene.name == "")
            {
                if (obj != this.gameObject)
                {
                    Debug.Log("🗑 ลบ DDOL: " + obj.name);
                    Destroy(obj);
                }
            }
        }
    }
}
