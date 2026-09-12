using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movimento")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float waitTime = 0.5f;

    private Vector3 destination;
    private float waitCounter;

    private void Start()
    {
        destination = pointB.position;
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, destination) <= 0.02f)
        {
            waitCounter += Time.fixedDeltaTime;

            if (waitCounter >= waitTime)
            {
                destination = destination == pointA.position
                    ? pointB.position
                    : pointA.position;

                waitCounter = 0f;
            }

            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            destination,
            speed * Time.fixedDeltaTime
        );
    }
}