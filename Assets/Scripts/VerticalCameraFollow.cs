using UnityEngine;

public class VerticalCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    [Header("Posição")]
    [SerializeField] private Vector2 offset = new Vector2(0f, 1.5f);

    [Header("Suavização")]
    [SerializeField] private float smoothTime = 0.2f;

    private Vector3 velocity;

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 desiredPosition = new Vector3(
            target.position.x + offset.x,
            target.position.y + offset.y,
            transform.position.z
        );

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref velocity,
            smoothTime
        );
    }
}