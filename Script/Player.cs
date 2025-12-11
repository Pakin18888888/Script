using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{
    public static Player Instance; // Singleton ให้คนอื่นเรียกหาได้ง่ายๆ

    [Header("ตั้งค่าการเคลื่อนที่")]
    public float moveSpeed = 5f;
    public bool canMove = true;

    [Header("Footstep Sound")]
    public AudioSource footstepSource;
    
    [Header("ตั้งค่าสถานะตัวละคร")]
    public int hp = 100;
    public int flashlightBattery = 100;
    
    // อ้างอิงไปที่ Inventory (ลากใส่ใน Inspector หรือปล่อยว่างไว้ก็ได้ เดี๋ยวโค้ดหาเอง)
    public Inventory inventory; 

    [Header("ตั้งค่าการสำรวจ (Interact)")]
    public float interactionRange = 2.0f; // ระยะที่มือกดถึง
    public LayerMask interactableLayer;   // Layer ของของที่เก็บได้ (เช่น Key)

    // ตัวแปรภายใน
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    private Camera cam; // เก็บกล้องไว้ใช้ตอนคลิกเมาส์

    // 🔥 ตัวแปรระบบซ่อนตัว
    [HideInInspector] public bool isHiding = false; 
    [HideInInspector] public TriggerLocker currentLocker;

    [Header("Flashlight Settings")]
    public Light2D playerLight; // ลาก Component Light 2D ของผู้เล่นมาใส่ช่องนี้
    public float baseLightRadius = 3.0f; // 🔥 ค่าแสงปกติ (ก่อนเก็บไฟฉาย)
    public float newLightRadius = 6.0f; // ขนาดรัศมีแสงที่จะใหญ่ขึ้น (หลังจากเก็บไฟฉาย)
    void Awake()
    {
        // ตั้งค่า Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // ดึง Component อัตโนมัติ
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        cam = Camera.main; // หากล้องหลักเตรียมไว้เลย
    }

    void Start()
    {
        // ซีนที่ไม่ต้องให้ Player อยู่
    string[] noPlayerScenes = {"CutEndScene" };

    string currentScene = SceneManager.GetActiveScene().name;

    if (System.Array.Exists(noPlayerScenes, scene => scene == currentScene))
    {
        Destroy(gameObject);
        return;
    }

    // หาของในฉาก
    if (inventory == null)
        inventory = FindObjectOfType<Inventory>();
    }

    void Update()
    {
        // ป้องกัน Error กรณีมี Player ซ้อนกัน
        if (Instance != this) return;

        // 1. ระบบเคลื่อนที่ (Movement)
        if (canMove)
        {
            rb.velocity = moveInput * moveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        // อัปเดตท่าทาง Animation
        UpdateAnimation();

        // 2. ระบบกดปุ่ม E (Interact) - เปิดประตู หรือ ซ่อนตู้
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame) 
        {
            CheckForDoorAndLocker(); 
        }

        // 3. ระบบคลิกซ้ายเก็บของ (Item Collection)
        if (Input.GetMouseButtonDown(0)) 
        {
            DetectObject();
        }
    }

    // --- ฟังก์ชันเกี่ยวกับการขยับ (Movement) ---
    public void Move(InputAction.CallbackContext context)
    {
        if (!canMove)
        {
            moveInput = Vector2.zero;
            return;
        }

        if (context.performed)
            moveInput = context.ReadValue<Vector2>();
        else if (context.canceled)
            moveInput = Vector2.zero;
    }

    void UpdateAnimation()
    {
        bool isMoving = moveInput.magnitude > 0;
        
        if (animator != null)
        {
            animator.SetBool("IsWalking", isMoving);

            if (isMoving)
            {
                animator.SetFloat("InputX", moveInput.x);
                animator.SetFloat("InputY", moveInput.y);
                animator.SetFloat("LastInputX", moveInput.x);
                animator.SetFloat("LastInputY", moveInput.y);
            }
        }
        // 🔊 ระบบเสียงเท้า
        if (footstepSource != null)
        {
            if (isMoving && canMove)
            {
                if (!footstepSource.isPlaying)
                    footstepSource.Play();
            }
            else
            {
                if (footstepSource.isPlaying)
                    footstepSource.Stop();
            }
        }
    }

    public void SetMovement(bool status)
    {
        canMove = status;
        isHiding = !status; // ถ้าขยับไม่ได้ แปลว่ากำลังซ่อนอยู่ (โดยประมาณ)

        if (!status)
        {
            moveInput = Vector2.zero;
            rb.velocity = Vector2.zero;
            if (animator != null) animator.SetBool("IsWalking", false);
        }
    }

    // --- ฟังก์ชันกด E (Interact) ---
    void CheckForDoorAndLocker()
    {
        // กรณีที่ 1: ถ้ากำลังซ่อนอยู่ในตู้ ให้กด E เพื่อออกมา
        if (currentLocker != null && isHiding)
        {
            currentLocker.OnPlayerInteracting();
            return; 
        }

        // กรณีที่ 2: ถ้าไม่ได้ซ่อน ให้สแกนรอบตัวว่ามีอะไรให้กดไหม
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactionRange);

        foreach (Collider2D hit in hits)
        {
            // A. เจอตู้ซ่อนไหม?
            TriggerLocker locker = hit.GetComponent<TriggerLocker>();
            if (locker != null && !isHiding)
            {
                locker.OnPlayerInteracting(); // สั่งเข้าตู้
                currentLocker = locker;       // จำว่าอยู่ตู้ไหน
                return; // จบการทำงานทันที (ไม่กดรัว)
            }

            // B. เจอประตูไหม?
            TriggerDoor door = hit.GetComponent<TriggerDoor>();
            if (door != null)
            {
                door.UnlockAndOpen(); // สั่งไขกุญแจ
                return; // จบการทำงานทันที
            }
        }
    }

    // --- ฟังก์ชันคลิกเมาส์เก็บของ (Raycast) ---
    void DetectObject()
    {
        if (cam == null) cam = Camera.main; // กันเหนียว หากล้องใหม่อีกรอบถ้าหลุด
        if (cam == null) return;

        // แปลงจุดที่คลิกบนจอ เป็นจุดในโลกเกม
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        
        // ยิง Raycast ไปที่จุดนั้น
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f, interactableLayer);

        if (hit.collider != null)
        {
            // เช็คว่าของอยู่ใกล้ตัวไหม (ห้ามเก็บของไกลเกินไป)
            float distance = Vector2.Distance(transform.position, hit.transform.position);
            
            if (distance <= interactionRange)
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                
                if (interactable != null)
                {
                    interactable.OnInteract(); // สั่งเก็บของ
                }
            }
            else
            {
                Debug.Log("ของอยู่ไกลเกินไป เอื้อมไม่ถึง!");
            }
        }
    }

    // เอาไว้วาดวงกลมในหน้า Scene จะได้เห็นระยะ Interact
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }

    public void ChangeLightRadius(float targetRadius)
    {
        if (playerLight != null)
        {
            // ใช้ค่าที่ส่งมาเป็นรัศมีใหม่ทันที
            playerLight.pointLightOuterRadius = targetRadius;
            Debug.Log($"ปรับแสงเป็น: {targetRadius}");
        }
    }

    public void EnableFlashlight()
    {
        if (playerLight != null)
        {
            // ปรับแสงเป็นค่าที่อัปเกรดแล้ว
            ChangeLightRadius(newLightRadius);
            Debug.Log("เก็บไฟฉายแล้ว! แสงกว้างขึ้น");
        }
    }

    // 🔥 ฟังก์ชันสำหรับลดแสงกลับมาเป็นค่าเริ่มต้น
    public void ResetLightRadius()
    {
        // ใช้ค่าที่กำหนดไว้ใน baseLightRadius
        ChangeLightRadius(baseLightRadius);
    }
}