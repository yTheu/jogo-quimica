using UnityEngine;
using UnityEngine.InputSystem;

public class PsiDepositMachine : MonoBehaviour
{
    [Header("Configuração")]
    [SerializeField] private int requiredPoints = 5;

    [Header("Referências")]
    [SerializeField] private SlidingMechanism challengeDoor;
    [SerializeField] private GameObject interactionIcon;

    [Header("Indicadores da máquina")]
    [SerializeField] private SpriteRenderer[] slots;
    [SerializeField] private Color emptyColor = Color.gray;
    [SerializeField] private Color filledColor = Color.green;

    private PsiInventory nearbyInventory;
    private int depositedPoints;
    private bool completed;

    private void Start()
    {
        if (interactionIcon != null)
            interactionIcon.SetActive(false);

        UpdateSlots();
    }

    private void Update()
    {
        if (completed || nearbyInventory == null)
            return;

        if (Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            DepositPoints();
        }
    }

    public void DepositPoints()
    {
        if (completed || nearbyInventory == null)
            return;

        int missingPoints =
            requiredPoints - depositedPoints;

        int transferredPoints =
            nearbyInventory.RemovePoints(missingPoints);

        depositedPoints += transferredPoints;

        UpdateSlots();

        Debug.Log(
            $"PSI depositado: {depositedPoints}/{requiredPoints}"
        );

        if (depositedPoints >= requiredPoints)
            CompleteMachine();
    }

    private void CompleteMachine()
    {
        completed = true;

        if (challengeDoor != null)
            challengeDoor.SetOpen(true);

        if (interactionIcon != null)
            interactionIcon.SetActive(false);

        Debug.Log("Máquina completa! Terceira alavanca liberada.");
    }

    private void UpdateSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
                continue;

            slots[i].color = i < depositedPoints
                ? filledColor
                : emptyColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        nearbyInventory =
            other.GetComponentInParent<PsiInventory>();

        if (interactionIcon != null && !completed)
            interactionIcon.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        nearbyInventory = null;

        if (interactionIcon != null)
            interactionIcon.SetActive(false);
    }
}