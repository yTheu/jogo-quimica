using UnityEngine;

public class PlataformaElevador : MonoBehaviour
{
    public enum DirecaoEmpurrao
    {
        Esquerda,
        Direita
    }

    [SerializeField] private Transform pontoSuperior;
    [SerializeField] private float velocidade = 3f;

    [Header("Protecao contra esmagamento")]
    [SerializeField] private DirecaoEmpurrao direcaoEmpurrao;
    [SerializeField] private float velocidadeEmpurrao = 8f;

    private Vector3 posicaoInicial;
    private bool jogadorEmCima;
    private Transform jogadorEmbaixo;

    private void Start()
    {
        posicaoInicial = transform.position;
    }

    private void Update()
    {
        Vector3 destino = jogadorEmCima
            ? pontoSuperior.position
            : posicaoInicial;

        transform.position = Vector3.MoveTowards(
            transform.position,
            destino,
            velocidade * Time.deltaTime
        );

        EmpurrarJogador();
    }

    private void EmpurrarJogador()
    {
        if (jogadorEmbaixo == null)
            return;

        float direcao = direcaoEmpurrao == DirecaoEmpurrao.Direita
            ? 1f
            : -1f;

        jogadorEmbaixo.position += Vector3.right *
                                  direcao *
                                  velocidadeEmpurrao *
                                  Time.deltaTime;
    }

    public void PlayerEntrouEmbaixo(Transform player)
    {
        jogadorEmbaixo = player;
    }

    public void PlayerSaiuDeBaixo(Transform player)
    {
        if (jogadorEmbaixo == player)
            jogadorEmbaixo = null;
    }

    public void JogadorEntrou()
    {
        jogadorEmCima = true;
    }

    public void JogadorSaiu()
    {
        jogadorEmCima = false;
    }
}