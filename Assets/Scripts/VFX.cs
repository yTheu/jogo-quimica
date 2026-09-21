using System.Collections;
using UnityEngine;

public class EffectiveCollisionVFX : MonoBehaviour
{
    [SerializeField] private SpriteRenderer flash;
    [SerializeField] private float escalaInicial = 0.35f;
    [SerializeField] private float escalaFinal = 1f;
    [SerializeField] private float duracao = 0.18f;

    private Vector3 escalaBase;

    private void Awake()
    {
        escalaBase = transform.localScale;
    }

    private void Start()
    {
        StartCoroutine(Animar());
    }

    private IEnumerator Animar()
    {
        float tempo = 0f;

        transform.localScale =
            escalaBase * escalaInicial;

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;

            float progresso =
                Mathf.Clamp01(tempo / duracao);

            float escala = Mathf.Lerp(
                escalaInicial,
                escalaFinal,
                progresso
            );

            transform.localScale =
                escalaBase * escala;

            if (flash != null)
            {
                Color cor = flash.color;
                cor.a = 1f - progresso;
                flash.color = cor;
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}