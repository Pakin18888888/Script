using UnityEngine;

public class UISideLoop : MonoBehaviour
{
    public float moveAmount = 50f;
    public float speed = 2f;

    private RectTransform rect;
    private Vector2 startPos;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        startPos = rect.anchoredPosition;
    }

    void Update()
    {
        float x = Mathf.Cos(Time.time * speed) * moveAmount;
        rect.anchoredPosition = new Vector2(startPos.x + x, startPos.y);
    }
}
