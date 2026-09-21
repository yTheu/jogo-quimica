using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class RafaelDialogueController : MonoBehaviour
{
    [SerializeField] private GameObject conteudoDialogo;
    [SerializeField] private TMP_Text textoDialogo;
    [SerializeField] private Image imagemRafael;

    [SerializeField] private Sprite rafaelSurpreso;
    [SerializeField] private Sprite rafaelApresentando;
    [SerializeField] private Sprite rafaelExplicando;
    [SerializeField] private Sprite rafaelNervoso;
    [SerializeField] private Sprite rafaelOrgulhoso;

    [SerializeField] private PlayerController player;
    [SerializeField] private TemperatureManager temperatureManager;
    [SerializeField] private float tempoParaSequencia2 = 1.5f;

    [SerializeField] private float atrasoInicial = 0.7f;
    [SerializeField] private float velocidadeTexto = 0.03f;

    public static bool PularIntroducaoAoReiniciar { get; set; } = false;
    public static bool Sequencia2JaConcluida { get; set; } = false;

    private const int temperaturaInicial = 4;

    private readonly string[] falasSequencia1 =
    {
        "Opa! Você chegou mais rápido do que eu esperava!",
        "Eu sou Rafael, o cientista responsável por este laboratório. Seja bem-vindo!",
        "Essa área é dedicada ao estudo da temperatura. Parece simples, mas ela pode mudar bastante o comportamento das partículas — e até a velocidade de uma reação química.",
        "Você mesmo vai poder controlar a temperatura daqui. Use T para aumentar e Y para diminuir.",
        "Só cuidado para não exagerar. O laboratório foi feito para aguentar muita coisa... você, talvez nem tanto...",
        "Enfim! Vá em frente. Experimente alterar a temperatura e se movimentar um pouco. Observe o que acontece com você e com o ambiente. Depois a gente conversa."
    };

    private readonly string[] falasSequencia2Aquecimento =
    {
        "Percebeu? Ao aumentar a temperatura, você ficou mais rápido.",
        "Isso acontece porque a temperatura está relacionada à energia cinética média das partículas. Quanto maior a temperatura, maior tende a ser a agitação delas.",
        "Se você tivesse diminuído a temperatura, o contrário teria acontecido: as partículas ficariam menos agitadas e se movimentariam mais lentamente.",
        "E não observe só você! A temperatura também pode alterar o comportamento do ambiente e dos mecanismos deste laboratório.",
        "Ah, e toda essa mudança na agitação das partículas tem uma consequência importante nas colisões entre elas...",
        "Continue em frente. Tem uma câmara mais adiante onde você vai poder observar isso melhor."
    };

    private readonly string[] falasSequencia2Resfriamento =
    {
        "Percebeu? Ao diminuir a temperatura, você ficou mais lento.",
        "Isso acontece porque a temperatura está relacionada à energia cinética média das partículas. Quanto menor a temperatura, menor tende a ser a agitação delas.",
        "Se você tivesse aumentado a temperatura, o contrário teria acontecido: as partículas ficariam mais agitadas e se movimentariam mais rapidamente.",
        "E não observe só você! A temperatura também pode alterar o comportamento do ambiente e dos mecanismos deste laboratório.",
        "Ah, e toda essa mudança na agitação das partículas tem uma consequência importante nas colisões entre elas...",
        "Continue em frente. Tem uma câmara mais adiante onde você vai poder observar isso melhor."
    };

    private readonly string[] falasSequencia2Ambos =
    {
        "Você testou os dois sentidos, hein? Percebeu como a temperatura alterou sua velocidade?",
        "Isso acontece porque a temperatura está relacionada à energia cinética média das partículas. Ao aumentar a temperatura, você aumenta a agitação delas; ao diminuir, reduz essa agitação.",
        "Por isso você ficou mais rápido quando aqueceu e mais lento quando resfriou. O seu movimento aqui está representando justamente essa diferença no comportamento das partículas.",
        "E não observe só você! A temperatura também pode alterar o comportamento do ambiente e dos mecanismos deste laboratório.",
        "Ah, e toda essa mudança na agitação das partículas tem uma consequência importante nas colisões entre elas...",
        "Continue em frente. Tem uma câmara mais adiante onde você vai poder observar isso melhor."
    };

    private readonly string[] falasSequencia3 =
    {
    "Bom, chegamos à câmara de reação! Lembra do que eu falei sobre a temperatura mudar a agitação das partículas? Pois é... agora você vai ver o que isso significa durante uma colisão.",
    "Quanto maior a temperatura, maior a energia cinética média das partículas. Elas se movimentam mais rápido e podem colidir com mais energia.",
    "Só que nem toda colisão provoca uma reação. Para isso, as partículas precisam colidir em condições adequadas e com energia suficiente para superar a chamada energia de ativação.",
    "Se estiver frio demais, as colisões podem não ter energia suficiente. Mas também não adianta pensar 'então é só esquentar o máximo possível'.",
    "Temperaturas muito altas também podem causar problemas, como favorecer reações indesejadas ou até decompor algumas substâncias.",
    "Cada reação tem suas próprias condições. O que funciona para uma pode não funcionar para outra.",
    "Logo depois da câmara você vai encontrar um botão. Quando apertá-lo, as moléculas serão liberadas lá dentro e o experimento começa.",
    "Acima do botão tem uma barra que acompanha a temperatura da câmara. Nela vai haver uma faixa verde e é ali que queremos ficar.",
    "E quando eu digo ficar, é ficar mesmo! Use T e Y para controlar a temperatura e tente manter a barra dentro daquela faixa por alguns instantes.",
    "Se você conseguir sustentar a temperatura certa, as colisões terão a energia necessária para a reação acontecer.",
    "Forme produtos suficientes e os pistões que estão bloqueando o caminho vão se abrir. Aí você pode continuar!"
    };

    private string[] falasAtuais;

    private int falaAtual = 0;
    private int sequenciaAtual = 1;

    private bool dialogoAtivo = true;
    private bool escrevendo = false;
    private bool podeReceberInput = false;
    private Coroutine rotinaTexto;

    private float tempoMovimentoSequencia2 = 0f;
    private int ladoMovimentoAtual = 0;

    private bool sequencia1Concluida = false;
    private bool sequencia2Concluida = false;
    private bool sequencia3Concluida = false;

    private bool testouAumentar = false;
    private bool testouDiminuir = false;
    private bool sequencia2Ambos = false;

    private int frameLiberacaoTemperatura = -1;

    public bool DialogoAtivo => dialogoAtivo;

    public bool PodeAlterarTemperatura
    {
        get
        {
            return !dialogoAtivo &&
                   sequencia1Concluida &&
                   Time.frameCount > frameLiberacaoTemperatura;
        }
    }

    private IEnumerator Start()
    {
        falasAtuais = falasSequencia1;

        if (PularIntroducaoAoReiniciar)
        {
            PularIntroducaoAoReiniciar = false;
            dialogoAtivo = false;
            sequencia1Concluida = true;
            sequencia2Concluida = Sequencia2JaConcluida;
            frameLiberacaoTemperatura = Time.frameCount;
            conteudoDialogo.SetActive(false);
            yield break;
        }

        conteudoDialogo.SetActive(true);
        textoDialogo.text = "";

        yield return new WaitForSeconds(atrasoInicial);

        podeReceberInput = true;
        MostrarFala();
    }

    private void Update()
    {
        if (dialogoAtivo)
        {
            ProcessarInputDialogo();
            return;
        }

        DetectarTriggerSequencia2();
    }

    public void RegistrarAumentoTemperatura()
    {
    }

    public void RegistrarDiminuicaoTemperatura()
    {
    }

    public void DispararSequencia3()
    {
        if (dialogoAtivo || sequencia3Concluida)
            return;

        sequencia3Concluida = true;
        sequenciaAtual = 3;
        falasAtuais = falasSequencia3;
        falaAtual = 0;

        dialogoAtivo = true;
        escrevendo = false;
        podeReceberInput = true;

        conteudoDialogo.SetActive(true);

        MostrarFala();
    }

    private void ProcessarInputDialogo()
    {
        if (!podeReceberInput || Keyboard.current == null)
            return;

        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            EncerrarDialogo();
            return;
        }

        if (Keyboard.current.yKey.wasPressedThisFrame)
        {
            if (escrevendo)
                CompletarFala();
            else
                AvancarDialogo();
        }
    }

    private void DetectarTriggerSequencia2()
    {
        if (!sequencia1Concluida || sequencia2Concluida)
            return;

        if (!PodeAlterarTemperatura)
            return;

        if (player == null || temperatureManager == null)
            return;

        int temperaturaAtual = temperatureManager.temperaturaAtual;
        int ladoAtual = 0;

        if (temperaturaAtual > temperaturaInicial)
            ladoAtual = 1;
        else if (temperaturaAtual < temperaturaInicial)
            ladoAtual = -1;

        if (ladoAtual == 0)
        {
            tempoMovimentoSequencia2 = 0f;
            ladoMovimentoAtual = 0;
            return;
        }

        if (ladoMovimentoAtual != 0 && ladoMovimentoAtual != ladoAtual)
        {
            tempoMovimentoSequencia2 = 0f;
        }

        ladoMovimentoAtual = ladoAtual;

        if (!player.EstaSeMovendoHorizontalmente)
            return;

        if (ladoAtual > 0)
            testouAumentar = true;
        else
            testouDiminuir = true;

        tempoMovimentoSequencia2 += Time.deltaTime;

        if (tempoMovimentoSequencia2 >= tempoParaSequencia2)
            DispararSequencia2();
    }

    private void DispararSequencia2()
    {
        sequencia2Concluida = true;
        Sequencia2JaConcluida = true;
        sequenciaAtual = 2;

        sequencia2Ambos = testouAumentar && testouDiminuir;

        if (sequencia2Ambos)
        {
            falasAtuais = falasSequencia2Ambos;
        }
        else if (testouAumentar)
        {
            falasAtuais = falasSequencia2Aquecimento;
        }
        else if (testouDiminuir)
        {
            falasAtuais = falasSequencia2Resfriamento;
        }
        else
        {
            return;
        }

        falaAtual = 0;
        tempoMovimentoSequencia2 = 0f;

        dialogoAtivo = true;
        escrevendo = false;
        podeReceberInput = true;

        conteudoDialogo.SetActive(true);

        MostrarFala();
    }

    private void MostrarFala()
    {
        AtualizarSprite();

        if (rotinaTexto != null)
            StopCoroutine(rotinaTexto);

        rotinaTexto = StartCoroutine(EscreverFala());
    }

    private void AtualizarSprite()
    {
        if (sequenciaAtual == 3)
        {
            if (falaAtual == 0 ||
                falaAtual == 6 ||
                falaAtual == 7 ||
                falaAtual == 8)
            {
                imagemRafael.sprite = rafaelApresentando;
                return;
            }

            if (falaAtual == 3 || falaAtual == 4)
            {
                imagemRafael.sprite = rafaelNervoso;
                return;
            }

            imagemRafael.sprite = rafaelExplicando;
            return;
        }

        if (sequenciaAtual == 2)
        {
            if (sequencia2Ambos && falaAtual == 0)
            {
                imagemRafael.sprite = rafaelOrgulhoso;
                return;
            }

            if (falaAtual == 3 || falaAtual == 5)
            {
                imagemRafael.sprite = rafaelApresentando;
                return;
            }

            imagemRafael.sprite = rafaelExplicando;
            return;
        }

        switch (falaAtual)
        {
            case 0:
                imagemRafael.sprite = rafaelSurpreso;
                break;

            case 1:
                imagemRafael.sprite = rafaelApresentando;
                break;

            case 2:
            case 3:
                imagemRafael.sprite = rafaelExplicando;
                break;

            case 4:
                imagemRafael.sprite = rafaelNervoso;
                break;

            case 5:
                imagemRafael.sprite = rafaelExplicando;
                break;
        }
    }

    private IEnumerator EscreverFala()
    {
        escrevendo = true;

        textoDialogo.text = falasAtuais[falaAtual];
        textoDialogo.maxVisibleCharacters = 0;

        textoDialogo.ForceMeshUpdate();

        int totalCaracteres = textoDialogo.textInfo.characterCount;

        for (int i = 0; i <= totalCaracteres; i++)
        {
            textoDialogo.maxVisibleCharacters = i;
            yield return new WaitForSeconds(velocidadeTexto);
        }

        escrevendo = false;
        rotinaTexto = null;
    }

    private void CompletarFala()
    {
        if (rotinaTexto != null)
        {
            StopCoroutine(rotinaTexto);
            rotinaTexto = null;
        }

        textoDialogo.text = falasAtuais[falaAtual];
        textoDialogo.maxVisibleCharacters = int.MaxValue;
        escrevendo = false;
    }

    private void AvancarDialogo()
    {
        falaAtual++;

        if (falaAtual >= falasAtuais.Length)
        {
            EncerrarDialogo();
            return;
        }

        MostrarFala();
    }

    private void EncerrarDialogo()
    {
        if (rotinaTexto != null)
        {
            StopCoroutine(rotinaTexto);
            rotinaTexto = null;
        }

        dialogoAtivo = false;
        podeReceberInput = false;
        escrevendo = false;

        if (sequenciaAtual == 1)
            sequencia1Concluida = true;

        frameLiberacaoTemperatura = Time.frameCount;
        conteudoDialogo.SetActive(false);
    }
}