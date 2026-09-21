using UnityEngine;

public class TriggerSequencia3 : MonoBehaviour
{
    [SerializeField] private RafaelDialogueController rafaelDialogue;

    private bool ativado;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ativado)
            return;

        if (!other.CompareTag("Player"))
            return;

        ativado = true;

        if (rafaelDialogue != null)
            rafaelDialogue.DispararSequencia3();
    }
}