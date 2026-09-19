using UnityEngine;

public class VisorTemperatura : MonoBehaviour
{
    [SerializeField] private TemperatureManager temperatureManager;

    [SerializeField] private SpriteRenderer posicao1;
    [SerializeField] private SpriteRenderer posicao2;
    [SerializeField] private SpriteRenderer posicao3;
    [SerializeField] private SpriteRenderer posicao4;
    [SerializeField] private SpriteRenderer posicao5;

    [SerializeField] private Sprite[] numeros;
    [SerializeField] private Sprite virgula;
    [SerializeField] private Sprite menos;

    private SpriteRenderer[] posicoes;

    private void Awake()
    {
        posicoes = new SpriteRenderer[]
        {
            posicao1,
            posicao2,
            posicao3,
            posicao4,
            posicao5
        };
    }

    private void Update()
    {
        float temperatura = temperatureManager.TemperaturaCelsius;
        string texto = temperatura.ToString("0.0").Replace(".", ",");

        for (int i = 0; i < posicoes.Length; i++)
        {
            posicoes[i].sprite = null;
        }

        int inicio = posicoes.Length - texto.Length;

        for (int i = 0; i < texto.Length; i++)
        {
            char caractere = texto[i];
            Sprite sprite = null;

            if (char.IsDigit(caractere))
            {
                sprite = numeros[caractere - '0'];
            }
            else if (caractere == ',')
            {
                sprite = virgula;
            }
            else if (caractere == '-')
            {
                sprite = menos;
            }

            posicoes[inicio + i].sprite = sprite;
        }
    }
}