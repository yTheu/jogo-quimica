using UnityEngine;

public class TabletFragment : MonoBehaviour
{
    public int SourceTabletId { get; private set; }
    public bool WasDeposited { get; private set; }

    public void Initialize(int tabletId)
    {
        SourceTabletId = tabletId;
    }

    public bool TryDeposit()
    {
        if (WasDeposited)
            return false;

        WasDeposited = true;
        return true;
    }
}