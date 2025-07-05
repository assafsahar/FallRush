using UnityEngine;
using UnityEngine.Pool;

namespace Core.Utils
{
    public class ReturnToPool : MonoBehaviour
    {
        private ObjectPool<GameObject> pool;
        private Camera mainCam;

        // Set the pool that this object belongs to
        public void SetPool(ObjectPool<GameObject> _pool)
        {
            pool = _pool;
            if (!mainCam) mainCam = Camera.main;
        }

        private void Update()
        {
            // Return the object to pool if it moves above the camera by 5 units
            if (mainCam && transform.position.y > mainCam.transform.position.y + 5f)
            {
                pool.Release(gameObject);
            }
        }
    }
}
