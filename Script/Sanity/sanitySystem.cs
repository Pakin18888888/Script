using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class sanitySystem : MonoBehaviour
{
    public static sanitySystem Instance;
    bool isGameOver = false;

    [Header("Scene Control")]
    public string safeSceneName = "StartScene";
    bool allowDrain = false;

    [Header("Sanity Settings")]
    public float maxSanity = 100f;
    public float currentSanity = 100f;

    [Header("Drain Settings")]
    public float passiveDrain = 1f;
    public float jumpScareDamage = 20f;

    [Header("Movement Effect")]
    public float slowThreshold = 30f;
    public float slowMultiplier = 0.5f;

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
        RefreshPlayerReference();
        currentSanity = maxSanity;
        FindSanityBarInScene();
    }

    void Update()
    {
        if (isGameOver) return;

        DrainSanityOverTime();
        UpdateUI();
        ApplySanityEffectToPlayer();

        if (currentSanity <= 0)
        {
            TriggerGameOver();
        }
    }

    // ============================== GAME OVER ==============================
    void TriggerGameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        currentSanity = 0;
        UpdateUI();

        Debug.Log("❌ GAME OVER TRIGGERED");

        // 🔎 หาตัว GameOverUI ใหม่ทุกครั้ง
        if (GameOverUI.Instance != null)
        {
            GameOverUI.Instance.ShowGameOver();
        }
        else
        {
            Debug.LogError("❌ GameOverUI Instance = NULL — ไม่พบ UI ใน Scene นี้");
        }

        // 🔒 ปิดการเดิน
        if (Player.Instance != null)
            Player.Instance.canMove = false;
    }

    // ============================== SANITY LOGIC ==============================

    void DrainSanityOverTime()
    {
        if (!allowDrain) return;

        currentSanity -= passiveDrain * Time.deltaTime;
        currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);
    }

    public void TakeSanityDamage(float amount)
    {
        currentSanity -= amount;
        currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);
    }

    void ApplySanityEffectToPlayer()
    {
        if (player == null) return;

        if (currentSanity <= slowThreshold)
            player.moveSpeed = originalSpeed * slowMultiplier;
        else
            player.moveSpeed = originalSpeed;
    }

    // ============================== UI ==============================

    void UpdateUI()
    {
        if (sanityBar != null)
            sanityBar.value = currentSanity / maxSanity;
    }

    // ============================== SCENE LOAD ==============================

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
        allowDrain = scene.name != safeSceneName;

        RefreshPlayerReference();
        FindSanityBarInScene();

        if (isGameOver)
        {
            if (GameOverUI.Instance != null)
                GameOverUI.Instance.ShowGameOver();
        }
    }

    void RefreshPlayerReference()
    {
        player = Player.Instance;

        if (player != null)
            originalSpeed = player.moveSpeed;
    }

    void FindSanityBarInScene()
    {
        Slider[] sliders = FindObjectsOfType<Slider>();
        foreach (var s in sliders)
        {
            if (s.CompareTag("SanityBar"))
            {
                sanityBar = s;
                return;
            }
        }
    }
}
