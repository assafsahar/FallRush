using Core.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.Obstacles
{
    public class ObstacleSpawner : MonoBehaviour
    {
        [Header("Prefabs List")]
        [SerializeField] private List<GameObject> obstaclePrefabs;
        [SerializeField] private float obstacleYOffsetFromCamera = 15f;


        [Header("Pooling Settings")]
        [SerializeField] private int poolCapacity = 10;
        [SerializeField] private int poolMaxSize = 30;

        [Header("Spawn Settings")]
        [SerializeField] private Transform player; // Assign the ragdoll's main body here
        [SerializeField] private float spawnDistance = 10f;
        [SerializeField] private float spawnRangeX = 2.5f;
        [SerializeField] private int initialSpawnCount = 10;

        private float nextSpawnY;
        private List<ObjectPool<GameObject>> pools;

        private void Awake()
        {
            InitializePools();
            InitializeObstacles();
        }

        private void InitializeObstacles()
        {
            // Initial spawn of obstacles below the player
            nextSpawnY = player.position.y - spawnDistance;
            for (int i = 0; i < initialSpawnCount; i++)
                SpawnRandomObstacle();
        }

        private void InitializePools()
        {
            // Create a pool for each obstacle prefab
            pools = new List<ObjectPool<GameObject>>();
            foreach (var prefab in obstaclePrefabs)
            {
                pools.Add(new ObjectPool<GameObject>(
                    () => Instantiate(prefab),
                    obj => obj.SetActive(true),
                    obj => obj.SetActive(false),
                    obj => Destroy(obj),
                    true, poolCapacity, poolMaxSize
                ));
            }
        }

        private void Update()
        {
            TrySpawnObstaclesIfNeeded();
        }

        private void TrySpawnObstaclesIfNeeded()
        {
            while (player.position.y - nextSpawnY < spawnDistance * initialSpawnCount)
                SpawnRandomObstacle();
        }


        private void SpawnRandomObstacle()
        {
            int prefabIndex = Random.Range(0, pools.Count);
            var pool = pools[prefabIndex];
            GameObject obj = pool.Get();
            SetObstaclePosition(obj);
            SetupReturnToPool(pool, obj);
            SetupObstacleRotation(obj);

            nextSpawnY -= spawnDistance;
        }

        private static void SetupObstacleRotation(GameObject obj)
        {
            // Adding spin
            var rotator = obj.GetComponent<ObstacleRotator>();
            if (rotator == null)
                rotator = obj.AddComponent<ObstacleRotator>();
            float randomSpeed = Random.Range(-180f, 180f);
            rotator.SetRotationSpeed(randomSpeed);
        }

        private void SetupReturnToPool(ObjectPool<GameObject> pool, GameObject obj)
        {
            // Attach the ReturnToPool component if missing, and set pool reference
            var returnToPool = obj.GetComponent<ReturnToPool>();
            if (returnToPool == null)
                returnToPool = obj.AddComponent<ReturnToPool>();
            returnToPool.SetPool(pool);
            returnToPool.SetYOffsetFromCamera(obstacleYOffsetFromCamera);
        }

        private void SetObstaclePosition(GameObject obj)
        {
            // Set random X, fixed Y for the obstacle
            float x = player.position.x + Random.Range(-spawnRangeX, spawnRangeX);
            obj.transform.position = new Vector3(x, nextSpawnY, 0f);
        }
    }
}
