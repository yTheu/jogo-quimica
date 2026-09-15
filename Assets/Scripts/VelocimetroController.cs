using UnityEngine;

public class VelocimetroController : MonoBehaviour
{
    [SerializeField] private Transform pivoPonteiro;
    [SerializeField] private Rigidbody2D playerRb;

    [SerializeField] private float anguloMinimo = 155f;
    [SerializeField] private float anguloMaximo = -55f;
    [SerializeField] private float velocidadeMaxima = 15f;
    [SerializeField] private float velocidadePonteiro = 250f;

    [Header("Lasers")]
    [SerializeField] private LaserController[] lasers;
    [SerializeField] private float velocidadeSegura = 3.3f;

    private void Start()
    {
        pivoPonteiro.localRotation = Quaternion.Euler(
            0f,
            0f,
            anguloMinimo
        );
    }

    private void Update()
    {
        float velocidade = Mathf.Abs(playerRb.linearVelocity.x);

        float percentual = Mathf.InverseLerp(
            0f,
            velocidadeMaxima,
            velocidade
        );

        float anguloAlvo = Mathf.Lerp(
            anguloMinimo,
            anguloMaximo,
            percentual
        );

        float anguloAtual = pivoPonteiro.localEulerAngles.z;

        if (anguloAtual > 180f)
            anguloAtual -= 360f;

        float novoAngulo = Mathf.MoveTowards(
            anguloAtual,
            anguloAlvo,
            velocidadePonteiro * Time.deltaTime
        );

        pivoPonteiro.localRotation = Quaternion.Euler(
            0f,
            0f,
            novoAngulo
        );

        if (velocidade > velocidadeSegura)
        {
            foreach (LaserController laser in lasers)
            {
                laser.Ativar();
            }
        }
        else
        {
            foreach (LaserController laser in lasers)
            {
                laser.Desativar();
            }
        }
    }
}