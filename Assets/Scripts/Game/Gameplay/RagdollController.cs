using UnityEngine;

namespace Game.GamePlay
{
    public class RagdollController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D bodyRigidbody;
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float maxSpeedX = 15f;
        [SerializeField] private float controlSensitivity = 1f;
        [SerializeField] private float boundaryPadding = 0.2f; // Space between player and edge
        [SerializeField] private float maxFallSpeed = -20f;

        private float minX;
        private float maxX;
        private bool isTouching = false;
        private float targetX;

        private void Start()
        {
            float camHeight = 2f * Camera.main.orthographicSize;
            float camWidth = camHeight * Camera.main.aspect;
            float camCenterX = Camera.main.transform.position.x;

            minX = camCenterX - camWidth / 2f + boundaryPadding;
            maxX = camCenterX + camWidth / 2f - boundaryPadding;
        }

        private void Update()
        {
            // Touch (mobile)
            if (Input.touchCount > 0)
            {
                isTouching = true;
                Touch touch = Input.GetTouch(0);
                Vector3 touchWorldPos = Camera.main.ScreenToWorldPoint(touch.position);
                targetX = touchWorldPos.x;
                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    isTouching = false;
            }
            // Mouse (editor/desktop)
            else if (Input.GetMouseButton(0))
            {
                isTouching = true;
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetX = mouseWorldPos.x;
            }
            else
            {
                isTouching = false;
            }
        }

        private void FixedUpdate()
        {
            // Clamp falling speed to maxFallSpeed
            Vector2 lv = bodyRigidbody.linearVelocity;
            if (lv.y < maxFallSpeed)
                lv.y = maxFallSpeed;
            bodyRigidbody.linearVelocity = lv;

            float velocityX = 0f;

            if (isTouching)
            {
                float direction = targetX - bodyRigidbody.position.x;
                velocityX = direction * moveSpeed * controlSensitivity;
                velocityX = Mathf.Clamp(velocityX, -maxSpeedX, maxSpeedX);
            }
            else
            {
                float tilt = Input.acceleration.x;
#if UNITY_EDITOR
                // for testing in the editor
                tilt = Input.GetKey(KeyCode.LeftArrow) ? -1f :
                       Input.GetKey(KeyCode.RightArrow) ? 1f : 0f;
#endif
                velocityX = tilt * moveSpeed * controlSensitivity;
                velocityX = Mathf.Clamp(velocityX, -maxSpeedX, maxSpeedX);
            }

            Vector2 newLinearVelocity = new Vector2(velocityX, bodyRigidbody.linearVelocity.y);
            bodyRigidbody.linearVelocity = newLinearVelocity;

            // Clamp position to X bounds
            Vector2 pos = bodyRigidbody.position;
            if (pos.x < minX)
                pos.x = minX;
            else if (pos.x > maxX)
                pos.x = maxX;
            bodyRigidbody.position = pos;
        }
    }
}
