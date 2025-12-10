using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class sanitySystem : MonoBehaviour
{
    public static sanitySystem Instance;

    [Header("Scene Control")]
    public string safeSceneName = "StartScene";  // ✅ ชื่อ Scene ที่ Sanity ไม่ลด
    bool allowDrain = false;


    [Header("Sanity Settings")]
    public float maxSanity = 100f;
    public float currentSanity = 100f;

    [Header("Drain Settings")]
    public float passiveDrain = 1f;      // ลดอัตโนมัติ ต่อวินาที
    public float jumpScareDamage = 20f;  // ลดตอนโดนหลอก

    [Header("Movement Effect")]
    public float slowThreshold = 30f;    // ต่ำกว่านี้ เดินช้าลง
    public float slowMultiplier = 0.5f;  // ความเร็วลดกี่ %

    [Header("UI")]
    public Slider sanityBar;

    Player player;
    float originalSpeed;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        player = Player.Instance;

        if (player != null)
            originalSpeed = player.moveSpeed;

        currentSanity = maxSanity;

        FindSanityBarInScene();
    }

    void Update()
    {
        DrainSanityOverTime();
        UpdateUI();
        ApplySanityEffectToPlayer();
    }

    // ==============================
    // ลดค่าสติอัตโนมัติ
    // ==============================
    void DrainSanityOverTime()
    {
        if (!allowDrain) return;
        currentSanity -= passiveDrain * Time.deltaTime;
        currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);
    }

    // ==============================
    // ฟังก์ชันเรียกจาก JumpScare / Enemy
    // ==============================
    public void TakeSanityDamage(float amount)
    {
        currentSanity -= amount;
        currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);
    }

    // ==============================
    // ส่งผลต่อ Player
    // ==============================
    void ApplySanityEffectToPlayer()
    {
        if (player == null) return;

        if (currentSanity <= slowThreshold)
        {
            player.moveSpeed = originalSpeed * slowMultiplier;
        }
        else
        {
            player.moveSpeed = originalSpeed;
        }
    }

    // ==============================
    // อัปเดตหลอด UI
    // ==============================
    void UpdateUI()
    {
        if (sanityBar != null)
            sanityBar.value = currentSanity / maxSanity;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // เช็คว่าอยู่ใน scene ปลอดภัยหรือไม่
        allowDrain = scene.name != safeSceneName;

        // ✅ หา Slider ใหม่ เมื่อเปลี่ยน Scene
        FindSanityBarInScene();
    }
    void FindSanityBarInScene()
    {
        if (sanityBar != null) return;

        Slider found = FindObjectOfType<Slider>();
        if (found != null)
            sanityBar = found;
    }




}
