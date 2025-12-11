using System.Collections;
using System.Collections.Generic; // เพิ่มบรรทัดนี้เพื่อใช้ HashSet
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class stuff : MonoBehaviour
{
    [Header("Save Settings")]
    public string objectID; // ตั้งชื่อตู้ให้ไม่ซ้ำกัน (เช่น Cab1, Cab2)

    // 🔥 เปลี่ยนมาใช้ static HashSet เพื่อจำค่าเฉพาะตอนรันเกมรอบนั้นๆ
    // พอหยุดเกมแล้วกด Play ใหม่ ตัวนี้จะถูกล้างค่าทิ้งเอง
    private static HashSet<string> triggeredEvents = new HashSet<string>();

    [Header("UI Interact")]
    public TextMeshProUGUI GJ;
    public GameObject GJ1;
    public bool pickUpAllowed;

    [Header("Ghost Settings")]
    public bool hasGhost = false;       
    public bool isJumpScareCabinet = false; 
    public bool ghostSpawned = false;       
    public GameObject ghostPrefab;          
    public float ghostSpawnDistance = 4f;  

    [Header("Jump Scare Settings")]
    public bool useJumpScare = false;   
    public JumpScare jumpScare;         

    void Start()
    {
        if (GJ != null) GJ.gameObject.SetActive(false);
        if (GJ1 != null) GJ1.gameObject.SetActive(false);
        
        pickUpAllowed = false;

        // ✅ เช็คจากตัวแปร static ว่ารอบการเล่นนี้ ตู้นี้เคยทำงานไปหรือยัง
        if (!string.IsNullOrEmpty(objectID) && triggeredEvents.Contains(objectID))
        {
            ghostSpawned = true; // ถ้าเคยทำไปแล้ว ให้ถือว่า Spawn แล้ว (จะไม่ทำซ้ำ)
        }
    }

    void Update()
    {
        if(pickUpAllowed && Input.GetKeyDown(KeyCode.E))
        {
            // ถ้าเป็นตู้ Jump Scare และยังไม่เคยทำงาน
            if (useJumpScare && !ghostSpawned)
            {
                ghostSpawned = true;

                // ✅ บันทึกชื่อตู้นี้ลงในรายการที่ "ทำไปแล้ว"
                if (!string.IsNullOrEmpty(objectID))
                {
                    triggeredEvents.Add(objectID);
                }

                // เรียกจั้มฟ์สแก
                if (jumpScare != null)
                {
                    jumpScare.Play();   
                }

                return;
            }

            // ถ้าตู้นี้ spawn ผี
            if (hasGhost && !ghostSpawned)
            {
                SpawnGhost();
                ghostSpawned = true;
            }

            // สลับสถานะ UI (เปิด/ปิด)
            bool isActive = !GJ1.activeSelf;
            bool isActive1 = !GJ.gameObject.activeSelf;
            
            if (GJ != null) GJ.gameObject.SetActive(isActive1);
            if (GJ1 != null) GJ1.SetActive(isActive);

            if (isActive)
            {
                if (Player.Instance != null) Player.Instance.interactionRange = 10.0f;
                Time.timeScale = 0f;
            }
            else
            {
                if (Player.Instance != null) Player.Instance.interactionRange = 1.0f;
                Time.timeScale = 1f;
            }
        }
    }
    
    private void SpawnGhost()
    {
        if (Player.Instance == null || ghostPrefab == null) return;

        Vector3 centerPoint = transform.position;
        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector3 randomDir = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0f);
        Vector3 spawnPos = centerPoint + randomDir.normalized * ghostSpawnDistance;
        spawnPos.z = 0f;

        GameObject newGhost = Instantiate(ghostPrefab, spawnPos, Quaternion.identity);
        
        GhostAI ghostAI = newGhost.GetComponent<GhostAI>();
        if (ghostAI != null)
        {
            ghostAI.playerTransform = Player.Instance.transform;
        }

        Debug.Log("Spawned ghost from cabinet: " + gameObject.name + " at " + spawnPos);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (GJ != null) GJ.gameObject.SetActive(true);
            pickUpAllowed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(GJ != null)
            {
                GJ.gameObject.SetActive(false);
            }
            pickUpAllowed = false;
        }
    }
}