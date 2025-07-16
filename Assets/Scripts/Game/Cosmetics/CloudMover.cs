using UnityEngine;
using UnityEngine.Pool;

namespace Game.Cosmetics
{
    public class CloudMover : MonoBehaviour
    {
        private float speed = 1f;
        private ObjectPool<GameObject> pool;

        public void SetSpeed(float s) => speed = s;
        public void SetPool(ObjectPool<GameObject> p) => pool = p;

        private void FixedUpdate()
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);

            if (Camera.main != null)
            {
                Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
                if (transform.position.y > topRight.y + 1f)
                    pool?.Release(gameObject);
            }
        }
    }
}
