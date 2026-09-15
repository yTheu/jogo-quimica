using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlataformaQueCai : MonoBehaviour
{
    [SerializeField] private float tempoParaCair = 0.7f;
    [SerializeField] private float tempoParaVoltar = 2f;
    [SerializeField] private float intensidadeTremor = 0.03f;

    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer spriteRenderer;

    private Vector3 posicaoInicial;
    private Quaternion rotacaoInicial;

    private bool ativada = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        posicaoInicial = transform.position;
        rotacaoInicial = transform.rotation;

        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (ativada)
            return;

        if (!collision.gameObject.CompareTag("Player"))
            return;

        ativada = true;
        StartCoroutine(SequenciaQueda());
    }

    private IEnumerator SequenciaQueda()
    {
        float intervaloPisca = 0.12f;
        float tempoPiscando = intervaloPisca * 4f;

        for (int i = 0; i < 2; i++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(intervaloPisca);

            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(intervaloPisca);
        }

        float tempoRestante = tempoParaCair - tempoPiscando;

        while (tempoRestante > 0f)
        {
            transform.position = posicaoInicial + new Vector3(
                Random.Range(-intensidadeTremor, intensidadeTremor),
                Random.Range(-intensidadeTremor, intensidadeTremor),
                0f
            );

            tempoRestante -= Time.deltaTime;
            yield return null;
        }

        transform.position = posicaoInicial;

        rb.bodyType = RigidbodyType2D.Dynamic;

        yield return new WaitForSeconds(tempoParaVoltar);

        spriteRenderer.enabled = false;
        col.enabled = false;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;

        transform.position = posicaoInicial;
        transform.rotation = rotacaoInicial;

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.enabled = true;
        col.enabled = true;

        ativada = false;
    }
}