using UnityEngine;

public class Grinder : MonoBehaviour
{
    [Header("Fragmentos")]
    [SerializeField] private TabletFragment fragmentPrefab;
    [SerializeField] private Transform fragmentSpawnPoint;
    [SerializeField] private int fragmentCount = 8;

    [Header("Dispersão")]
    [SerializeField] private float horizontalSpread = 1.2f;
    [SerializeField] private float downwardSpeed = 1.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        ReagentTablet tablet =
            other.GetComponent<ReagentTablet>();

        if (tablet == null)
            return;

        CrushTablet(tablet);
    }

    private void CrushTablet(ReagentTablet tablet)
    {
        if (!tablet.TryCrush())
            return;

        for (int i = 0; i < fragmentCount; i++)
        {
            TabletFragment fragment = Instantiate(
                fragmentPrefab,
                fragmentSpawnPoint.position,
                Quaternion.identity
            );

            fragment.Initialize(tablet.TabletId);

            Rigidbody2D fragmentRigidbody =
                fragment.GetComponent<Rigidbody2D>();

            if (fragmentRigidbody != null)
            {
                fragmentRigidbody.linearVelocity =
                    new Vector2(
                        Random.Range(
                            -horizontalSpread,
                            horizontalSpread
                        ),
                        -downwardSpeed
                    );
            }
        }

        Destroy(tablet.gameObject);
    }
}