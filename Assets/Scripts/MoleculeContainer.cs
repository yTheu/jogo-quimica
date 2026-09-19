using UnityEngine;

public abstract class MoleculeContainer : MonoBehaviour
{
    public abstract void GetMovementLimits(
        out float minimumX,
        out float maximumX,
        out float minimumY,
        out float maximumY
    );
}