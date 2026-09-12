using UnityEngine;
using UnityEngine.InputSystem;

public class PressureLever : MonoBehaviour
{
    [SerializeField] private int leverIndex;
    [SerializeField] private PressureStageManager stageManager;

    [Header("Visual")]
    [SerializeField] private SpriteRenderer leverRenderer;
    [SerializeField] private Sprite activatedSprite;

    private bool playerNearby;
    private bool activated;

    private void Update()
    {
        if (activated || !playerNearby)
            return;

        if (Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            Activate();
        }
    }

    private void Activate()
    {
        activated = true;

        if (leverRenderer != null &&
            activatedSprite != null)
        {
            leverRenderer.sprite = activatedSprite;
        }

        if (stageManager != null)
            stageManager.ActivateLever(leverIndex);

        Debug.Log($"Alavanca {leverIndex + 1} ativada.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNearby = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNearby = false;
    }
}