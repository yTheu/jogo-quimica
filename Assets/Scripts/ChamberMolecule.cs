using UnityEngine;

public class ChamberMolecule : MonoBehaviour
{
    [SerializeField] private float minimumSpeed = 0.8f;
    [SerializeField] private float maximumSpeed = 1.3f;

    private PressureChamberVisual chamber;
    private Vector2 direction;
    private float baseSpeed;
    private float speedMultiplier = 1f;

    public void Initialize(PressureChamberVisual chamberVisual)
    {
        chamber = chamberVisual;

        direction = Random.insideUnitCircle.normalized;

        if (direction == Vector2.zero)
            direction = Vector2.right;

        baseSpeed = Random.Range(
            minimumSpeed,
            maximumSpeed
        );
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }

    private void Update()
    {
        if (chamber == null)
            return;

        Vector3 position = transform.localPosition;

        position += (Vector3)(
            direction *
            baseSpeed *
            speedMultiplier *
            Time.deltaTime
        );

        chamber.GetMovementLimits(
            out float minimumX,
            out float maximumX,
            out float minimumY,
            out float maximumY
        );

        if (position.x <= minimumX ||
            position.x >= maximumX)
        {
            direction.x *= -1f;
            position.x = Mathf.Clamp(
                position.x,
                minimumX,
                maximumX
            );
        }

        if (position.y <= minimumY ||
            position.y >= maximumY)
        {
            direction.y *= -1f;
            position.y = Mathf.Clamp(
                position.y,
                minimumY,
                maximumY
            );
        }

        transform.localPosition = position;
    }
}