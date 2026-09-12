using UnityEngine;

public class PlayerVisualAnimator : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private Transform visual;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Direção")]
    [SerializeField] private float movementThreshold = 0.05f;
    [SerializeField] private bool spriteFacesRight = true;

    [Header("Respiração")]
    [SerializeField] private float breathingSpeed = 2f;
    [SerializeField] private float breathingAmount = 0.04f;

    [Header("Movimento")]
    [SerializeField] private float runningAnimationSpeed = 10f;
    [SerializeField] private float runningSquashAmount = 0.06f;
    [SerializeField] private float runningBobAmount = 0.04f;

    private Vector3 originalScale;
    private Vector3 originalPosition;

    private void Awake()
    {
        if (visual != null)
        {
            originalScale = visual.localScale;
            originalPosition = visual.localPosition;
        }
    }

    private void LateUpdate()
    {
        if (playerRigidbody == null || visual == null)
            return;

        float horizontalSpeed = playerRigidbody.linearVelocity.x;
        bool isMoving = Mathf.Abs(horizontalSpeed) > movementThreshold;

        UpdateDirection(horizontalSpeed);

        if (isMoving)
            AnimateRunning();
        else
            AnimateBreathing();
    }

    private void UpdateDirection(float horizontalSpeed)
    {
        if (Mathf.Abs(horizontalSpeed) <= movementThreshold)
            return;

        bool movingRight = horizontalSpeed > 0f;

        spriteRenderer.flipX = spriteFacesRight
            ? !movingRight
            : movingRight;
    }

    private void AnimateBreathing()
    {
        float breathing = Mathf.Sin(Time.time * breathingSpeed)
                          * breathingAmount;

        visual.localScale = new Vector3(
            originalScale.x * (1f - breathing * 0.4f),
            originalScale.y * (1f + breathing),
            originalScale.z
        );

        visual.localPosition = originalPosition + new Vector3(
            0f,
            breathing * 0.15f,
            0f
        );
    }

    private void AnimateRunning()
    {
        float movement = Mathf.Sin(
            Time.time * runningAnimationSpeed
        );

        float squash = movement * runningSquashAmount;
        float bob = Mathf.Abs(movement) * runningBobAmount;

        visual.localScale = new Vector3(
            originalScale.x * (1f + squash),
            originalScale.y * (1f - squash),
            originalScale.z
        );

        visual.localPosition = originalPosition + new Vector3(
            0f,
            bob,
            0f
        );
    }
}