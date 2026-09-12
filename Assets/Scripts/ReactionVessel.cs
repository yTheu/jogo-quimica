using UnityEngine;

public class ReactionVessel : MonoBehaviour
{
    [SerializeField]
    private SurfaceAreaStageManager stageManager;

    [SerializeField]
    private float fragmentLifetimeInsideVessel = 3f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        TabletFragment fragment =
            other.GetComponent<TabletFragment>();

        if (fragment == null)
            return;

        if (!fragment.TryDeposit())
            return;

        if (stageManager != null)
        {
            stageManager.DepositFragment(
                fragment.SourceTabletId
            );
        }

        SettleFragment(fragment);
    }

    private void SettleFragment(
        TabletFragment fragment)
    {
        Collider2D fragmentCollider =
            fragment.GetComponent<Collider2D>();

        if (fragmentCollider != null)
            fragmentCollider.enabled = false;

        Rigidbody2D fragmentRigidbody =
            fragment.GetComponent<Rigidbody2D>();

        if (fragmentRigidbody != null)
        {
            fragmentRigidbody.linearVelocity =
                Vector2.down * 0.2f;

            fragmentRigidbody.gravityScale = 0.1f;
        }

        Destroy(
            fragment.gameObject,
            fragmentLifetimeInsideVessel
        );
    }
}