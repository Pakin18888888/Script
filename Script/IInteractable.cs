using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable 
{
    void OnInteract(); 
    
    // (Option) ชื่อที่จะขึ้นโชว์ตอนเอาเมาส์ไปชี้ เช่น "เก็บกุญแจ", "เปิดไฟ"
    string GetDescription();
}
