using UnityEngine;

public class StartSceneMessage : MonoBehaviour
{
    void Start()
    {
        // ถ้าเคยเก็บไฟฉายแล้ว → ไม่ต้องแสดงข้อความอีก
        if (FlashlightItem.flashlightCollected)
        {
            Destroy(gameObject);  
            return;
        }

        // แสดงข้อความเฉพาะตอนยังไม่ได้เก็บไฟฉาย
        if (TutorialUI.Instance != null)
        {
            TutorialUI.Instance.ShowMessage(
                "You should look around… maybe there’s a flashlight in this room."
            );
        }
    }
}
