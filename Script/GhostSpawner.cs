using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    public GameObject ghostPrefab;
    public float spawnDistance = 5f; // ระยะที่ผีเกิดจากผู้เล่น

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SpawnGhost();
        }
    }

    void SpawnGhost()
    {
        // สุ่มตำแหน่งรอบๆ ผู้เล่น
        Vector3 randomDir = Random.insideUnitSphere.normalized;
        randomDir.y = 0; // ถ้าเป็น 3D ป้องกันผีเกิดลอย

        Vector3 spawnPos = transform.position + randomDir * spawnDistance;

        Instantiate(ghostPrefab, spawnPos, Quaternion.identity);
    }
}