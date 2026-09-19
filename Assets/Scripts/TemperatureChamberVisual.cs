using System.Collections.Generic;
using UnityEngine;

public class TemperatureChamberVisual : MoleculeContainer
{
    [Header("Moléculas")]
    [SerializeField] private MoleculeChamber moleculePrefab;
    [SerializeField] private Transform moleculeContainer;
    [SerializeField] private int moleculeCount = 12;

    [Header("Área interna")]
    [SerializeField] private Vector2 chamberSize =
        new Vector2(5f, 4f);

    [SerializeField] private float moleculePadding = 0.2f;

    [Header("Temperatura")]
    [SerializeField] private TemperatureManager temperatureManager;
    [SerializeField] private float minimumSpeedMultiplier = 0.25f;
    [SerializeField] private float maximumSpeedMultiplier = 10f;
    [SerializeField] private float thermalResponseSpeed = 10f;

    private readonly List<MoleculeChamber> molecules = new();

    private float internalTemperature;

    public float InternalTemperature
    {
        get
        {
            return internalTemperature;
        }
    }

    private void Awake()
    {
        SpawnMolecules();
    }

    private void Start()
    {
        if (temperatureManager == null)
            return;

        internalTemperature =
            temperatureManager.TemperaturaCelsius;

        UpdateMoleculeSpeed();
    }

    private void Update()
    {
        if (temperatureManager == null)
            return;

        float targetTemperature =
            temperatureManager.TemperaturaCelsius;

        internalTemperature = Mathf.MoveTowards(
            internalTemperature,
            targetTemperature,
            thermalResponseSpeed * Time.deltaTime
        );

        UpdateMoleculeSpeed();
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

            MoleculeChamber molecule = Instantiate(
                moleculePrefab,
                moleculeContainer
            );

            molecule.transform.localPosition = position;
            molecule.Initialize(this);

            molecules.Add(molecule);
        }
    }

    private void UpdateMoleculeSpeed()
    {
        float normalizedTemperature = Mathf.InverseLerp(
            -25f,
            112.5f,
            internalTemperature
        );

        float speedMultiplier = Mathf.Lerp(
            minimumSpeedMultiplier,
            maximumSpeedMultiplier,
            normalizedTemperature
        );

        foreach (MoleculeChamber molecule in molecules)
        {
            if (molecule != null)
                molecule.SetSpeedMultiplier(speedMultiplier);
        }
    }

    public override void GetMovementLimits(
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
            moleculePadding;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        Vector3 size = new Vector3(
            chamberSize.x,
            chamberSize.y,
            0f
        );

        Gizmos.DrawWireCube(transform.position, size);
    }
}