using UnityEngine;

namespace Game.Ragdoll
{
    public class RagdollConstantSpin : MonoBehaviour
    {
        [SerializeField] private float spinTorque = 2f;

        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (Mathf.Abs(rb.angularVelocity) < 0.1f)
                rb.angularVelocity = 0f; 
            rb.AddTorque(spinTorque);
        }

    }
}
