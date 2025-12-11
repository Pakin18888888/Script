using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance; // ตัวแปรสำหรับเช็คว่ามีตัวนี้อยู่หรือยัง
    public AudioSource audioSource;      // แหล่งกำเนิดเสียง

    void Awake()
    {
        // 1. เช็คว่ามี SoundManager อยู่แล้วหรือยัง?
        if (Instance == null)
        {
            // ถ้ายังไม่มี -> ให้ตัวนี้เป็นตัวหลัก
            Instance = this;
            DontDestroyOnLoad(gameObject); // สั่งห้ามทำลายเมื่อเปลี่ยน Scene
        }
        else
        {
            // ถ้ามีอยู่แล้ว (จาก Scene ก่อนหน้า) -> ทำลายตัวนี้ทิ้งทันที
            // เพื่อให้เสียงตัวเก่าเล่นต่อเนื่องไม่สะดุด และไม่เกิดเสียงซ้อน
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // ป้องกันกันเหนียว: ถ้าเสียงยังไม่เล่น ให้สั่งเล่น
        // แต่ถ้ามันเล่นอยู่แล้ว (จาก Scene เก่า) ก็ปล่อยมันเล่นต่อไป ไม่ต้องสั่ง Play ซ้ำ
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}