using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{

    public GameObject meteorPrefab;  // Sa�l�k objesinin prefab'�
    public float spawnInterval = 20f;     // Sa�l�k objelerinin d��me aral��� (saniye)
    public float spawnRangeX = 8f;       // Sa�l�k objelerinin sa�a ve sola ne kadar da��labilece�i (X ekseninde)

    private void Start()
    {
        InvokeRepeating("SpawnMeteorDrop", 0f, spawnInterval);  // Sa�l�k objelerini belirli aral�klarla d���r
    }

    private void SpawnMeteorDrop()
    {
        // X ekseninde rastgele bir pozisyon belirle
        float spawnPosX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector2 spawnPos = new Vector2(spawnPosX, 10.5f);

        
        Instantiate(meteorPrefab, spawnPos, Quaternion.identity);
    }
}
 
