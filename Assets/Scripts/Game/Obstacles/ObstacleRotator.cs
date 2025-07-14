using UnityEngine;

namespace Game.Obstacles
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ObstacleRotator : MonoBehaviour
    {
        private float rotationSpeed;
        private Rigidbody2D rb;

        public void SetRotationSpeed(float speed)
        {
            rotationSpeed = speed;
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            rb.MoveRotation(rb.rotation + rotationSpeed * Time.fixedDeltaTime);
        }
    }
}
