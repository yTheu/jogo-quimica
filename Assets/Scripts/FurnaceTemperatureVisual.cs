using UnityEngine;

public class FurnaceTemperatureVisual : MonoBehaviour
{
    public TemperatureManager temperatureManager;

    public GameObject estadoFrio;
    public GameObject estadoNormal;
    public GameObject estadoQuente;
    public GameObject estadoExtremo;

    void Update()
    {
        int temperatura = temperatureManager.temperaturaAtual;

        estadoFrio.SetActive(temperatura >= 0 && temperatura <= 2);
        estadoNormal.SetActive(temperatura >= 3 && temperatura <= 5);
        estadoQuente.SetActive(temperatura >= 6 && temperatura <= 10);
        estadoExtremo.SetActive(temperatura == 11);

        if (temperatura >= 12)
        {
            estadoFrio.SetActive(false);
            estadoNormal.SetActive(false);
            estadoQuente.SetActive(false);
            estadoExtremo.SetActive(false);

            Debug.Log("FORNALHA EXPLODIU");
        }
    }
}