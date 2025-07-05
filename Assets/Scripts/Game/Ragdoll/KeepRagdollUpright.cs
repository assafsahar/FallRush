using UnityEngine;

namespace Game.Ragdoll
{
    public class KeepRagdollUpright : MonoBehaviour
    {
        public float uprightStrength = 10f;

        Rigidbody2D rb;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            float angle = rb.rotation;
            float desiredAngle = 180f;

            float delta = Mathf.DeltaAngle(angle, desiredAngle);
            rb.AddTorque(-delta * uprightStrength);
        }
    }
}
