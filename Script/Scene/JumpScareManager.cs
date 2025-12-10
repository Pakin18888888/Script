using UnityEngine;

public class JumpScareManager : MonoBehaviour
{
    public string scareID;
    public string jumpScareID = "Scare_A_Mirror"; 

    private bool hasPlayed = false;

    void Awake()
    {
        // 1. ตรวจสอบสถานะเมื่อโหลดฉาก
        if (GameManager.Instance != null && GameManager.Instance.sceneState.playedJumpScare.Contains(jumpScareID))
        {
            hasPlayed = true;
            // ปิดการทำงานของ Trigger นี้ทันที
            // เช่น: GetComponent<Collider>().enabled = false;
            // หรือ: gameObject.SetActive(false); 
        }
    }

    void Start()
    {
        if (GameManager.Instance.sceneState.playedJumpScare.Contains(scareID))
        {
            DisableScare();
        }
    }

    public void PlayScare()
    {
        DoJumpScare();
        GameManager.Instance.sceneState.playedJumpScare.Add(scareID);
    }

    void DoJumpScare()
    {
        // ใส่อนิเมชันหรือเสียงของ jumpscare
        Debug.Log("PLAY JUMPSCARE: " + scareID);
    }

    void DisableScare()
    {
        // ปิดตัว trigger ไม่ให้เล่นอีก
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed) return; // ไม่เล่นซ้ำถ้าเคยเล่นแล้ว

        if (other.CompareTag("Player"))
        {
            DoJumpScare(); // ฟังก์ชันที่ทำ Jump Scare จริงๆ
            
            // 2. บันทึกสถานะว่าเล่นไปแล้ว
            if (GameManager.Instance != null)
            {
                GameManager.Instance.sceneState.playedJumpScare.Add(jumpScareID);
            }
            
            hasPlayed = true;
            
            // ปิดการทำงานของ Trigger
            GetComponent<Collider>().enabled = false;
            // หรือ: Destroy(gameObject);
        }
    }

    // void DoJumpScare()
    // {
    //     // ... (โค้ดเล่นเสียง/ภาพผีหลอก/ลดค่า Sanity) ...
    //     // sanitySystem.Instance.TakeSanityDamage(sanitySystem.Instance.jumpScareDamage); 
    // }
}