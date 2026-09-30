using UnityEngine;
using UnityEngine.SceneManagement;

public class PortaSaida : MonoBehaviour
{
    [SerializeField] private string nomeCenaMenu = "Menu";

    private bool finalizando;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || finalizando)
            return;

        finalizando = true;

        RafaelDialogueController.ResetarProgressoDialogos();
        FimDemoController.MostrarFimDemo = true;

        SceneManager.LoadScene(nomeCenaMenu);
    }
}