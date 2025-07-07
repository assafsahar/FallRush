using UnityEngine;
using UnityEngine.Pool;

namespace Core.Utils
{
    public class ReturnToPool : MonoBehaviour
    {
        private ObjectPool<GameObject> pool;
        private Camera mainCam;
        private float yOffsetFromCamera = 5f;

        // Set the pool that this object belongs to
        public void SetPool(ObjectPool<GameObject> _pool)
        {
            pool = _pool;
            if (!mainCam) mainCam = Camera.main;
        }

        private void Update()
        {
            CheckIfShouldReturnToPool();
        }

        public void SetYOffsetFromCamera(float offset)
        {
            yOffsetFromCamera = offset;
        }

        private void CheckIfShouldReturnToPool()
        {
            if (mainCam && transform.position.y > mainCam.transform.position.y + yOffsetFromCamera)
            {
                pool.Release(gameObject);
            }
        }

        
    }
}
