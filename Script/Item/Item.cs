using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using static UnityEditor.Progress;

public class Item : MonoBehaviour
{
    // Start is called before the first frame update

    public int ItemID;
    public string ItemName;
    public string ItemType;
    public string ItemDescription;

    public Item(int itemID, string itemName, string itemType, string itemDescription)
    {
        ItemID = itemID;
        ItemName = itemName;
        ItemType = itemType;
        ItemDescription = itemDescription;
    }

   public virtual void Use(Player player)
    {
        Debug.Log($"{ItemName} Used!");

        switch (ItemType)
        {
            case "Use":
                Debug.Log($"Using item: {ItemName}");
                break;

            case "Key":
                Debug.Log("This is a key item — cannot use directly.");
                break;

            case "Equip":
                Equip(player);
                break;

            default:
                Debug.Log("Unknown item type: " + ItemType);
                break;
        }
    }

    // ใส่อุปกรณ์
    public virtual void Equip(Player player)
    {
        Debug.Log($"{ItemName} Equipped on player.");
    }

    // รวมไอเท็ม
    public virtual Item Combine(Item otherItem)
    {
        Debug.Log($"Trying to combine {ItemName} with {otherItem.ItemName}");

        // ตัวอย่าง: รวม itemID 1 + itemID 2 = Key ใหม่
        if (ItemID == 1 && otherItem.ItemID == 2)
        {
            Debug.Log("Combination success → Master Key Created!");
            return new Item(100, "Master Key", "Key", "A fully assembled key.");
        }

        Debug.Log("Cannot combine these items.");
        return null;
    }
}

