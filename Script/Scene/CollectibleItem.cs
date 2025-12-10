using UnityEngine;
using UnityEngine.SceneManagement; // อาจไม่จำเป็นถ้าใช้ GameManager.Instance

public class CollectibleItem : MonoBehaviour, IInteractable
{
    // ✅ ต้องตั้งค่า ID ที่ไม่ซ้ำกันใน Editor สำหรับไอเทมแต่ละชิ้น
    public string uniqueItemID; 

    void Awake()
    {
        // 1. ตรวจสอบสถานะทันทีเมื่อโหลดฉาก
        if (GameManager.Instance != null && GameManager.Instance.sceneState.collectedItems.Contains(uniqueItemID))
        {
            // ถ้า ID นี้ถูกบันทึกว่าเก็บไปแล้ว ให้ทำลายตัวเองทิ้ง
            Destroy(gameObject);
            return;
        }
    }

    public string GetDescription()
    {
        return "เก็บ " + gameObject.name;
    }

    public void OnInteract()
    {
        // 2. เมื่อผู้เล่นเก็บไอเทม
        
        // (ส่วนนี้ต้องเรียก Inventory เพื่อเพิ่มไอเทมจริง)
        // ตัวอย่าง: Inventory.Instance.AddItem(uniqueItemID); 

        // 3. บันทึกสถานะว่าไอเทมนี้ถูกเก็บไปแล้ว
        if (!GameManager.Instance.sceneState.collectedItems.Contains(uniqueItemID))
        {
            GameManager.Instance.sceneState.collectedItems.Add(uniqueItemID);
        }

        // 4. ทำลายวัตถุนี้ในฉากปัจจุบัน
        Destroy(gameObject); 
        Debug.Log($"เก็บไอเทม: {uniqueItemID} และบันทึกสถานะแล้ว");
    }
}