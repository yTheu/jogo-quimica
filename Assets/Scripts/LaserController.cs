using UnityEngine;

public class LaserController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer emissor;
    [SerializeField] private Sprite emissorDesligado;
    [SerializeField] private Sprite emissorLigado;
    [SerializeField] private GameObject feixe;

    [Header("Laser")]
    [SerializeField] private float velocidadeDescida = 4f;

    private Vector3 escalaOriginalFeixe;
    private bool ativo;

    private void Start()
    {
        escalaOriginalFeixe = feixe.transform.localScale;

        Vector3 escalaInicial = escalaOriginalFeixe;
        escalaInicial.y = 0f;

        feixe.transform.localScale = escalaInicial;
        feixe.SetActive(false);

        emissor.sprite = emissorDesligado;
    }

    private void Update()
    {
        if (!ativo)
            return;

        feixe.transform.localScale = Vector3.MoveTowards(
            feixe.transform.localScale,
            escalaOriginalFeixe,
            velocidadeDescida * Time.deltaTime
        );
    }

    public void Ativar()
    {
        if (ativo)
            return;

        ativo = true;
        emissor.sprite = emissorLigado;
        feixe.SetActive(true);
    }

    public void Desativar()
    {
        if (!ativo)
            return;

        ativo = false;
        emissor.sprite = emissorDesligado;

        Vector3 escalaInicial = escalaOriginalFeixe;
        escalaInicial.y = 0f;

        feixe.transform.localScale = escalaInicial;
        feixe.SetActive(false);
    }
}