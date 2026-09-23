using UnityEngine;

public class DetectorInferiorPlataforma : MonoBehaviour
{
    [SerializeField] private PlataformaElevador plataforma;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        plataforma.PlayerEntrouEmbaixo(other.transform);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        plataforma.PlayerSaiuDeBaixo(other.transform);
    }
}