using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentUI : MonoBehaviour
{
    static PersistentUI instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);   // กัน UI ซ้อน
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);   // ✅ อยู่ทุก Scene
    }
}
