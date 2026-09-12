using UnityEngine;
using UnityEngine.UI;

public class PressureStageManager : MonoBehaviour
{
    [Header("Interface das alavancas")]
    [SerializeField] private Image[] leverIndicators;

    [SerializeField] private Color inactiveLeverColor =
        new Color(0.3f, 0.35f, 0.45f, 1f);

    [SerializeField] private Color activeLeverColor =
        new Color(0.2f, 1f, 0.45f, 1f);

    [Header("Elementos da fase")]
    [SerializeField] private NextLevelDoor nextLevelDoor;
    [SerializeField] private PressureChamberVisual chamberVisual;
    [SerializeField] private SlidingMechanism finalDoor;

    [Header("Luzes no cenário")]
    [SerializeField] private SpriteRenderer[] indicatorLights;

    private readonly bool[] activatedLevers = new bool[3];

    public int ActivatedLeverCount { get; private set; }

    private void Start()
    {
        UpdateIndicators();
    }

    public void ActivateLever(int leverIndex)
    {
        if (leverIndex < 0 || leverIndex >= activatedLevers.Length)
            return;

        if (activatedLevers[leverIndex])
            return;

        activatedLevers[leverIndex] = true;
        ActivatedLeverCount++;

        if (chamberVisual != null)
        {
            chamberVisual.SetPressureLevel(
                ActivatedLeverCount
            );
        }

        UpdateIndicators();

        Debug.Log(
            $"Alavancas ativadas: {ActivatedLeverCount}/3"
        );

        if (ActivatedLeverCount >= activatedLevers.Length)
            CompletePressureStage();
    }

    private void UpdateIndicators()
    {
        // Atualiza os indicadores fixos do Canvas
        if (leverIndicators != null)
        {
            for (int i = 0; i < leverIndicators.Length; i++)
            {
                if (leverIndicators[i] == null)
                    continue;

                bool isActive =
                    i < activatedLevers.Length &&
                    activatedLevers[i];

                leverIndicators[i].color = isActive
                    ? activeLeverColor
                    : inactiveLeverColor;
            }
        }

        // Atualiza as luzes existentes no cenário
        if (indicatorLights != null)
        {
            for (int i = 0; i < indicatorLights.Length; i++)
            {
                if (indicatorLights[i] == null)
                    continue;

                bool isActive =
                    i < activatedLevers.Length &&
                    activatedLevers[i];

                indicatorLights[i].color = isActive
                    ? activeLeverColor
                    : inactiveLeverColor;
            }
        }
    }

    private void CompletePressureStage()
    {
        if (finalDoor != null)
            finalDoor.SetOpen(true);

        if (nextLevelDoor != null)
            nextLevelDoor.Unlock();

        Debug.Log(
            "Pressão máxima atingida! Porta final liberada."
        );
    }
}