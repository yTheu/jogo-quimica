using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GearKillZone : MonoBehaviour
{
    [Header("Renascimento")]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float respawnDelay = 0.7f;

    [Header("Efeito visual opcional")]
    [SerializeField] private Image damageFlash;
    [SerializeField] private float flashDuration = 0.2f;

    private bool playerIsDead;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || playerIsDead)
            return;

        StartCoroutine(DeathRoutine(other.gameObject));
    }

    private IEnumerator DeathRoutine(GameObject player)
    {
        playerIsDead = true;

        Rigidbody2D playerRigidbody =
            player.GetComponent<Rigidbody2D>();

        SpriteRenderer playerRenderer =
            player.GetComponent<SpriteRenderer>();

        Collider2D playerCollider =
            player.GetComponent<Collider2D>();

        PlayerController playerController =
            player.GetComponent<PlayerController>();

        if (playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = Vector2.zero;
            playerRigidbody.simulated = false;
        }

        if (playerController != null)
            playerController.enabled = false;

        if (playerCollider != null)
            playerCollider.enabled = false;

        if (playerRenderer != null)
            playerRenderer.enabled = false;

        if (damageFlash != null)
        {
            damageFlash.gameObject.SetActive(true);
            yield return new WaitForSeconds(flashDuration);
            damageFlash.gameObject.SetActive(false);
        }

        float remainingDelay = Mathf.Max(
            0f,
            respawnDelay - flashDuration
        );

        yield return new WaitForSeconds(remainingDelay);

        player.transform.position = respawnPoint.position;

        if (playerRigidbody != null)
        {
            playerRigidbody.simulated = true;
            playerRigidbody.linearVelocity = Vector2.zero;
        }

        if (playerRenderer != null)
            playerRenderer.enabled = true;

        if (playerCollider != null)
            playerCollider.enabled = true;

        if (playerController != null)
            playerController.enabled = true;

        playerIsDead = false;
    }
}