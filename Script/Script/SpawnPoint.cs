using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public string spawnID; 

    void OnDrawGizmos()
    {
        // วาดรูปกากบาทสีฟ้าในหน้า Scene จะได้เห็นชัดๆ ว่าจุดเกิดอยู่ตรงไหน
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}