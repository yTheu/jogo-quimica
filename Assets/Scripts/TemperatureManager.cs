using UnityEngine;
using UnityEngine.InputSystem;

public class TemperatureManager : MonoBehaviour
{
    [Range(0, 15)]
    public int temperaturaAtual = 5;

    [SerializeField] private PlayerController player;
    [SerializeField] private GameOverManager gameOverManager;

    private bool explodiu = false;

    private void Start()
    {
        player.AtualizarVelocidadePorTemperatura(temperaturaAtual);
    }

    void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            AumentarTemperatura();
        }

        if (Keyboard.current.yKey.wasPressedThisFrame)
        {
            DiminuirTemperatura();
        }
    }

    public void AumentarTemperatura()
    {
        temperaturaAtual++;

        if (temperaturaAtual > 15)
            temperaturaAtual = 15;

        player.AtualizarVelocidadePorTemperatura(temperaturaAtual);

        if (temperaturaAtual == 15 && !explodiu)
        {
            explodiu = true;
            gameOverManager.GameOver(GameOverManager.TipoMorte.Explosao);
        }
    }

    public void DiminuirTemperatura()
    {
        temperaturaAtual--;

        if (temperaturaAtual < 0)
            temperaturaAtual = 0;

        player.AtualizarVelocidadePorTemperatura(temperaturaAtual);
    }
}