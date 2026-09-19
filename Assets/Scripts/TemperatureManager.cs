using UnityEngine;
using UnityEngine.InputSystem;

public class TemperatureManager : MonoBehaviour
{
    [Range(0, 12)]
    public int temperaturaAtual = 4;

    [SerializeField] private PlayerController player;
    [SerializeField] private GameOverManager gameOverManager;
    [SerializeField] private RafaelDialogueController rafaelDialogue;

    [SerializeField] private float tempoMaximoMenos12 = 8f;
    [SerializeField] private float tempoMaximoMenos25 = 4f;

    private bool explodiu = false;
    private bool morreuDeFrio = false;
    private float tempoExposicaoFrio = 0f;

    public float TemperaturaCelsius
    {
        get
        {
            return 12.5f * temperaturaAtual - 25f;
        }
    }

    private void Start()
    {
        player.AtualizarVelocidadePorTemperatura(temperaturaAtual);
    }

    private void Update()
    {
        AtualizarExposicaoFrio();

        if (Keyboard.current == null)
            return;

        if (rafaelDialogue != null)
        {
            if (rafaelDialogue.DialogoAtivo)
                return;

            if (!rafaelDialogue.PodeAlterarTemperatura)
                return;
        }

        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            AumentarTemperatura();
        }

        if (Keyboard.current.yKey.wasPressedThisFrame)
        {
            DiminuirTemperatura();
        }
    }

    private void AtualizarExposicaoFrio()
    {
        if (morreuDeFrio)
            return;

        if (temperaturaAtual >= 2)
        {
            tempoExposicaoFrio = 0f;
            return;
        }

        tempoExposicaoFrio += Time.deltaTime;

        float tempoMaximo = temperaturaAtual == 0
            ? tempoMaximoMenos25
            : tempoMaximoMenos12;

        if (tempoExposicaoFrio >= tempoMaximo)
        {
            morreuDeFrio = true;
            gameOverManager.GameOver(GameOverManager.TipoMorte.FrioExtremo);
        }
    }

    public void AumentarTemperatura()
    {
        int temperaturaAnterior = temperaturaAtual;

        temperaturaAtual++;

        if (temperaturaAtual > 12)
            temperaturaAtual = 12;

        player.AtualizarVelocidadePorTemperatura(temperaturaAtual);

        if (temperaturaAtual > temperaturaAnterior && rafaelDialogue != null)
            rafaelDialogue.RegistrarAumentoTemperatura();

        if (temperaturaAtual == 12 && !explodiu)
        {
            explodiu = true;
            gameOverManager.GameOver(GameOverManager.TipoMorte.Explosao);
        }
    }

    public void DiminuirTemperatura()
    {
        int temperaturaAnterior = temperaturaAtual;

        temperaturaAtual--;

        if (temperaturaAtual < 0)
            temperaturaAtual = 0;

        player.AtualizarVelocidadePorTemperatura(temperaturaAtual);

        if (temperaturaAtual < temperaturaAnterior && rafaelDialogue != null)
            rafaelDialogue.RegistrarDiminuicaoTemperatura();
    }
}