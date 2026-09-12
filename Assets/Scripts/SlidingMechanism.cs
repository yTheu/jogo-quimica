using UnityEngine;

public class SlidingMechanism : MonoBehaviour
{
    [SerializeField] private Vector3 openOffset;
    [SerializeField] private float movementSpeed = 4f;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpen;

    private void Awake()
    {
        closedPosition = transform.localPosition;
        openPosition = closedPosition + openOffset;
    }

    private void Update()
    {
        Vector3 targetPosition = isOpen
            ? openPosition
            : closedPosition;

        transform.localPosition = Vector3.MoveTowards(
            transform.localPosition,
            targetPosition,
            movementSpeed * Time.deltaTime
        );
    }

    public void SetOpen(bool open)
    {
        isOpen = open;
    }
}