using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private LayerMask activationLayer;
    [SerializeField] private SlidingMechanism[] mechanisms;

    private readonly HashSet<Collider2D> objectsOnPlate = new();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsOnActivationLayer(other.gameObject))
            return;

        objectsOnPlate.Add(other);
        UpdateMechanisms();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!IsOnActivationLayer(other.gameObject))
            return;

        objectsOnPlate.Remove(other);
        UpdateMechanisms();
    }

    private bool IsOnActivationLayer(GameObject objectToCheck)
    {
        return (activationLayer.value &
                (1 << objectToCheck.layer)) != 0;
    }

    private void UpdateMechanisms()
    {
        bool shouldOpen = objectsOnPlate.Count > 0;

        foreach (SlidingMechanism mechanism in mechanisms)
        {
            if (mechanism != null)
                mechanism.SetOpen(shouldOpen);
        }
    }
}