using UnityEngine;

namespace Game.Obstacles
{
    public class ResettableObstacle : MonoBehaviour
    {
        private Collider2D obstacleCollider;

        private void Awake()
        {
            obstacleCollider = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
            if (obstacleCollider != null)
                obstacleCollider.enabled = true;
        }
    }
}
