using UnityEngine;

public class ValveFloorButton : MonoBehaviour
{
    [SerializeField] private TimedValve connectedValve;

    [Header("Animação do botão")]
    [SerializeField] private Transform buttonVisual;
    [SerializeField] private float pressedDistance = 0.1f;

    private Vector3 initialPosition;
    private int playersOnButton;

    private void Awake()
    {
        if (buttonVisual != null)
            initialPosition = buttonVisual.localPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playersOnButton++;

        if (connectedValve != null)
            connectedValve.Activate();

        UpdateButtonVisual();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playersOnButton = Mathf.Max(
            0,
            playersOnButton - 1
        );

        UpdateButtonVisual();
    }

    private void UpdateButtonVisual()
    {
        if (buttonVisual == null)
            return;

        buttonVisual.localPosition = playersOnButton > 0
            ? initialPosition + Vector3.down * pressedDistance
            : initialPosition;
    }
}