using System.Collections;
using UnityEngine;

public class BotaoPorta : MonoBehaviour
{
    [SerializeField] private PistaoPorta pistaoPorta;
    [SerializeField] private float tempoAberto = 4f;

    [Header("Animacao do Botao")]
    [SerializeField] private float distanciaAfundar = 0.15f;
    [SerializeField] private float velocidadeBotao = 5f;

    private bool ativado;

    private Vector3 posicaoNormal;
    private Vector3 posicaoPressionada;
    private Vector3 posicaoAlvo;

    private void Start()
    {
        posicaoNormal = transform.localPosition;
        posicaoPressionada = posicaoNormal + Vector3.down * distanciaAfundar;
        posicaoAlvo = posicaoNormal;
    }

    private void Update()
    {
        transform.localPosition = Vector3.MoveTowards(
            transform.localPosition,
            posicaoAlvo,
            velocidadeBotao * Time.deltaTime
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (ativado)
            return;

        StartCoroutine(AtivarPorta());
    }

    private IEnumerator AtivarPorta()
    {
        ativado = true;

        posicaoAlvo = posicaoPressionada;
        pistaoPorta.Abrir();

        yield return new WaitForSeconds(tempoAberto);

        pistaoPorta.Fechar();
        posicaoAlvo = posicaoNormal;

        ativado = false;
    }
}