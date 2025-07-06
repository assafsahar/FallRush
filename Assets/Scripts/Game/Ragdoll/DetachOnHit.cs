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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (isDetached || !collision.CompareTag(obstacleTag))
                return;
            Destroy(collision.gameObject);
            hitCount++;

            Quaternion rotation = Quaternion.identity;
            if (gameObject.CompareTag("Left"))
            {
                rotation = Quaternion.Euler(0, 0, 180);
            }
            else if (gameObject.CompareTag("Right"))
            {
                rotation = Quaternion.Euler(0, 0, 0);
            }
            // Play "hit" effect at every hit
            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, transform.position, rotation);
            }

            // Detach only after enough hits
            if (hitCount >= hitsToDetach && hingeJoint != null)
            {
                Rigidbody2D connected = hingeJoint.connectedBody;

                // Spawn continuous effect at the connection point (optional)
                if (continuousEffectPrefab != null && connected != null)
                {
                    Vector3 worldAnchor = connected.transform.TransformPoint(hingeJoint.connectedAnchor);
                    Quaternion effectRotation = transform.rotation;
                    GameObject effect = Instantiate(continuousEffectPrefab, worldAnchor, effectRotation);
                    effect.transform.SetParent(connected.transform);
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
