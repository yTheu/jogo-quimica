using System.Collections;
using UnityEngine;

public class ReleaseButton : MonoBehaviour
{
    [SerializeField] private CollisionPuzzleController collisionPuzzle;

    [Header("Animacao do Botao")]
    [SerializeField] private float distanciaAfundar = 0.15f;
    [SerializeField] private float velocidadeBotao = 5f;
    [SerializeField] private float tempoPressionado = 0.3f;

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

        StartCoroutine(Pressionar());
    }

    private IEnumerator Pressionar()
    {
        ativado = true;

        posicaoAlvo = posicaoPressionada;

        if (collisionPuzzle != null)
            collisionPuzzle.LiberarCarga();

        yield return new WaitForSeconds(tempoPressionado);

        posicaoAlvo = posicaoNormal;

        ativado = false;
    }
}