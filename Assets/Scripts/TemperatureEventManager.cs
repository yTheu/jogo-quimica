using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TemperatureEventManager : MonoBehaviour
{
    [Header("Duração")]
    [SerializeField] private float eventDuration = 20f;
    [SerializeField] private float minimumTickTime = 3f;
    [SerializeField] private float maximumTickTime = 5f;
    [SerializeField] private float minimumTimeBetweenEvents = 8f;
    [SerializeField] private float maximumTimeBetweenEvents = 14f;

    [Header("Interface")]
    [SerializeField] private GameObject eventPanel;
    [SerializeField] private TMP_Text eventText;
    [SerializeField] private Image eventPanelBackground;

    [Header("Cores")]
    [SerializeField] private Color sunColor =
        new Color(1f, 0.55f, 0.1f, 0.85f);

    [SerializeField] private Color moonColor =
        new Color(0.2f, 0.45f, 1f, 0.85f);

    private EnergyController energyController;

    private void Awake()
    {
        energyController = GetComponent<EnergyController>();

        if (energyController == null)
        {
            Debug.LogError(
                "EnergyController não foi encontrado!"
            );
        }
    }

    private void Start()
    {
        eventPanel.SetActive(false);
        StartCoroutine(EventCycle());
    }

    private IEnumerator EventCycle()
    {
        yield return new WaitForSeconds(5f);

        // Apenas o primeiro evento será aleatório.
        bool nextEventIsSun = Random.value >= 0.5f;

        while (true)
        {
            yield return StartCoroutine(
                RunEvent(nextEventIsSun)
            );

            nextEventIsSun = !nextEventIsSun;

            float interval = Random.Range(
                minimumTimeBetweenEvents,
                maximumTimeBetweenEvents
            );

            yield return new WaitForSeconds(interval);
        }
    }

    private IEnumerator RunEvent(bool sunEvent)
    {
        eventPanel.SetActive(true);

        if (eventPanelBackground != null)
        {
            eventPanelBackground.color =
                sunEvent ? sunColor : moonColor;
        }

        float remainingTime = eventDuration;

        float timeUntilNextTick = Random.Range(
            minimumTickTime,
            maximumTickTime
        );

        while (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;
            timeUntilNextTick -= Time.deltaTime;

            UpdateEventText(
                sunEvent,
                remainingTime,
                timeUntilNextTick
            );

            if (timeUntilNextTick <= 0f)
            {
                int energyChange = sunEvent ? 1 : -1;

                energyController.ChangeEnergy(
                    energyChange
                );

                timeUntilNextTick = Random.Range(
                    minimumTickTime,
                    maximumTickTime
                );
            }

            yield return null;
        }

        eventPanel.SetActive(false);
    }

    private void UpdateEventText(
        bool sunEvent,
        float remainingTime,
        float timeUntilNextTick
    )
    {
        int remainingSeconds = Mathf.CeilToInt(
            Mathf.Max(0f, remainingTime)
        );

        int nextChangeSeconds = Mathf.CeilToInt(
            Mathf.Max(0f, timeUntilNextTick)
        );

        if (sunEvent)
        {
            eventText.text =
                $"SOL ATIVO\n" +
                $"+1 em em {nextChangeSeconds}s\n" +
                $"{remainingSeconds}s restantes";
        }
        else
        {
            eventText.text =
                $"LUA ATIVA\n" +
                $"-1 em {nextChangeSeconds}s\n" +
                $"{remainingSeconds}s restantes";
        }
    }
}