using System.Collections;
using UnityEngine;

public class GhostAI : MonoBehaviour
{
    [Header("Detect & Move")]
    public Transform playerTransform;
    public float detectRange = 20f;
    public float speed = 3f;
    public float disappearRange = 30f;
    public float jumpScareDistance = 0.8f;

    [Header("Fade Settings")]
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 2f;

    [Header("Hide Logic (Searching)")]
    public float searchDuration = 5.0f;     // ผีจะเดินหานานแค่ไหนก่อนยอมแพ้
    public float wanderRadius = 3.0f;       // รัศมีที่จะเดินวนเวียนรอบตู้

    [Header("Jumpscare")]
    public GameObject jumpScareUI;
    public AudioSource jumpScareSound;

    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;

    bool isVisible = false;
    bool isFadingIn = false;
    bool isFadingOut = false;
    bool isJumpScaring = false;

    float searchTimer = 0f;
    Vector2 wanderTarget; // เป้าหมายที่จะเดินไปตอนหาตัว

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (playerTransform == null)
        {
            if (Player.Instance != null)
                playerTransform = Player.Instance.transform;
            else
                playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        SetAlpha(0f);
        // กำหนดจุดเริ่มต้นให้เป็นที่เดียวกับตัวเองก่อน
        wanderTarget = transform.position; 
    }

    void FixedUpdate()
    {
        if (playerTransform == null || isJumpScaring)
            return;

        Vector2 ghostPos = rb.position;
        Vector2 playerPos = playerTransform.position;

        // ================= PLAYER HIDING (โหมดเดินหา) =================
        if (Player.Instance != null && Player.Instance.isHiding)
        {
            // ถ้ากำลังจางหายอยู่ ให้หยุดเดินแล้วรอหายไป
            if (isFadingOut) 
            {
                rb.velocity = Vector2.zero;
                return;
            }

            // เริ่มนับเวลาการหา
            searchTimer += Time.fixedDeltaTime;

            // 1. ถ้ายังหาไม่ครบเวลา -> ให้เดินวนเวียน (Wander)
            if (searchTimer < searchDuration)
            {
                WanderAroundPosition(playerPos);
            }
            // 2. ถ้าหมดเวลาแล้ว -> เริ่มจางหาย (Give Up)
            else
            {
                rb.velocity = Vector2.zero; // หยุดเดินตอนจะหายตัว
                UpdateAnimation(Vector2.zero);
                StartCoroutine(FadeOutAndDisappear());
            }

            return; // จบงานของเฟรมนี้ ไม่ต้องไปทำ Chase Logic
        }
        else
        {
            // ถ้าผู้เล่นออกมาแล้ว ให้รีเซ็ตเวลาหา
            searchTimer = 0f;
            
            // ถ้ากำลังจางหายอยู่แต่ออกมาเจอพอดี ให้กลับมาไล่ล่าต่อ
            if (isFadingOut) 
            {
                StopAllCoroutines();
                isFadingOut = false;
                isVisible = true; 
                SetAlpha(1f);
            }
        }

        // ================= NORMAL CHASE LOGIC (โหมดไล่ล่าปกติ) =================
        float dist = Vector2.Distance(ghostPos, playerPos);

        if (dist <= detectRange)
        {
            if (!isVisible && !isFadingIn)
                StartCoroutine(FadeInGhost());

            // เดินเข้าหาผู้เล่นตรงๆ
            MoveToTarget(playerPos);
        }
        else
        {
            if (isVisible)
            {
                SetAlpha(0f);
                isVisible = false;
            }

            if (dist >= disappearRange)
                Destroy(gameObject);
        }

        // ================= JUMPSCARE =================
        if (isVisible && dist <= jumpScareDistance && !isJumpScaring)
        {
            StartCoroutine(JumpAttack());
        }
    }

    // 🔥 ฟังก์ชันใหม่: เดินวนเวียนสุ่มจุดรอบๆ เป้าหมาย
    void WanderAroundPosition(Vector2 centerPos)
    {
        // ถ้าเดินถึงจุดหมายเก่าแล้ว (หรือใกล้มาก) ให้สุ่มจุดใหม่
        if (Vector2.Distance(rb.position, wanderTarget) < 0.2f)
        {
            // สุ่มจุดใหม่ในวงกลมรอบๆ ตู้ (Random Point)
            Vector2 randomPoint = Random.insideUnitCircle * wanderRadius;
            wanderTarget = centerPos + randomPoint;
        }

        // เดินไปหาจุดนั้น
        MoveToTarget(wanderTarget);
    }

    // ฟังก์ชันเดินและอัปเดตอนิเมชั่น
    void MoveToTarget(Vector2 target)
    {
        Vector2 currentPos = rb.position;
        Vector2 direction = target - currentPos;
        
        // ขยับตัว
        Vector2 newPos = Vector2.MoveTowards(currentPos, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        // อัปเดตอนิเมชั่น
        UpdateAnimation(direction);
    }

    // ... (ส่วน FadeIn, FadeOut, JumpAttack, SetAlpha เหมือนเดิม ไม่ต้องแก้) ...
    
    IEnumerator FadeInGhost()
    {
        isFadingIn = true;
        float t = 0f;
        while (t < fadeInDuration)
        {
            if (Player.Instance != null && Player.Instance.isHiding)
            {
                isFadingIn = false;
                yield break;
            }
            t += Time.deltaTime;
            SetAlpha(Mathf.Lerp(0f, 1f, t / fadeInDuration));
            yield return null;
        }
        SetAlpha(1f);
        isVisible = true;
        isFadingIn = false;
    }

    IEnumerator FadeOutAndDisappear()
    {
        isFadingOut = true;
        float t = 0f;
        float startAlpha = spriteRenderer.color.a;

        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            SetAlpha(Mathf.Lerp(startAlpha, 0f, t / fadeOutDuration));
            yield return null;
        }
        SetAlpha(0f);
        gameObject.SetActive(false);
    }

    IEnumerator JumpAttack()
    {
        isJumpScaring = true;
        if (jumpScareUI != null) jumpScareUI.SetActive(true);
        if (jumpScareSound != null) jumpScareSound.Play();
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    void UpdateAnimation(Vector2 dir)
    {
        if (animator == null) return;
        bool move = dir.magnitude > 0.01f;
        animator.SetBool("IsWalking", move);
        if (move)
        {
            animator.SetFloat("InputX", dir.x);
            animator.SetFloat("InputY", dir.y);
        }
    }

    void SetAlpha(float a)
    {
        if (spriteRenderer == null) return;
        Color c = spriteRenderer.color;
        c.a = a;
        spriteRenderer.color = c;
    }
}