using UnityEngine;

public class TemperatureOrb : MonoBehaviour
{
    [Header("Energia")]
    [SerializeField] private int energyChange = 1;

    [Header("Movimento visual")]
    [SerializeField] private float floatHeight = 0.15f;
    [SerializeField] private float floatSpeed = 2f;
    [SerializeField] private float rotationSpeed = 50f;

    private Vector3 initialPosition;
    private EnergyController energyController;

    private void Start()
    {
        initialPosition = transform.position;

        energyController =
            FindAnyObjectByType<EnergyController>();

        if (energyController == null)
        {
            Debug.LogError(
                "EnergyController não foi encontrado na cena!"
            );
        }
    }

    private void Update()
    {
        float verticalMovement =
            Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        transform.position = new Vector3(
            initialPosition.x,
            initialPosition.y + verticalMovement,
            initialPosition.z
        );

        transform.Rotate(
            0f,
            0f,
            rotationSpeed * Time.deltaTime
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (energyController == null)
            return;

        energyController.ChangeEnergy(energyChange);

        Destroy(gameObject);
    }
}