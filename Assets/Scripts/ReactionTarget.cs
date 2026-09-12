using UnityEngine;

public class ReactionTarget : MonoBehaviour
{
    private EnergyController energyController;
    private LevelManager levelManager;

    private bool reactionCompleted;

    private void Awake()
    {
        energyController =
            FindAnyObjectByType<EnergyController>();

        levelManager =
            FindAnyObjectByType<LevelManager>();

        if (energyController == null)
        {
            Debug.LogError(
                "EnergyController não foi encontrado!"
            );
        }

        if (levelManager == null)
        {
            Debug.LogError(
                "LevelManager não foi encontrado!"
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (reactionCompleted)
            return;

        PlayerController player =
            other.GetComponentInParent<PlayerController>();

        if (player == null)
            return;

        if (!player.IsDashing)
        {
            Debug.Log(
                "É necessário usar o dash para realizar a colisão."
            );

            return;
        }

        if (!energyController.HasExactEnergy())
        {
            Debug.Log(
                $"Colisão não efetiva. Energia atual: " +
                $"{energyController.CurrentEnergy}. " +
                $"Energia necessária: 10."
            );

            return;
        }

        reactionCompleted = true;
        levelManager.ShowVictory();
    }
}