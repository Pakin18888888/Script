using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySanityDrain : MonoBehaviour
{
    public float sanityDamagePerSecond = 10f;
    public float detectRange = 2f;

    Transform player;

    void Start()
    {
        if (Player.Instance != null)
            player = Player.Instance.transform;
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= detectRange)
        {
            sanitySystem.Instance?.TakeSanityDamage(sanityDamagePerSecond * Time.deltaTime);
        }
    }
}
