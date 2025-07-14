using UnityEngine;

namespace Game.Ragdoll
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class RagdollConstantSpin : MonoBehaviour
    {
        [SerializeField] private float spinPower = -90f; // Degrees per second

        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            rb.angularVelocity = spinPower;
        }
    }
}
