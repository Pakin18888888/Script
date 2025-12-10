using UnityEngine;

public class JumpScareManager : MonoBehaviour
{
    public string scareID;

    void Start()
    {
        if (GameManager.Instance.sceneState.playedJumpScare.Contains(scareID))
        {
            DisableScare();
        }
    }

    public void PlayScare()
    {
        DoJumpScare();
        GameManager.Instance.sceneState.playedJumpScare.Add(scareID);
    }

    void DoJumpScare()
    {
        // ใส่อนิเมชันหรือเสียงของ jumpscare
        Debug.Log("PLAY JUMPSCARE: " + scareID);
    }

    void DisableScare()
    {
        // ปิดตัว trigger ไม่ให้เล่นอีก
        gameObject.SetActive(false);
    }
}