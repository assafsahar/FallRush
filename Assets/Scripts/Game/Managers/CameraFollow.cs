using UnityEngine;

namespace Game.Managers
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float fixedX = 0f;
        [SerializeField] private float fixedZ = -10f;
        [SerializeField] float offsetY = 2f;

        private void LateUpdate()
        {
            Vector3 camPos = transform.position;
            camPos.x = fixedX;
            camPos.y = target.position.y + offsetY;
            camPos.z = fixedZ;
            transform.position = camPos;
        }
    }
}
