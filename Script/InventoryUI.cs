using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public Transform slotParent;  // Panel ที่มีช่องไอเท็ม
    public GameObject slotPrefab; // Prefab ช่องไอเท็มตัวเดียว

    void OnEnable()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        foreach (Transform child in slotParent)
            Destroy(child.gameObject);

        foreach (var item in Inventory.Instance.items)
        {
            GameObject slot = Instantiate(slotPrefab, slotParent);

            slot.transform.Find("Icon").GetComponent<Image>().sprite = item.icon;
            slot.transform.Find("Amount").GetComponent<TextMeshProUGUI>().text =
                item.amount > 1 ? item.amount.ToString() : "";
        }
    }
}
