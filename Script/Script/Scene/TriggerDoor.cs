using UnityEngine;
using System.Linq;
using TMPro;
using Unity.VisualScripting; // จำเป็นสำหรับ .Contains

public class TriggerDoor : MonoBehaviour
{
    public string doorID; 
    
    [Header("สถานะ")]
    public bool isLocked = true;

    private Collider2D doorCollider; 
    private SpriteRenderer spriteRenderer;
    public TextMeshProUGUI textMeshProUGUI;

    void Awake()
    {
        doorCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        textMeshProUGUI.gameObject.SetActive(false);
        // --- 1. ตรวจสอบกับ GameManager ว่าประตูนี้เคยเปิดไปหรือยัง ---
        if (GameManager.Instance != null)
        {
            // ถ้าในความทรงจำมีชื่อประตูนี้อยู่ แสดงว่าเคยเปิดแล้ว
            if (GameManager.Instance.sceneState.openedDoors.Contains(doorID))
            {
                OpenInstant(); // สั่งให้เปิดทันทีโดยไม่ต้องไข
            }
        }
    }

    public void UnlockAndOpen()
    {
        if (!isLocked) return; 

        // เช็คกุญแจจาก Inventory
        if (CheckIfPlayerHasKey())
        {
            Debug.Log("ไขกุญแจสำเร็จ: " + doorID);
            isLocked = false;
            OpenDoor();
        }
        else
        {
            Debug.Log("ไม่มีกุญแจสำหรับประตู: " + doorID);
        }
    }

    bool CheckIfPlayerHasKey()
    {
        if (Inventory.Instance == null) return false;
        return Inventory.Instance.keyIDs.Contains(doorID);
    }

    public void OpenDoor()
    {
        // --- 2. บันทึกชื่อประตูลง GameManager ---
        if(GameManager.Instance != null)
        {
            // ถ้ายังไม่มีชื่อในลิสต์ ให้เพิ่มเข้าไป
            if (!GameManager.Instance.sceneState.openedDoors.Contains(doorID))
            {
                GameManager.Instance.sceneState.openedDoors.Add(doorID);
            }
        }

        OpenAnimation();
    }

    void OpenInstant() 
    {
        isLocked = false;
        if(doorCollider) doorCollider.enabled = false;
        if(spriteRenderer) spriteRenderer.enabled = false; 
    }

    void OpenAnimation() 
    {
        // ใส่ Effect เปิดประตู หรือ Animation ตรงนี้
        if(doorCollider) doorCollider.enabled = false;
        if(spriteRenderer) spriteRenderer.enabled = false; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ถ้าเปิดประตูไปแล้ว ไม่ต้องโชว์ข้อความ
        if (!isLocked) return;

        if (collision.CompareTag("Player"))
        {
            if (textMeshProUGUI != null)
            {
                textMeshProUGUI.gameObject.SetActive(true);
                
                if (CheckIfPlayerHasKey())
                {
                    textMeshProUGUI.text = "[E] Unlock"; // บอกปุ่มด้วยจะดีมาก
                }
                else
                {
                    textMeshProUGUI.text = "Locked";
                }
            }
        }
    }

    // ✅ แก้ไข 2: เพิ่มฟังก์ชันปิดข้อความเมื่อเดินออก
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (textMeshProUGUI != null)
            {
                textMeshProUGUI.gameObject.SetActive(false);
            }
        }
    }
}