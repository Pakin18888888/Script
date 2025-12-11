using UnityEngine;

public class TriggerDoor : MonoBehaviour
{
    public string doorID; 
    public bool isLocked = true;

    private Collider2D doorCollider; 
    private SpriteRenderer spriteRenderer;

    [Header("UI")]
    public TMPro.TextMeshProUGUI textMeshProUGUI;

    [Header("Sound")]
    public AudioSource doorSound;  // ← ใส่เสียงประตูที่นี่

    void Awake()
    {
        doorCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        textMeshProUGUI.gameObject.SetActive(false);

        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.sceneState.openedDoors.Contains(doorID))
                OpenInstant();
        }
    }

    public void UnlockAndOpen()
    {
        if (!isLocked) return;

        if (CheckIfPlayerHasKey())
        {
            Debug.Log("ไขกุญแจสำเร็จ: " + doorID);
            isLocked = false;
            OpenDoor();
        }
        else
        {
            Debug.Log("ไม่มีกุญแจสำหรับประตู: " + doorID);
        }
    }

    bool CheckIfPlayerHasKey()
    {
        if (Inventory.Instance == null) return false;
        
        return Inventory.Instance.HasItem(doorID);
    }

    public void OpenDoor()
    {
        // เล่นเสียงประตู 🔊
        if (doorSound != null)
            doorSound.Play();

        // บันทึกสถานะเปิดประตู
        if (GameManager.Instance != null)
        {
            if (!GameManager.Instance.sceneState.openedDoors.Contains(doorID))
                GameManager.Instance.sceneState.openedDoors.Add(doorID);
        }

        OpenAnimation();
    }

    void OpenInstant()
    {
        isLocked = false;
        if (doorCollider) doorCollider.enabled = false;
        if (spriteRenderer) spriteRenderer.enabled = false;
    }

    void OpenAnimation()
    {
        if (doorCollider) doorCollider.enabled = false;
        if (spriteRenderer) spriteRenderer.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isLocked) return;

        if (collision.CompareTag("Player"))
        {
            textMeshProUGUI.gameObject.SetActive(true);

            textMeshProUGUI.text = CheckIfPlayerHasKey() ? "[E] Unlock" : "Locked";
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            textMeshProUGUI.gameObject.SetActive(false);
        }
    }
}