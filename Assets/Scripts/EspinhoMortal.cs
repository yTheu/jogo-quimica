using UnityEngine;

public class EspinhoMortal : MonoBehaviour
{
    [SerializeField] private GameOverManager gameOverManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        gameOverManager.GameOver(GameOverManager.TipoMorte.Espinhos);
    }
}