using UnityEngine;

public class EfeitoFrioController : MonoBehaviour
{
    [SerializeField] private TemperatureManager temperatureManager;
    [SerializeField] private Material materialBordas;

    [SerializeField] private float velocidadePulsacao = 1.5f;

    private void Update()
    {
        float intensidadeMinima;
        float intensidadeMaxima;

        if (temperatureManager.temperaturaAtual == 2)
        {
            intensidadeMinima = 0.12f;
            intensidadeMaxima = 0.30f;
        }
        else if (temperatureManager.temperaturaAtual == 1)
        {
            intensidadeMinima = 0.22f;
            intensidadeMaxima = 0.48f;
        }
        else if (temperatureManager.temperaturaAtual == 0)
        {
            intensidadeMinima = 0.32f;
            intensidadeMaxima = 0.65f;
        }
        else
        {
            materialBordas.SetFloat("_Intensidade", 0f);
            return;
        }

        float pulsacao = Mathf.PingPong(Time.time * velocidadePulsacao, 1f);
        float intensidadeFinal = Mathf.Lerp(intensidadeMinima, intensidadeMaxima, pulsacao);

        materialBordas.SetFloat("_Intensidade", intensidadeFinal);
    }
}