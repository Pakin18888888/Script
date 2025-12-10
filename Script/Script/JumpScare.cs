using System.Collections;
using System.Drawing;
using UnityEngine;

public class JumpScare : MonoBehaviour
{
    public GameObject jumpScareUI;
    public AudioSource jumpScareSound;
    public Camera mainCam;
    public float shakeAmount = 0.2f;
    public float shakeDuration = 0.3f;
    public float displayTime = 1f;

    bool hasPlayed = false;

    public void Play()
    {
        if (hasPlayed) return;   // ✅ ป้องกันเล่นซ้ำ
        hasPlayed = true;

        StartCoroutine(PlayRoutine());
    }

    IEnumerator PlayRoutine()
    {
        if (jumpScareUI != null) jumpScareUI.gameObject.SetActive(true);
        if (jumpScareSound != null) jumpScareSound.Play();

        Vector3 origin = mainCam.transform.localPosition;
        float t = 0f;

        while (t < shakeDuration)
        {
            mainCam.transform.localPosition = origin + (Vector3)Random.insideUnitCircle * shakeAmount;
            t += Time.deltaTime;
            yield return null;
        }

        mainCam.transform.localPosition = origin;

        yield return new WaitForSeconds(displayTime);

        if (jumpScareUI != null) jumpScareUI.gameObject.SetActive(false);
    }
}
