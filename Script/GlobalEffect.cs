using UnityEngine;

public class GlobalEffect : MonoBehaviour
{
    public static GlobalEffect Instance;

    [Header("การตั้งค่า")]
    public Vector3 offset = new Vector3(0, 0, 10); // ระยะห่างจากกล้อง (Z=10 มักจะอยู่หน้ากล้องพอดี)

    void Awake()
    {
        // 1. ระบบ Singleton (มีได้แค่ตัวเดียว)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ห้ามทำลายเมื่อเปลี่ยน Scene
        }
        else
        {
            Destroy(gameObject); // ถ้ามีตัวซ้ำ ให้ลบตัวใหม่ทิ้ง
        }
    }

    void LateUpdate()
    {
        // 2. ให้ Particle วิ่งตามกล้องหลักเสมอ
        if (Camera.main != null)
        {
            // ย้ายตำแหน่ง Particle ไปที่ตำแหน่งกล้อง + ระยะห่างที่ตั้งไว้
            transform.position = Camera.main.transform.position + offset;
        }
    }
}