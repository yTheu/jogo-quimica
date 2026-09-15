using UnityEngine;

public class RioCongelado : MonoBehaviour
{
    [SerializeField] private TemperatureManager temperatureManager;
    [SerializeField] private GameObject agua;
    [SerializeField] private GameObject gelo;

    private void Update()
    {
        if (temperatureManager == null || agua == null || gelo == null)
            return;

        bool congelado = temperatureManager.temperaturaAtual <= 3;

        agua.SetActive(!congelado);
        gelo.SetActive(congelado);
    }
}