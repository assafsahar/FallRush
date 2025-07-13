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
        [SerializeField] float screenBottomPercent = 0.3f;
        [SerializeField] private float easing = 0.15f;
        [SerializeField] private float gyroSensitivity = 6f;

        private float minX;
        private float maxX;
        private bool isTouching = false;
        private float targetX;
        private float currentVelocityX = 0f;

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
            HandleInput();
        }

        private void HandleInput()
        {
            // Touch (mobile)
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 pos = touch.position;
                if (pos.y < Screen.height * screenBottomPercent)
                {
                    isTouching = true;
                    if (pos.x < Screen.width / 2f)
                        targetX = bodyRigidbody.position.x - 1f; 
                    else
                        targetX = bodyRigidbody.position.x + 1f;
                }
                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    isTouching = false;
            }
            // Mouse (editor/desktop)
            else if (Input.GetMouseButton(0))
            {
                Vector2 pos = Input.mousePosition;
                if (pos.y < Screen.height * screenBottomPercent)
                {
                    isTouching = true;
                    if (pos.x < Screen.width / 2f)
                        targetX = bodyRigidbody.position.x - 1f;
                    else
                        targetX = bodyRigidbody.position.x + 1f;
                }
            }
            else
            {
                isTouching = false;
            }
        }

        private void FixedUpdate()
        {
            ClampFallSpeed();
            MoveRagdoll();
            ClampPosition();
        }

        private void ClampPosition()
        {
            // Clamp position to X bounds
            Vector2 pos = bodyRigidbody.position;
            if (pos.x < minX)
                pos.x = minX;
            else if (pos.x > maxX)
                pos.x = maxX;
            bodyRigidbody.position = pos;
        }

        private void MoveRagdoll()
        {
            float targetVelocityX = 0f;

            if (isTouching)
            {
                float direction = targetX - bodyRigidbody.position.x;
                targetVelocityX = direction * moveSpeed * controlSensitivity;
                targetVelocityX = Mathf.Clamp(targetVelocityX, -maxSpeedX, maxSpeedX);
            }
            else
            {
                float tilt = Input.acceleration.x;
#if UNITY_EDITOR
                // for testing in the editor
                tilt = Input.GetKey(KeyCode.LeftArrow) ? -1f :
                       Input.GetKey(KeyCode.RightArrow) ? 1f : 0f;
#endif
                targetVelocityX = tilt * moveSpeed * controlSensitivity * gyroSensitivity;

                targetVelocityX = Mathf.Clamp(targetVelocityX, -maxSpeedX, maxSpeedX);
            }
            // easing
            currentVelocityX = Mathf.Lerp(currentVelocityX, targetVelocityX, easing);

            Vector2 newLinearVelocity = new Vector2(currentVelocityX, bodyRigidbody.linearVelocity.y);
            bodyRigidbody.linearVelocity = newLinearVelocity;
        }

        private void ClampFallSpeed()
        {
            // Clamp falling speed to maxFallSpeed
            Vector2 lv = bodyRigidbody.linearVelocity;
            if (lv.y < maxFallSpeed)
                lv.y = maxFallSpeed;
            bodyRigidbody.linearVelocity = lv;
        }
    }
}
