using UnityEngine;

public class SensorElevador : MonoBehaviour
{
    [SerializeField] private PlataformaElevador plataforma;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        plataforma.JogadorEntrou();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        plataforma.JogadorSaiu();
    }
}