using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class EnergyController : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [Header("Valores")]
    [SerializeField] private int currentEnergy = 0;
    [SerializeField] private int targetEnergy = 10;
    [SerializeField] private int criticalLimit = 15;

    [Header("Interface")]
    [SerializeField] private Slider energyBar;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private Image barFill;

    [Header("Cores")]
    [SerializeField] private Color normalColor = Color.cyan;
    [SerializeField] private Color targetColor = Color.green;
    [SerializeField] private Color warningColor = new Color(1f, 0.55f, 0f);
    [SerializeField] private Color criticalColor = Color.red;

    public int CurrentEnergy => currentEnergy;
    public int TargetEnergy => targetEnergy;
    public int CriticalLimit => criticalLimit;

    private void Start()
    {
        energyBar.minValue = 0;
        energyBar.maxValue = criticalLimit;
        energyBar.wholeNumbers = true;

        UpdateInterface();
    }

    private void Update()
    {
        // Controles temporários para testar a barra.
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            ChangeEnergy(1);
        }

        if (Keyboard.current.yKey.wasPressedThisFrame)
        {
            ChangeEnergy(-1);
        }
    }

    public void ChangeEnergy(int amount)
    {
        currentEnergy += amount;
        currentEnergy = Mathf.Clamp(
            currentEnergy,
            0,
            criticalLimit
        );

        UpdateInterface();

        if (currentEnergy >= criticalLimit)
        {
            MoleculeDegraded();
        }
    }

    public bool HasExactEnergy()
    {
        return currentEnergy == targetEnergy;
    }

    private void UpdateInterface()
    {
        energyBar.value = currentEnergy;

        energyText.text =
            $"Energia: {currentEnergy}/{criticalLimit}\n" +
            $"Alvo da reação: {targetEnergy}";

        if (currentEnergy >= criticalLimit)
        {
            barFill.color = criticalColor;
        }
        else if (currentEnergy > targetEnergy)
        {
            barFill.color = warningColor;
        }
        else if (currentEnergy == targetEnergy)
        {
            barFill.color = targetColor;
        }
        else
        {
            barFill.color = normalColor;
        }
    }

    private void MoleculeDegraded()
    {
        Debug.LogWarning(
            "A molécula atingiu o limite crítico!"
        );

        if (levelManager != null)
        {
            levelManager.ShowGameOver();
        }
        else
        {
            Debug.LogError(
                "LevelManager não foi atribuído ao EnergyController!"
            );
        }
    }
}