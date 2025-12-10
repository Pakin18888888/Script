using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class stuff : MonoBehaviour
{
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
        public bool useJumpScare = false;   // ติ๊กเพื่อให้ทำ Jump Scare
        public JumpScare jumpScare;         // ลากสคริปต์ JumpScare มาวาง


    void Start()
    {
        if (GJ != null) GJ.gameObject.SetActive(false);
        if (GJ1 != null) GJ1.gameObject.SetActive(false);
        
        pickUpAllowed = false;
    }

    void Update()
    {
        if(pickUpAllowed && Input.GetKeyDown(KeyCode.E))
        {
            // --- ส่วนที่ตัดออก: ไม่ควรไปยุ่งกับ interactionRange ของผู้เล่นตรงนี้ ---
            // Player.Instance.interactionRange = 6.0f; 
            // -----------------------------------------------------------

            // ถ้าเป็นตู้ Jump Scare
            if (useJumpScare && !ghostSpawned)
            {
                ghostSpawned = true;

                // เรียกจั้มฟ์สแก
                if (jumpScare != null)
                {
                    jumpScare.Play();   // <-- เรียกตรงนี้
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

            // หมายเหตุ: การใช้ Time.timeScale = 0 จะทำให้เกมหยุด "ทุกอย่าง" รวมทั้งผีด้วย
            // ถ้าตั้งใจให้เป็นตู้ซ่อนตัว (Hide) ไม่ควรหยุดเวลาครับ
            // แต่ถ้าตั้งใจให้เป็นหน้าอ่านกระดาษ (Read Note) การหยุดเวลาถือว่าถูกต้องครับ
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
        
        // --- ส่วนที่ตัดออก: ลบ else ที่ไปลดระยะแขนผู้เล่นทิ้ง ---
        /* else
        {
            Player.Instance.interactionRange = 1.0f; 
        }
        */
        // ---------------------------------------------------
        
    }
    
    private void SpawnGhost()
    {
        // ✅ แก้ไข: ใช้ Player.Instance เช็คได้เลย ไม่ต้อง Find ให้หนักเครื่อง
        if (Player.Instance == null || ghostPrefab == null) return;

        Vector3 centerPoint = transform.position;
        
        // สุ่มมุม 0-360 องศา
        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        
        // คำนวณทิศทางสุ่ม
        Vector3 randomDir = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0f);
        
        // คำนวณตำแหน่งเกิด
        Vector3 spawnPos = centerPoint + randomDir.normalized * ghostSpawnDistance;
        spawnPos.z = 0f;

        // สร้างผี
        GameObject newGhost = Instantiate(ghostPrefab, spawnPos, Quaternion.identity);
        
        // ✅ แก้ไข: ส่ง transform ของ Player.Instance ไปให้ผี
        GhostAI ghostAI = newGhost.GetComponent<GhostAI>();
        if (ghostAI != null)
        {
            ghostAI.playerTransform = Player.Instance.transform;
        }

        Debug.Log("Spawned ghost from cabinet: " + gameObject.name + " at " + spawnPos);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ใช้ CompareTag ประหยัดกว่า ==
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