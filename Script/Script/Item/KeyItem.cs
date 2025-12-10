using UnityEngine;

public class KeyItem : MonoBehaviour, IInteractable // <-- สืบทอดจาก Interface
{
    [Header("ID กุญแจ (ต้องตรงกับประตู)")]
    public string keyID; // เช่นพิมพ์ว่า "Room101" ใน Inspector

    public void OnInteract()
    {
        // เรียกใช้ Inventory ผ่าน Instance (เพราะเราทำเป็น static ไว้แล้ว)
        if (Inventory.Instance != null)
        {
            // 1. เพิ่ม ID กุญแจเข้ากระเป๋า
            Inventory.Instance.AddKey(keyID);
            
            // 2. (Optional) เล่นเสียงเก็บของ
            Debug.Log("เก็บกุญแจ " + keyID + " เรียบร้อย!");

            // 3. ทำลายวัตถุกุญแจออกจากฉาก
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("หา Inventory ไม่เจอ! (ลืมวาง Inventory ในฉากหรือเปล่า?)");
        }
    }
    public string GetDescription()
    {
        return "เก็บกุญแจห้อง " + keyID;
    }
}