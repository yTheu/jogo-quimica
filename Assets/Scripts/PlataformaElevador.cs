using UnityEngine;

public class PlataformaElevador : MonoBehaviour
{
    [SerializeField] private Transform pontoSuperior;
    [SerializeField] private float velocidade = 3f;

    private Vector3 posicaoInicial;
    private bool jogadorEmCima;

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