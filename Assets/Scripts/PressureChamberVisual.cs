using System.Collections.Generic;
using UnityEngine;

public class PressureChamberVisual : MonoBehaviour
{
    [Header("Moléculas")]
    [SerializeField] private ChamberMolecule moleculePrefab;
    [SerializeField] private Transform moleculeContainer;
    [SerializeField] private int moleculeCount = 12;

    [Header("Área interna")]
    [SerializeField] private Vector2 chamberSize =
        new Vector2(4f, 4f);

    [SerializeField] private float moleculePadding = 0.2f;

    [Header("Pistões")]
    [SerializeField] private Transform[] pistons;
    [SerializeField] private float maximumPistonDescent = 1.8f;
    [SerializeField] private float pistonMovementSpeed = 2f;

    [Header("Velocidade visual")]
    [SerializeField] private float maximumSpeedMultiplier = 1f;

    private readonly List<ChamberMolecule> molecules = new();
    private Vector3[] initialPistonPositions;

    private int pressureLevel;
    private float currentCompression;
    private float targetCompression;

    private void Awake()
    {
        SavePistonPositions();
        SpawnMolecules();
        SetPressureLevel(0);
    }

    private void Update()
    {
        currentCompression = Mathf.MoveTowards(
            currentCompression,
            targetCompression,
            pistonMovementSpeed * Time.deltaTime
        );

        UpdatePistons();
    }

    private void SavePistonPositions()
    {
        initialPistonPositions =
            new Vector3[pistons.Length];

        for (int i = 0; i < pistons.Length; i++)
        {
            if (pistons[i] != null)
            {
                initialPistonPositions[i] =
                    pistons[i].localPosition;
            }
        }
    }

    private void SpawnMolecules()
    {
        if (moleculePrefab == null ||
            moleculeContainer == null)
        {
            return;
        }

        for (int i = 0; i < moleculeCount; i++)
        {
            GetMovementLimits(
                out float minimumX,
                out float maximumX,
                out float minimumY,
                out float maximumY
            );

            Vector3 position = new Vector3(
                Random.Range(minimumX, maximumX),
                Random.Range(minimumY, maximumY),
                0f
            );

            ChamberMolecule molecule = Instantiate(
                moleculePrefab,
                moleculeContainer
            );

            molecule.transform.localPosition = position;
            molecule.Initialize(this);

            molecules.Add(molecule);
        }
    }

    public void SetPressureLevel(int level)
    {
        pressureLevel = Mathf.Clamp(level, 0, 3);

        float normalizedPressure =
            pressureLevel / 3f;

        targetCompression =
            maximumPistonDescent *
            normalizedPressure;

        float speedMultiplier = Mathf.Lerp(
            1f,
            maximumSpeedMultiplier,
            normalizedPressure
        );

        foreach (ChamberMolecule molecule in molecules)
        {
            if (molecule != null)
                molecule.SetSpeedMultiplier(speedMultiplier);
        }
    }

    private void UpdatePistons()
    {
        for (int i = 0; i < pistons.Length; i++)
        {
            if (pistons[i] == null)
                continue;

            Vector3 targetPosition =
                initialPistonPositions[i] +
                Vector3.down * currentCompression;

            pistons[i].localPosition = targetPosition;
        }
    }

    public void GetMovementLimits(
        out float minimumX,
        out float maximumX,
        out float minimumY,
        out float maximumY)
    {
        minimumX =
            -chamberSize.x / 2f +
            moleculePadding;

        maximumX =
            chamberSize.x / 2f -
            moleculePadding;

        minimumY =
            -chamberSize.y / 2f +
            moleculePadding;

        maximumY =
            chamberSize.y / 2f -
            moleculePadding -
            currentCompression;

        if (maximumY < minimumY)
            maximumY = minimumY;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        Vector3 center = transform.position +
                         Vector3.down *
                         currentCompression *
                         0.5f;

        Vector3 size = new Vector3(
            chamberSize.x,
            chamberSize.y - currentCompression,
            0f
        );

        Gizmos.DrawWireCube(center, size);
    }
}