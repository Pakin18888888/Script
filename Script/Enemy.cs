using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Sta
    // rt is called before the first frame update
    public Transform target;
    private string EnemyID;
    public float detectionRange = 2f;
    public float moveSpeed = 3f;
    public float detectionRadius = 1f;
    private bool isFollowing = false;
    public float detectionTime = 2f;
    private float outofdetectionRangeTimer = 0f;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= detectionRadius)
        {
            isFollowing = true;
        }
        else
        {
            isFollowing = false;
            outofdetectionRangeTimer += Time.deltaTime;
        }
        
        if (isFollowing)
        {
            if (distance > detectionRange)
            {
            // ทิศทางวิ่งเข้าหา
            Vector3 direction = (target.position - transform.position).normalized;

            // ขยับตัวละครไปตามทิศทาง
            transform.position += direction * moveSpeed * Time.deltaTime;
            }
        }

        if (outofdetectionRangeTimer >= detectionTime)
        {
            Destroy(gameObject);
        }
    }
}
