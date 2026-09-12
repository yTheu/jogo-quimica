using UnityEngine;

public class TimedValve : MonoBehaviour
{
    [Header("Configuração")]
    [SerializeField] private float activeDuration = 10f;
    [SerializeField] private float rotationSpeed = 360f;

    [Header("Referências")]
    [SerializeField] private Transform valveVisual;
    [SerializeField] private ValveChallengeManager challengeManager;

    [Header("Visual")]
    [SerializeField] private SpriteRenderer indicator;
    [SerializeField] private Color inactiveColor = Color.red;
    [SerializeField] private Color activeColor = Color.green;

    public bool IsActive { get; private set; }
    public float RemainingTime { get; private set; }
    public float ActiveDuration => activeDuration;

    private Quaternion initialRotation;
    private Quaternion activeRotation;

    private void Awake()
    {
        if (valveVisual == null)
            valveVisual = transform;

        initialRotation = valveVisual.localRotation;

        activeRotation = initialRotation *
                         Quaternion.Euler(0f, 0f, -90f);

        UpdateIndicator();
    }

    private void Update()
    {
        UpdateRotation();

        if (!IsActive)
            return;

        RemainingTime -= Time.deltaTime;

        if (RemainingTime <= 0f)
            Deactivate();
    }

    public void Activate()
    {
        IsActive = true;
        RemainingTime = activeDuration;

        UpdateIndicator();

        if (challengeManager != null)
            challengeManager.CheckValves();
    }

    private void Deactivate()
    {
        IsActive = false;
        RemainingTime = 0f;

        UpdateIndicator();

        if (challengeManager != null)
            challengeManager.CheckValves();
    }

    private void UpdateRotation()
    {
        Quaternion targetRotation = IsActive
            ? activeRotation
            : initialRotation;

        valveVisual.localRotation = Quaternion.RotateTowards(
            valveVisual.localRotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private void UpdateIndicator()
    {
        if (indicator == null)
            return;

        indicator.color = IsActive
            ? activeColor
            : inactiveColor;
    }
}