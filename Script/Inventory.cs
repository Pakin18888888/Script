using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    // เปลี่ยนจาก object เป็น List<string> เพื่อเก็บ ID กุญแจ
    public List<string> keyIDs = new List<string>(); 

    public List<string> items = new List<string>();

    void Awake()
    {
       // บรรทัดนี้สำคัญมาก! ถ้าไม่มีบรรทัดนี้ คนอื่นจะหา Inventory ไม่เจอ
    if (Instance == null)
    {
        Instance = this;
        DontDestroyOnLoad(gameObject); // (ใส่หรือไม่ใส่ก็ได้ ถ้าแปะที่ Player ที่มี DontDestroy อยู่แล้ว)
    }
    else
    {
        Destroy(gameObject);
    }
    }

    // ฟังก์ชันสำหรับเพิ่มกุญแจเข้าตัว
    public void AddKey(string keyID)
    {
        if (!keyIDs.Contains(keyID))
        {
            keyIDs.Add(keyID);
            Debug.Log($"ได้รับกุญแจ: {keyID} แล้ว!");
        }
    }

    // (Option) สำหรับระบบ Save/Load
    public void LoadFromIdList(List<string> savedKeys)
    {
        keyIDs = new List<string>(savedKeys);
    }
}