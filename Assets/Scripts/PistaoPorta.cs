using UnityEngine;

public class PistaoPorta : MonoBehaviour
{
    [SerializeField] private Transform pistaoSuperior;
    [SerializeField] private Transform pistaoInferior;

    [SerializeField] private float escalaAberta = 0.15f;
    [SerializeField] private float velocidadeRetracao = 3f;

    private Vector3 escalaOriginalSuperior;
    private Vector3 escalaOriginalInferior;

    private bool aberto;

    private void Start()
    {
        escalaOriginalSuperior = pistaoSuperior.localScale;
        escalaOriginalInferior = pistaoInferior.localScale;
    }

    private void Update()
    {
        Vector3 alvoSuperior = escalaOriginalSuperior;
        Vector3 alvoInferior = escalaOriginalInferior;

        if (aberto)
        {
            alvoSuperior.y = escalaOriginalSuperior.y * escalaAberta;
            alvoInferior.y = escalaOriginalInferior.y * escalaAberta;
        }

        pistaoSuperior.localScale = Vector3.MoveTowards(
            pistaoSuperior.localScale,
            alvoSuperior,
            velocidadeRetracao * Time.deltaTime
        );

        pistaoInferior.localScale = Vector3.MoveTowards(
            pistaoInferior.localScale,
            alvoInferior,
            velocidadeRetracao * Time.deltaTime
        );
    }

    public void Abrir()
    {
        aberto = true;
    }

    public void Fechar()
    {
        aberto = false;
    }
}