using System.Collections;
using UnityEngine;

public class MoleculeChamber : MonoBehaviour
{
    public enum MoleculeType
    {
        Red,
        Blue,
        Purple
    }

    [SerializeField] private MoleculeType moleculeType;
    [SerializeField] private float minimumSpeed = 0.8f;
    [SerializeField] private float maximumSpeed = 1.3f;

    [Header("Movimento Aleatorio")]
    [SerializeField] private float minimumDirectionChangeTime = 0.6f;
    [SerializeField] private float maximumDirectionChangeTime = 1.5f;
    [SerializeField] private float directionVariation = 50f;

    [Header("Impacto")]
    [SerializeField] private float squashAmount = 0.75f;
    [SerializeField] private float stretchAmount = 1.15f;
    [SerializeField] private float squashDuration = 0.06f;
    [SerializeField] private float recoveryDuration = 0.08f;

    private MoleculeContainer container;
    private CollisionPuzzleController collisionPuzzle;
    private Vector2 direction;
    private float baseSpeed;
    private float speedMultiplier = 1f;
    private float directionChangeTimer;
    private Vector3 originalScale;
    private Coroutine squashCoroutine;
    private bool reagiu;

    public MoleculeType Type
    {
        get
        {
            return moleculeType;
        }
    }

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    public void Initialize(MoleculeContainer moleculeContainer)
    {
        container = moleculeContainer;
        collisionPuzzle =
            moleculeContainer.GetComponent<CollisionPuzzleController>();

        direction = Random.insideUnitCircle.normalized;

        if (direction == Vector2.zero)
            direction = Vector2.right;

        baseSpeed = Random.Range(
            minimumSpeed,
            maximumSpeed
        );

        ResetDirectionChangeTimer();
    }

    public void Initialize(
        MoleculeContainer moleculeContainer,
        Vector2 initialDirection)
    {
        container = moleculeContainer;
        collisionPuzzle =
            moleculeContainer.GetComponent<CollisionPuzzleController>();

        direction = initialDirection.normalized;

        if (direction == Vector2.zero)
            direction = Vector2.down;

        baseSpeed = Random.Range(
            minimumSpeed,
            maximumSpeed
        );

        ResetDirectionChangeTimer();
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }

    public void MarcarComoReagida()
    {
        reagiu = true;
    }

    private void Update()
    {
        if (container == null || reagiu)
            return;

        UpdateRandomDirection();

        Vector3 position = transform.localPosition;

        position += (Vector3)(
            direction *
            baseSpeed *
            speedMultiplier *
            Time.deltaTime
        );

        container.GetMovementLimits(
            out float minimumX,
            out float maximumX,
            out float minimumY,
            out float maximumY
        );

        if (position.x <= minimumX ||
            position.x >= maximumX)
        {
            direction.x *= -1f;

            position.x = Mathf.Clamp(
                position.x,
                minimumX,
                maximumX
            );
        }

        if (position.y <= minimumY ||
            position.y >= maximumY)
        {
            direction.y *= -1f;

            position.y = Mathf.Clamp(
                position.y,
                minimumY,
                maximumY
            );
        }

        transform.localPosition = position;
    }

    private void UpdateRandomDirection()
    {
        directionChangeTimer -= Time.deltaTime;

        if (directionChangeTimer > 0f)
            return;

        float angle = Random.Range(
            -directionVariation,
            directionVariation
        );

        direction =
            Quaternion.Euler(0f, 0f, angle) *
            direction;

        direction.Normalize();

        ResetDirectionChangeTimer();
    }

    private void ResetDirectionChangeTimer()
    {
        directionChangeTimer = Random.Range(
            minimumDirectionChangeTime,
            maximumDirectionChangeTime
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (reagiu)
            return;

        MoleculeChamber otherMolecule =
            other.GetComponent<MoleculeChamber>();

        if (otherMolecule == null ||
            otherMolecule.reagiu)
        {
            return;
        }

        if (otherMolecule.Type == moleculeType)
            return;

        bool redBlue =
            (moleculeType == MoleculeType.Red &&
             otherMolecule.Type == MoleculeType.Blue) ||
            (moleculeType == MoleculeType.Blue &&
             otherMolecule.Type == MoleculeType.Red);

        if (!redBlue)
            return;

        if (collisionPuzzle != null &&
            collisionPuzzle.PodeOcorrerColisaoEfetiva)
        {
            reagiu = true;
            otherMolecule.MarcarComoReagida();

            collisionPuzzle.RegistrarColisaoEfetiva(
                this,
                otherMolecule
            );

            return;
        }

        Vector2 awayDirection =
            (transform.position -
             otherMolecule.transform.position).normalized;

        if (awayDirection == Vector2.zero)
            awayDirection = -direction;

        direction = awayDirection;

        PlaySquash(awayDirection);
    }

    private void PlaySquash(Vector2 impactDirection)
    {
        if (squashCoroutine != null)
            StopCoroutine(squashCoroutine);

        squashCoroutine =
            StartCoroutine(SquashRoutine(impactDirection));
    }

    private IEnumerator SquashRoutine(Vector2 impactDirection)
    {
        float angle =
            Mathf.Atan2(
                impactDirection.y,
                impactDirection.x
            ) * Mathf.Rad2Deg;

        Quaternion originalRotation =
            transform.localRotation;

        transform.localRotation =
            Quaternion.Euler(0f, 0f, angle);

        Vector3 squashedScale = new Vector3(
            originalScale.x * squashAmount,
            originalScale.y * stretchAmount,
            originalScale.z
        );

        float elapsed = 0f;

        while (elapsed < squashDuration)
        {
            elapsed += Time.deltaTime;

            transform.localScale = Vector3.Lerp(
                originalScale,
                squashedScale,
                elapsed / squashDuration
            );

            yield return null;
        }

        elapsed = 0f;

        while (elapsed < recoveryDuration)
        {
            elapsed += Time.deltaTime;

            transform.localScale = Vector3.Lerp(
                squashedScale,
                originalScale,
                elapsed / recoveryDuration
            );

            yield return null;
        }

        transform.localScale = originalScale;
        transform.localRotation = originalRotation;

        squashCoroutine = null;
    }
}