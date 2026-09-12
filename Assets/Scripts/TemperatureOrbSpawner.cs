using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureOrbSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject hotOrbPrefab;
    [SerializeField] private GameObject coldOrbPrefab;

    [Header("Pontos de surgimento")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform orbContainer;

    [Header("Quantidade")]
    [SerializeField] private int initialOrbCount = 6;
    [SerializeField] private int maximumActiveOrbs = 10;

    [Header("Tempo de reaparecimento")]
    [SerializeField] private float minimumRespawnTime = 3f;
    [SerializeField] private float maximumRespawnTime = 6f;

    [Header("Probabilidade")]
    [SerializeField]
    [Range(0f, 1f)]
    private float hotOrbChance = 0.55f;

    private readonly List<GameObject> activeOrbs =
        new List<GameObject>();

    private void Start()
    {
        int amountToCreate = Mathf.Min(
            initialOrbCount,
            maximumActiveOrbs
        );

        for (int i = 0; i < amountToCreate; i++)
        {
            SpawnRandomOrb();
        }

        StartCoroutine(RespawnCycle());
    }

    private IEnumerator RespawnCycle()
    {
        while (true)
        {
            float respawnTime = Random.Range(
                minimumRespawnTime,
                maximumRespawnTime
            );

            yield return new WaitForSeconds(respawnTime);

            RemoveDestroyedOrbs();

            if (activeOrbs.Count < maximumActiveOrbs)
            {
                SpawnRandomOrb();
            }
        }
    }

    private void SpawnRandomOrb()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning(
                "Nenhum ponto de surgimento foi configurado."
            );

            return;
        }

        List<Transform> availablePoints =
            GetAvailableSpawnPoints();

        if (availablePoints.Count == 0)
            return;

        Transform selectedPoint = availablePoints[
            Random.Range(0, availablePoints.Count)
        ];

        bool createHotOrb =
            Random.value <= hotOrbChance;

        GameObject selectedPrefab =
            createHotOrb
                ? hotOrbPrefab
                : coldOrbPrefab;

        if (selectedPrefab == null)
        {
            Debug.LogError(
                "O prefab da esfera não foi atribuído."
            );

            return;
        }

        GameObject newOrb = Instantiate(
            selectedPrefab,
            selectedPoint.position,
            Quaternion.identity,
            orbContainer
        );

        activeOrbs.Add(newOrb);
    }

    private List<Transform> GetAvailableSpawnPoints()
    {
        List<Transform> availablePoints =
            new List<Transform>();

        foreach (Transform point in spawnPoints)
        {
            if (point == null)
                continue;

            bool occupied = false;

            foreach (GameObject orb in activeOrbs)
            {
                if (orb == null)
                    continue;

                float distance = Vector2.Distance(
                    orb.transform.position,
                    point.position
                );

                if (distance < 0.5f)
                {
                    occupied = true;
                    break;
                }
            }

            if (!occupied)
            {
                availablePoints.Add(point);
            }
        }

        return availablePoints;
    }

    private void RemoveDestroyedOrbs()
    {
        activeOrbs.RemoveAll(orb => orb == null);
    }
}