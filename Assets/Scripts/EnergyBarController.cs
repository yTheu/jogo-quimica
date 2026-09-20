using UnityEngine;
using UnityEngine.UI;

public class EnergyBarController : MonoBehaviour
{
    [SerializeField] private TemperatureChamberVisual temperatureChamber;
    [SerializeField] private Image fillImage;
    [SerializeField] private RectTransform activationMarker;
    [SerializeField] private float activationTemperature = 81.25f;

    private RectTransform barRect;

    private void Awake()
    {
        barRect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        UpdateActivationMarker();
    }

    private void Update()
    {
        if (temperatureChamber == null || fillImage == null)
            return;

        float normalizedEnergy = Mathf.InverseLerp(
            -25f,
            112.5f,
            temperatureChamber.InternalTemperature
        );

        fillImage.fillAmount = normalizedEnergy;
    }

    private void UpdateActivationMarker()
    {
        if (activationMarker == null || barRect == null)
            return;

        float normalizedActivation = Mathf.InverseLerp(
            -25f,
            112.5f,
            activationTemperature
        );

        float barWidth = barRect.rect.width;

        float positionX =
            -barWidth * 0.5f +
            barWidth * normalizedActivation;

        Vector2 position = activationMarker.anchoredPosition;
        position.x = positionX;
        activationMarker.anchoredPosition = position;
    }
}