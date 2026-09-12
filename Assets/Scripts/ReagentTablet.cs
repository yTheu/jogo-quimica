using UnityEngine;

public class ReagentTablet : MonoBehaviour
{
    [SerializeField] private int tabletId;

    public int TabletId => tabletId;
    public bool WasCrushed { get; private set; }

    public bool TryCrush()
    {
        if (WasCrushed)
            return false;

        WasCrushed = true;
        return true;
    }
}