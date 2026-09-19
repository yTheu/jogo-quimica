using UnityEngine;

public class AguaMortal : MonoBehaviour
{
    [SerializeField] private GameOverManager gameOverManager;
    [SerializeField] private TemperatureManager temperatureManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (temperatureManager.temperaturaAtual >= 10)
        {
            gameOverManager.GameOver(GameOverManager.TipoMorte.AguaFervente);
            return;
        }

        gameOverManager.GameOver(GameOverManager.TipoMorte.Afogado);
    }
}