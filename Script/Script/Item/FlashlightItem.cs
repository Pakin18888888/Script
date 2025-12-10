using UnityEngine;

// ต้องใส่ IInteractable ต่อท้าย เพื่อให้คลิกเมาส์แล้ว Player มองเห็น
public class FlashlightItem : MonoBehaviour, IInteractable 
{
    public string itemID = "Flashlight";

    // ฟังก์ชันนี้จะทำงานอัตโนมัติเมื่อผู้เล่นเอาเมาส์ไปคลิกที่ไฟฉาย
    public void OnInteract()
    {
        Debug.Log("คลิกเก็บไฟฉายแล้ว!");

        // 1. เพิ่มชื่อลงกระเป๋า (เพื่อให้รู้ว่าเคยเก็บแล้ว)
        if (Inventory.Instance != null)
        {
            if (!Inventory.Instance.items.Contains(itemID))
            {
                Inventory.Instance.items.Add(itemID);
            }
        }

        // 2. สั่งให้ Player เปิดไฟ/ขยายไฟ
        if (Player.Instance != null)
        {
            Player.Instance.EnableFlashlight(); 
        }

        // 3. หายตัวไป
        Destroy(gameObject);
    }
    
    // (Option) เผื่อในอนาคตทำ UI ขึ้นชื่อของ
    public string GetDescription()
    {
        return "เก็บไฟฉาย";
    }
}