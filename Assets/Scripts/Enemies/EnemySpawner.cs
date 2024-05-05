using Unity.Netcode;
using UnityEngine;

namespace Enemies
{
    public class EnemySpawner : NetworkBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private float enemySpawnRadius = 30f;
        [SerializeField]
        private NetworkVariable<float> enemySpawnTime =
            new NetworkVariable<float>(5f, NetworkVariableReadPermission.Everyone);
    
   
        private float _currentEnemySpawnTime = 0f;

    
        // Update is called once per frame
        private void Update()
        {
            // Logic only executes on server
            if (!IsServer) return;
            UpdateEnemySpawning();
        }

        private void UpdateEnemySpawning()
        {
            _currentEnemySpawnTime += Time.deltaTime;

            if (!(_currentEnemySpawnTime >= enemySpawnTime.Value)) return;
            
            Vector3 newEnemyPos = GetRandomEnemySpawnPosition();
            _SpawnEnemy(enemyPrefab, newEnemyPos); // No futuro o prefab pode ser variado
            // Debug.Log("Spawning enemy with position: " + newEnemyPos.ToString());
            _currentEnemySpawnTime = 0f;
        }

        private Vector3 GetRandomEnemySpawnPosition()
        {
            return  new Vector3(
                Random.Range(-enemySpawnRadius, enemySpawnRadius),
                1f,
                Random.Range(-enemySpawnRadius, enemySpawnRadius)
            );
        }

        private static void _SpawnEnemy(GameObject enemyPrefabToSpawn, Vector3 spawnPosition)
        {
            GameObject instance = Instantiate(enemyPrefabToSpawn, spawnPosition, Quaternion.identity);
            NetworkObject instanceNetworkObject = instance.GetComponent<NetworkObject>();
            instanceNetworkObject.Spawn();
        }
    }
}
