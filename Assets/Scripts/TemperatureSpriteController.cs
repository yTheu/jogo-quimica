using UnityEngine;

public class TemperatureSpriteController : MonoBehaviour
{
    [SerializeField] private TemperatureManager temperatureManager;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private Sprite spriteNormal;
    [SerializeField] private Sprite spriteExtremo;

    private void Update()
    {
        if (temperatureManager == null || spriteRenderer == null)
            return;

        if (temperatureManager.temperaturaAtual >= 11 &&
            temperatureManager.temperaturaAtual < 15)
        {
            spriteRenderer.sprite = spriteExtremo;
        }
        else
        {
            spriteRenderer.sprite = spriteNormal;
        }
    }
}