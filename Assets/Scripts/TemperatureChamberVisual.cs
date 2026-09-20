using System.Collections.Generic;
using UnityEngine;

public class TemperatureChamberVisual : MoleculeContainer
{
    [Header("Moléculas")]
    [SerializeField] private MoleculeChamber redMoleculePrefab;
    [SerializeField] private MoleculeChamber blueMoleculePrefab;
    [SerializeField] private GameObject purpleMoleculePrefab;
    [SerializeField] private Transform moleculeContainer;
    [SerializeField] private Transform moleculeSpawnPoint;
    [SerializeField] private int moleculesPerType = 4;
    [SerializeField] private float launchSpread = 0.35f;

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
    private int purpleMoleculeCount;

    public float InternalTemperature
    {
        get
        {
            return internalTemperature;
        }
    }

    public int PurpleMoleculeCount
    {
        get
        {
            return purpleMoleculeCount;
        }
    }

    private void Start()
    {
        if (temperatureManager == null)
            return;

        internalTemperature =
            temperatureManager.TemperaturaCelsius;
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

    public void LiberarMoleculas()
    {
        if (redMoleculePrefab == null ||
            blueMoleculePrefab == null ||
            moleculeContainer == null ||
            moleculeSpawnPoint == null)
        {
            return;
        }

        for (int i = 0; i < moleculesPerType; i++)
        {
            SpawnMolecule(redMoleculePrefab);
            SpawnMolecule(blueMoleculePrefab);
        }

        UpdateMoleculeSpeed();
    }

    private void SpawnMolecule(MoleculeChamber prefab)
    {
        MoleculeChamber molecule = Instantiate(
            prefab,
            moleculeContainer
        );

        molecule.transform.position =
            moleculeSpawnPoint.position;

        Vector2 launchDirection = new Vector2(
            Random.Range(-launchSpread, launchSpread),
            -1f
        ).normalized;

        molecule.Initialize(
            this,
            launchDirection
        );

        molecules.Add(molecule);
    }

    public void CriarProduto(
        MoleculeChamber primeira,
        MoleculeChamber segunda)
    {
        if (primeira == null ||
            segunda == null ||
            purpleMoleculePrefab == null)
        {
            return;
        }

        if (purpleMoleculeCount >= moleculesPerType)
            return;

        Vector3 collisionPosition =
            (primeira.transform.position +
             segunda.transform.position) * 0.5f;

        molecules.Remove(primeira);
        molecules.Remove(segunda);

        Destroy(primeira.gameObject);
        Destroy(segunda.gameObject);

        GameObject purpleObject = Instantiate(
            purpleMoleculePrefab,
            collisionPosition,
            Quaternion.identity,
            moleculeContainer
        );

        MoleculeChamber purpleMolecule =
            purpleObject.GetComponent<MoleculeChamber>();

        if (purpleMolecule != null)
        {
            purpleMolecule.Initialize(this);
            molecules.Add(purpleMolecule);

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

            purpleMolecule.SetSpeedMultiplier(
                speedMultiplier
            );
        }

        purpleMoleculeCount++;

        Debug.Log(
            "Produto formado: " +
            purpleMoleculeCount +
            "/4"
        );

        if (purpleMoleculeCount >= moleculesPerType)
        {
            CollisionPuzzleController collisionPuzzle =
                GetComponent<CollisionPuzzleController>();

            if (collisionPuzzle != null)
                collisionPuzzle.ConcluirPuzzle();
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

        Gizmos.DrawWireCube(
            transform.position,
            size
        );
    }
}