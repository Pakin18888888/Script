using UnityEngine;

public class KeyItem : MonoBehaviour, IInteractable
{
    [Header("ID กุญแจ เช่น key_blue")]
    public string keyID;
    public Sprite icon;

    [Header("Sound")]
    public AudioClip pickUpSound;
    public AudioSource audioSource;

    void Start()
    {
        if (Inventory.Instance != null &&
            Inventory.Instance.HasItem(keyID))
        {
            Destroy(gameObject);
        }
    }

    public void OnInteract()
{
    Inventory.Instance.AddItem(keyID, icon, 1);

    PlayPickupSoundDetached();

    Destroy(gameObject); // ลบทันที
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
        return "เก็บกุญแจ " + keyID;
    }
}
