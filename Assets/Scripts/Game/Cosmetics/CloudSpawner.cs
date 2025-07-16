using Core.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.Cosmetics
{
    public class CloudSpawner : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject smallCloudPrefab;
        [SerializeField] private GameObject bigCloudPrefab;

        [Header("Sprites")]
        [SerializeField] private Sprite[] smallCloudSprites;
        [SerializeField] private Sprite[] bigCloudSprites;

        [Header("Settings")]
        [SerializeField] private float spawnInterval = 1.5f;
        [SerializeField] private float cloudYOffset = 1f;
        [SerializeField] private float leftSpawnMargin = -3f;
        [SerializeField] private float rightSpawnMargin = 3f;

        [Header("Movement")]
        [SerializeField] private float smallCloudSpeed = 0.5f;
        [SerializeField] private float bigCloudSpeed = 1.5f;

        [Header("Sorting Layers")]
        [SerializeField] private string smallCloudSortingLayer = "BackgroundClouds";
        [SerializeField] private string bigCloudSortingLayer = "ForegroundClouds";

        private ObjectPool<GameObject> smallCloudPool;
        private ObjectPool<GameObject> bigCloudPool;

        private float timer;

        private void Start()
        {
            smallCloudPool = CreatePool(smallCloudPrefab);
            bigCloudPool = CreatePool(bigCloudPrefab);
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                timer = 0f;
                SpawnRandomCloud();
            }
        }

        private ObjectPool<GameObject> CreatePool(GameObject prefab)
        {
            return new ObjectPool<GameObject>(
                () => Instantiate(prefab),
                obj => obj.SetActive(true),
                obj => obj.SetActive(false),
                obj => Destroy(obj),
                true, 10, 30
            );
        }

        private void SpawnRandomCloud()
        {
            bool spawnSmall = Random.value < 0.5f;

            var pool = spawnSmall ? smallCloudPool : bigCloudPool;
            GameObject cloud = pool.Get();

            SetCloudPosition(cloud);
            SetupCloud(cloud, spawnSmall, pool);
        }

        private void SetCloudPosition(GameObject cloud)
        {
            Camera cam = Camera.main;
            if (cam == null) return;

            float screenHalfHeight = cam.orthographicSize;
            float screenHalfWidth = screenHalfHeight * cam.aspect;

            float leftEdge = cam.transform.position.x - screenHalfWidth;
            float rightEdge = cam.transform.position.x + screenHalfWidth;
            float bottomEdge = cam.transform.position.y - screenHalfHeight;

            float spriteWidth = 0f;
            var sr = cloud.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite != null)
            {
                spriteWidth = sr.sprite.bounds.size.x * cloud.transform.localScale.x;
            }

            float x = Random.Range(leftEdge, rightEdge - spriteWidth);
            float y = bottomEdge;

            cloud.transform.position = new Vector3(x, y, 0f);
        }



        private void SetupCloud(GameObject cloud, bool isSmall, ObjectPool<GameObject> pool)
        {
            var mover = cloud.GetComponent<CloudMover>();
            if (mover == null)
                mover = cloud.AddComponent<CloudMover>();

            mover.SetSpeed(isSmall ? smallCloudSpeed : bigCloudSpeed);
            mover.SetPool(pool);

            var sr = cloud.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sortingLayerName = isSmall ? smallCloudSortingLayer : bigCloudSortingLayer;

                // Pick a random sprite
                Sprite[] sprites = isSmall ? smallCloudSprites : bigCloudSprites;
                if (sprites != null && sprites.Length > 0)
                    sr.sprite = sprites[Random.Range(0, sprites.Length)];
            }
        }

    }
}
