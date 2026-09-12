using TMPro;
using UnityEngine;

public class PsiInventory : MonoBehaviour
{
    [Header("Inventário")]
    [SerializeField] private int maximumPsi = 5;

    [Header("Interface")]
    [SerializeField] private TMP_Text psiText;

    public int CurrentPsi { get; private set; }
    public int MaximumPsi => maximumPsi;

    private void Start()
    {
        UpdateUI();
    }

    public bool AddPsi(int amount = 1)
    {
        if (amount <= 0)
            return false;

        if (CurrentPsi >= maximumPsi)
            return false;

        CurrentPsi = Mathf.Min(
            CurrentPsi + amount,
            maximumPsi
        );

        UpdateUI();

        Debug.Log($"PSI coletado: {CurrentPsi}/{maximumPsi}");

        return true;
    }

    public bool AddPoints(int amount = 1)
    {
        return AddPsi(amount);
    }

    public int RemovePoints(int amount)
    {
        if (amount <= 0)
            return 0;

        int removedAmount = Mathf.Min(CurrentPsi, amount);

        CurrentPsi -= removedAmount;
        UpdateUI();

        Debug.Log(
            $"PSI depositado: {removedAmount}. Restante: {CurrentPsi}/{maximumPsi}"
        );

        return removedAmount;
    }

    public bool RemovePsi(int amount = 1)
    {
        if (amount <= 0)
            return false;

        if (CurrentPsi < amount)
            return false;

        CurrentPsi -= amount;

        UpdateUI();

        Debug.Log($"PSI restante: {CurrentPsi}/{maximumPsi}");

        return true;
    }

    public bool HasPsi(int amount = 1)
    {
        return CurrentPsi >= amount;
    }

    private void UpdateUI()
    {
        if (psiText != null)
        {
            psiText.text =
                $"PSI: {CurrentPsi}/{maximumPsi}";
        }
    }
}