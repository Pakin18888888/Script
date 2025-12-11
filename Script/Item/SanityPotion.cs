using UnityEngine;

public class SanityPotion : MonoBehaviour, IInteractable
{
    [Header("Sanity Heal")]
    public float healAmount = 5f;

    [Header("Pickup Settings")]
    public bool destroyOnUse = true;
    public Sprite icon;

    [Header("Sound")]
    public AudioClip pickUpSound;
    public AudioSource audioSource;

    public void OnInteract()
{
    if (sanitySystem.Instance != null)
        sanitySystem.Instance.TakeSanityDamage(-healAmount);

    PlayPickupSoundDetached();

    if (destroyOnUse)
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
        return $"ยาเพิ่มสติ +{healAmount}";
    }
}
