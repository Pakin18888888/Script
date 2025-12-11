using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    [System.Serializable]
    public class Item
    {
        public string id;
        public Sprite icon;
        public int amount = 1;
    }

    public List<Item> items = new List<Item>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    // เพิ่มไอเท็มแบบทั่วไป
    public void AddItem(string id, Sprite icon, int amount = 1)
    {
        Item existing = items.Find(i => i.id == id);

        if (existing != null)
        {
            existing.amount += amount;
        }
        else
        {
            Item newItem = new Item();
            newItem.id = id;
            newItem.icon = icon;
            newItem.amount = amount;
            items.Add(newItem);
        }
    }

    public bool HasItem(string id)
    {
        return items.Exists(i => i.id == id);
    }

    public void RemoveItem(string id, int amount = 1)
    {
        Item existing = items.Find(i => i.id == id);
        if (existing == null) return;

        existing.amount -= amount;
        if (existing.amount <= 0)
            items.Remove(existing);
    }
}
