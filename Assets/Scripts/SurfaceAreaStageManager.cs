using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurfaceAreaStageManager : MonoBehaviour
{
    [Header("Objetivo")]
    [SerializeField] private int totalTablets = 3;
    [SerializeField] private int fragmentsRequiredPerTablet = 5;

    [Header("Interface")]
    [SerializeField] private Image[] indicators;

    [SerializeField] private Color inactiveColor = new Color(0.25f, 0.3f, 0.4f, 1f);

    [SerializeField] private Color activeColor = new Color(0.2f, 1f, 0.45f, 1f);

    [Header("Reação visual")]
    [SerializeField] private ParticleSystem bubbles;

    [Header("Saída")]
    [SerializeField] private SlidingMechanism finalDoor;
    [SerializeField] private NextLevelDoor nextLevelDoor;

    private readonly Dictionary<int, int> fragmentCounts =
        new Dictionary<int, int>();

    private readonly HashSet<int> processedTablets =
        new HashSet<int>();

    private void Start()
    {
        for (int i = 0; i < indicators.Length; i++)
        {
            if (indicators[i] != null)
                indicators[i].color = inactiveColor;
        }

        if (finalDoor != null)
            finalDoor.SetOpen(false);
    }

    public void DepositFragment(int tabletId)
    {
        if (processedTablets.Contains(tabletId))
            return;

        if (!fragmentCounts.ContainsKey(tabletId))
            fragmentCounts[tabletId] = 0;

        fragmentCounts[tabletId]++;

        UpdateReactionVisual();

        if (fragmentCounts[tabletId] >= fragmentsRequiredPerTablet)
            CompleteTablet(tabletId);
    }

    private void CompleteTablet(int tabletId)
    {
        if (!processedTablets.Add(tabletId))
            return;

        if (tabletId >= 0 && tabletId < indicators.Length)
        {
            if (indicators[tabletId] != null)
                indicators[tabletId].color = activeColor;
        }

        if (processedTablets.Count >= totalTablets)
            CompleteStage();
    }

    private void UpdateReactionVisual()
    {
        if (bubbles == null)
            return;

        var emission = bubbles.emission;
        emission.rateOverTime =
            3f + processedTablets.Count * 8f;
    }

    private void CompleteStage()
    {
        if (finalDoor != null)
            finalDoor.SetOpen(true);

        if (nextLevelDoor != null)
            nextLevelDoor.Unlock();
    }
}