using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityPotion : MonoBehaviour , IInteractable
{
    [Header("Sanity Heal")]
    public float healAmount = 5f;   // ✅ ปรับได้ใน Inspector

    [Header("Pickup Settings")]
    public bool destroyOnUse = true;

    public void OnInteract()
    {
        Debug.Log("ใช้ยาเพิ่มสติ!");

        // ✅ เพิ่มค่า Sanity
        if (sanitySystem.Instance != null)
        {
            sanitySystem.Instance.TakeSanityDamage(-healAmount); // ลบด้วยค่าลบ = ฟื้น
        }

        // ✅ ทำลาย item
        if (destroyOnUse)
            Destroy(gameObject);
    }

    public string GetDescription()
    {
        return $"ยาเพิ่มสติ +{healAmount}";
    }
}
