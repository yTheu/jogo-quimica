using UnityEngine;

public class ExtremeTemperatureVisual : MonoBehaviour
{
    [SerializeField] private TemperatureManager temperatureManager;
    [SerializeField] private Rigidbody2D playerRb;

    [Header("Stretch")]
    [SerializeField] private float stretchHorizontal = 2.2f;
    [SerializeField] private float compressVertical = 0.45f;
    [SerializeField] private float stretchSpeed = 18f;

    [Header("Vibração")]
    [SerializeField] private float vibrationAmount = 0.09f;
    [SerializeField] private float vibrationSpeed = 55f;
    [SerializeField] private float scaleOscillation = 0.18f;

    private Vector3 escalaOriginal;
    private Vector3 posicaoOriginal;

    private void Start()
    {
        escalaOriginal = transform.localScale;
        posicaoOriginal = transform.localPosition;
    }

    private void LateUpdate()
    {
        if (temperatureManager == null || playerRb == null)
            return;

        int temperatura = temperatureManager.temperaturaAtual;

        if (temperatura < 11 || temperatura >= 15)
        {
            VoltarAoNormal();
            return;
        }

        float velocidadeX = Mathf.Abs(playerRb.linearVelocity.x);

        if (velocidadeX > 0.1f)
        {
            AplicarStretch(temperatura, velocidadeX);
        }
        else
        {
            AplicarVibracao(temperatura);
        }
    }

    private void AplicarStretch(int temperatura, float velocidade)
    {
        float intensidadeVelocidade = Mathf.InverseLerp(
            0f,
            15f,
            velocidade
        );

        float intensidadeTemperatura = Mathf.InverseLerp(
            11f,
            14f,
            temperatura
        );

        float intensidade = Mathf.Clamp01(
            0.65f +
            intensidadeVelocidade * 0.25f +
            intensidadeTemperatura * 0.25f
        );

        Vector3 escalaAlvo = new Vector3(
            escalaOriginal.x * Mathf.Lerp(
                1f,
                stretchHorizontal,
                intensidade
            ),
            escalaOriginal.y * Mathf.Lerp(
                1f,
                compressVertical,
                intensidade
            ),
            escalaOriginal.z
        );

        transform.localScale = Vector3.Lerp(
            transform.localScale,
            escalaAlvo,
            Time.deltaTime * stretchSpeed
        );

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            posicaoOriginal,
            Time.deltaTime * stretchSpeed
        );
    }

    private void AplicarVibracao(int temperatura)
    {
        float intensidadeTemperatura = Mathf.InverseLerp(
            11f,
            14f,
            temperatura
        );

        float intensidade = Mathf.Lerp(
            0.6f,
            1f,
            intensidadeTemperatura
        );

        float vibracaoX =
            Mathf.Sin(Time.time * vibrationSpeed) *
            vibrationAmount *
            intensidade;

        float vibracaoY =
            Mathf.Cos(Time.time * vibrationSpeed * 1.37f) *
            vibrationAmount *
            intensidade;

        transform.localPosition =
            posicaoOriginal +
            new Vector3(
                vibracaoX,
                vibracaoY,
                0f
            );

        float escalaX =
            1f +
            Mathf.Sin(Time.time * vibrationSpeed * 0.7f) *
            scaleOscillation *
            intensidade;

        float escalaY =
            1f +
            Mathf.Cos(Time.time * vibrationSpeed * 0.93f) *
            scaleOscillation *
            intensidade;

        transform.localScale = new Vector3(
            escalaOriginal.x * escalaX,
            escalaOriginal.y * escalaY,
            escalaOriginal.z
        );
    }

    private void VoltarAoNormal()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            escalaOriginal,
            Time.deltaTime * stretchSpeed
        );

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            posicaoOriginal,
            Time.deltaTime * stretchSpeed
        );
    }
}