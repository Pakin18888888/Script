using UnityEngine;

public class Door : MonoBehaviour
{
    public string sceneToLoad;
    public string spawnPointID;
    
    // เอาไว้เช็คว่าประตูนี้ต้องไขกุญแจก่อนไหม (ถ้าไม่ต้องไข ก็ปล่อยว่างไว้)
    public string requiredDoorIDToCheck; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // ถ้ามีการกำหนด ID ประตูที่ต้องเช็ค
            if (!string.IsNullOrEmpty(requiredDoorIDToCheck))
            {
                // เช็คใน GameManager ว่าประตูนี้เปิดหรือยัง
                if (!GameManager.Instance.sceneState.openedDoors.Contains(requiredDoorIDToCheck))
                {
                    Debug.Log("ประตูล็อคอยู่! เข้าไม่ได้");
                    return; // จบการทำงาน ไม่วาร์ป
                }
            }

            // ถ้าผ่านเงื่อนไข ก็สั่งวาร์ปผ่าน GameManager ตัวเดียวจบ
            GameManager.Instance.ChangeScene(sceneToLoad, spawnPointID);
        }
    }
}