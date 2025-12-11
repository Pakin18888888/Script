using UnityEngine;
using TMPro;

public class TutorialUI : MonoBehaviour
{
    public static TutorialUI Instance;

    public TextMeshProUGUI messageText; // ลาก Text ลง Inspector

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ==============================
    // แสดงข้อความ Tutorial
    // ==============================
    public void ShowMessage(string msg)
    {
        if (messageText == null)
        {
            Debug.LogError("❌ TutorialUI ไม่มีข้อความใน messageText!");
            return;
        }

        messageText.gameObject.SetActive(true);
        messageText.text = msg;
    }

    // ==============================
    // ลบ UI บทสอนออกจากเกมแบบถาวร
    // ==============================
    public void DestroyTutorialUI()
    {
        // ลบตัว UI ทั้งก้อน
        Destroy(gameObject);

        // ลบข้อความตกค้างใน Scene ให้หมด
        var tmpObjects = FindObjectsOfType<TextMeshProUGUI>(true);
        foreach (var t in tmpObjects)
        {
            if (t.text.Contains("flashlight") ||
                t.text.Contains("look around"))
            {
                Destroy(t.gameObject);
            }
        }
    }
}
