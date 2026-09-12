using UnityEngine;

public class PsiCollectible : MonoBehaviour
{
    [SerializeField] private int value = 1;

    private bool collected;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected || !other.CompareTag("Player"))
            return;

        PsiInventory inventory =
            other.GetComponentInParent<PsiInventory>();

        if (inventory == null)
            return;

        if (inventory.AddPoints(value))
        {
            collected = true;
            gameObject.SetActive(false);
        }
    }
}