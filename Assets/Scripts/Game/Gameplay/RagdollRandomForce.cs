using UnityEngine;

namespace Game.GamePlay
{
    public class RagdollRandomForce : MonoBehaviour
    {
        void Start()
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 randomForce = new Vector2(Random.Range(-1f, 1f), 0f) * 50f;
                float randomTorque = Random.Range(-50f, 50f);
                rb.AddForce(randomForce, ForceMode2D.Impulse);
                rb.AddTorque(randomTorque, ForceMode2D.Impulse);
            }
        }
    }
}
