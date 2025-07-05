using UnityEngine;

namespace Game.Obstacles
{
    public class ObstacleRotator : MonoBehaviour
    {
        private float rotationSpeed;

        public void SetRotationSpeed(float speed)
        {
            rotationSpeed = speed;
        }

        void Update()
        {
            transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        }
    }
}
