using UnityEngine;

public class InventoryOpen : MonoBehaviour
{
    public GameObject inventoryUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // 1. สลับสถานะ เปิด/ปิด UI (ถ้าเปิดอยู่ก็ปิด ถ้าปิดอยู่ก็เปิด)
            inventoryUI.SetActive(!inventoryUI.activeSelf);

            // 2. ตรวจสอบว่าหลังจากสลับแล้ว ตอนนี้ UI เปิดอยู่ไหม
            if (inventoryUI.activeSelf)
            {
                // ถ้า UI เปิดอยู่ -> หยุดเวลา
                Time.timeScale = 0f; 
                FindObjectOfType<InventoryUI>().RefreshUI();
            }
            else
            {
                // ถ้า UI ปิดไปแล้ว -> ให้เวลาเดินต่อ
                Time.timeScale = 1f; 
            }
        }
    }
}
