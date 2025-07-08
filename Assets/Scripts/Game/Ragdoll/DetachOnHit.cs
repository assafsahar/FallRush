using System.Collections;
using UnityEngine;

namespace Game.Ragdoll
{
    public class DetachOnHit : MonoBehaviour
    {
        [SerializeField] private string obstacleTag = "Obstacle";
        [SerializeField] private GameObject hitEffectPrefab;
        [SerializeField] private GameObject continuousEffectPrefab;
        [SerializeField] private int hitsToDetach = 3;

        private int hitCount = 0;
        private HingeJoint2D hingeJoint;
        private bool isDetached = false;
        private Collider2D collider2D;

        private void Awake()
        {
            hingeJoint = GetComponent<HingeJoint2D>();
            collider2D = GetComponent<Collider2D>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (isDetached || !collision.collider.CompareTag(obstacleTag))
                return;

            ApplyHorizontalPush(collision.transform.position);
            TurnOffCollider(collision);

            HandleHit(collision.collider.gameObject);
        }

        private static void TurnOffCollider(Collision2D collision)
        {
            Collider2D obstacleCollider = collision.gameObject.GetComponent<Collider2D>();
            if (obstacleCollider != null)
            {
                obstacleCollider.enabled = false;
            }
        }

        private void ApplyHorizontalPush(Vector2 fromPosition)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb == null) return;

            // Calculate horizontal direction only
            float direction = transform.position.x > fromPosition.x ? 1f : -1f;
            Vector2 force = new Vector2(direction * 200f, 0f); // Adjust force if needed
            rb.AddForce(force, ForceMode2D.Impulse);
        }

        private void HandleHit(GameObject obstacle)
        {
            //Destroy(obstacle);
            hitCount++;

            Quaternion rotation = Quaternion.identity;
            if (gameObject.CompareTag("Left"))
                rotation = Quaternion.Euler(0, 0, 180);
            else if (gameObject.CompareTag("Right"))
                rotation = Quaternion.Euler(0, 0, 0);

            if (hitEffectPrefab != null)
                Instantiate(hitEffectPrefab, transform.position, rotation);

            if (hitCount >= hitsToDetach && hingeJoint != null)
            {
                if (continuousEffectPrefab != null && hingeJoint.connectedBody != null)
                {
                    Vector3 worldAnchor = hingeJoint.connectedBody.transform.TransformPoint(hingeJoint.connectedAnchor);
                    Quaternion effectRotation = transform.rotation;
                    GameObject effect = Instantiate(continuousEffectPrefab, worldAnchor, effectRotation);
                    effect.transform.SetParent(hingeJoint.connectedBody.transform);
                }

                StartCoroutine(DetachJoint());
                if (GameManager.Instance != null)
                    GameManager.Instance.RegisterJointDetached();
                isDetached = true;
            }
        }
        private IEnumerator DetachJoint()
        {
            yield return null;

            if (hingeJoint != null)
            {
                hingeJoint.connectedBody = null;
                hingeJoint.enabled = false;
                hingeJoint = null;
            }
        }

    }

}
