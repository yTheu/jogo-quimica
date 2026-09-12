using UnityEngine;

public class ValveChallengeManager : MonoBehaviour
{
    [SerializeField] private TimedValve[] valves;
    [SerializeField] private SlidingMechanism challengeDoor;

    public bool ChallengeCompleted { get; private set; }

    public void CheckValves()
    {
        if (ChallengeCompleted)
            return;

        if (valves == null || valves.Length == 0)
            return;

        foreach (TimedValve valve in valves)
        {
            if (valve == null || !valve.IsActive)
                return;
        }

        CompleteChallenge();
    }

    private void CompleteChallenge()
    {
        ChallengeCompleted = true;

        if (challengeDoor != null)
            challengeDoor.SetOpen(true);

        Debug.Log(
            "Desafio das válvulas concluído!"
        );
    }
}