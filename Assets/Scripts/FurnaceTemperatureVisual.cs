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

        estadoFrio.SetActive(temperatura >= 0 && temperatura <= 3);
        estadoNormal.SetActive(temperatura >= 4 && temperatura <= 6);
        estadoQuente.SetActive(temperatura >= 7 && temperatura <= 10);
        estadoExtremo.SetActive(temperatura >= 11 && temperatura <= 14);

        if (temperatura >= 15)
        {
            estadoFrio.SetActive(false);
            estadoNormal.SetActive(false);
            estadoQuente.SetActive(false);
            estadoExtremo.SetActive(false);

            Debug.Log("FORNALHA EXPLODIU");
        }
    }
}