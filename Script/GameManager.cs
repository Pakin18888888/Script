using UnityEngine;
using UnityEngine.SceneManagement; // อย่าลืมบรรทัดนี้
using System.Collections.Generic; // สำหรับ List

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PlayerData player;
    public SceneStateData sceneState;

    // --- ส่วนที่เพิ่มเข้ามาใหม่สำหรับระบบวาร์ป ---
    public string nextSpawnID;
    // -------------------------------------

    public GameObject gameOverUIPrefab;

    void Start()
    {
        if (GameOverUI.Instance == null)
        {
            Instantiate(gameOverUIPrefab);
        }
    }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            player = new PlayerData();
            sceneState = new SceneStateData();

            // ผูก event เมื่อโหลดฉากเสร็จ
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    // --- ฟังก์ชันสำหรับวาร์ป (ยกมาจาก GameSceneManager) ---
    public void ChangeScene(string sceneName, string targetSpawnID)
    {
        nextSpawnID = targetSpawnID; // จำ ID เป้าหมาย
        SceneManager.LoadScene(sceneName);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // หา Player ตัวจริง (ที่เป็น Singleton)
        Player currentPlayer = Player.Instance;
        
        // ถ้ามี Player และมีการระบุ nextSpawnID ไว้
        if (currentPlayer != null && !string.IsNullOrEmpty(nextSpawnID))
        {
            SpawnPoint[] spawns = FindObjectsOfType<SpawnPoint>();
            foreach (SpawnPoint sp in spawns)
            {
                if (sp.spawnID == nextSpawnID)
                {
                    currentPlayer.transform.position = sp.transform.position;
                    break;
                }
            }
            nextSpawnID = null; // รีเซ็ต
        }
    }

    [System.Serializable]
public class PlayerData
{
    public int hp = 100;
    // เก็บค่าอื่นๆ ของผู้เล่นที่นี่
}

[System.Serializable]
public class SceneStateData
{
    // ลิสต์เก็บชื่อประตูที่เปิดไปแล้ว
    public List<string> openedDoors = new List<string>(); 
    public List<string> playedJumpScare = new List<string>();
}

}