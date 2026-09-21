using UnityEngine;

public class PurpleMoleculePulse : MonoBehaviour
{
    [SerializeField] private SpriteRenderer auraRed;
    [SerializeField] private SpriteRenderer auraBlue;
    [SerializeField] private float duracaoPulso = 0.8f;
    [SerializeField] private float escalaInicial = 0.8f;
    [SerializeField] private float escalaFinal = 1.15f;
    [SerializeField] private float intervalo = 0.15f;

    private Vector3 escalaBaseRed;
    private Vector3 escalaBaseBlue;
    private float tempo;
    private bool pulsoVermelho;

    private void Awake()
    {
        if (auraRed != null)
            escalaBaseRed = auraRed.transform.localScale;

        if (auraBlue != null)
            escalaBaseBlue = auraBlue.transform.localScale;

        float duracaoCiclo = duracaoPulso + intervalo;

        tempo = Random.Range(0f, duracaoCiclo);
        pulsoVermelho = Random.value > 0.5f;
    }

    private void Update()
    {
        if (auraRed == null || auraBlue == null)
            return;

        tempo += Time.deltaTime;

        if (tempo <= duracaoPulso)
        {
            float progresso = tempo / duracaoPulso;

            if (pulsoVermelho)
            {
                AtualizarAura(
                    auraRed,
                    escalaBaseRed,
                    progresso
                );

                EsconderAura(
                    auraBlue,
                    escalaBaseBlue
                );
            }
            else
            {
                AtualizarAura(
                    auraBlue,
                    escalaBaseBlue,
                    progresso
                );

                EsconderAura(
                    auraRed,
                    escalaBaseRed
                );
            }

            return;
        }

        EsconderAura(auraRed, escalaBaseRed);
        EsconderAura(auraBlue, escalaBaseBlue);

        if (tempo >= duracaoPulso + intervalo)
        {
            tempo = 0f;
            pulsoVermelho = !pulsoVermelho;
        }
    }

    private void AtualizarAura(
        SpriteRenderer aura,
        Vector3 escalaBase,
        float progresso)
    {
        float escala = Mathf.Lerp(
            escalaInicial,
            escalaFinal,
            progresso
        );

        aura.transform.localScale =
            escalaBase * escala;

        Color cor = aura.color;
        cor.a = 1f - progresso;
        aura.color = cor;
    }

    private void EsconderAura(
        SpriteRenderer aura,
        Vector3 escalaBase)
    {
        aura.transform.localScale =
            escalaBase * escalaInicial;

        Color cor = aura.color;
        cor.a = 0f;
        aura.color = cor;
    }
}