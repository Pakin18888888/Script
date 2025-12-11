using UnityEngine;

public class FlashlightItem : MonoBehaviour, IInteractable
{
    public string itemID = "flashlight";
    public Sprite icon;

    [Header("Sound")]
    public AudioClip pickUpSound;
    public AudioSource audioSource;
    public static bool flashlightCollected = false;

    void Start()
    {
        if (Inventory.Instance != null &&
            Inventory.Instance.HasItem(itemID))
        {
            Destroy(gameObject);
        }
    }

    public void OnInteract()
{
    flashlightCollected = true;

    Inventory.Instance.AddItem(itemID, icon, 1);

    if (Player.Instance != null)
        Player.Instance.EnableFlashlight();

    if (TutorialUI.Instance != null)
        TutorialUI.Instance.DestroyTutorialUI();  // ลบ UI บทสอน

    PlayPickupSoundDetached();

    Destroy(gameObject);
}


void PlayPickupSoundDetached()
{
    if (pickUpSound == null) return;
    AudioSource.PlayClipAtPoint(pickUpSound, transform.position, 1f);
}

    void PlayPickupSound()
    {
        if (audioSource != null && pickUpSound != null)
            audioSource.PlayOneShot(pickUpSound);
    }

    public string GetDescription()
    {
        return "เก็บไฟฉาย";
    }
    
}
