using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    
    [SerializeField]
    private NetworkVariable<float> enemySpawnTime =
        new NetworkVariable<float>(5f, NetworkVariableReadPermission.Everyone);

    [SerializeField] private float enemySpawnRadius = 30f;
    
    private float currentEnemySpawnTime = 0f;
    
    
    // Update is called once per frame
    void Update()
    {
        // Logic only executes on server
        if (!IsServer) return;
        UpdateEnemySpawning();
    }

    void UpdateEnemySpawning()
    {
        currentEnemySpawnTime += Time.deltaTime;

        if (currentEnemySpawnTime >= enemySpawnTime.Value)
        {
            Vector3 newEnemyPos = GetRandomEnemySpawnPosition();
            SpawnEnemy(enemyPrefab, newEnemyPos); // No futuro o prefab pode ser variado
            Debug.Log("Spawning enemy with position: " + newEnemyPos.ToString());
            currentEnemySpawnTime = 0f;
        }
    }

    Vector3 GetRandomEnemySpawnPosition()
    {
        return  new Vector3(
            Random.Range(-enemySpawnRadius, enemySpawnRadius),
            1f,
            Random.Range(-enemySpawnRadius, enemySpawnRadius)
        );
    }

    void SpawnEnemy(GameObject enemyPrefabToSpawn, Vector3 spawnPosition)
    {
        var instance = Instantiate(enemyPrefabToSpawn, spawnPosition, Quaternion.identity);
        var instanceNetworkObject = instance.GetComponent<NetworkObject>();
        instanceNetworkObject.Spawn();
    }
}
