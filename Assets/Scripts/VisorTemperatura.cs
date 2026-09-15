using UnityEngine;

public class VisorTemperatura : MonoBehaviour
{
    [SerializeField] private TemperatureManager temperatureManager;

    [SerializeField] private SpriteRenderer digitoEsquerdo;
    [SerializeField] private SpriteRenderer digitoDireito;

    [SerializeField] private Sprite[] numeros;

    private void Update()
    {
        int temperatura = temperatureManager.temperaturaAtual;

        int dezena = temperatura / 10;
        int unidade = temperatura % 10;

        digitoEsquerdo.sprite = numeros[dezena];
        digitoDireito.sprite = numeros[unidade];
    }
}