using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelDoor : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [SerializeField] private Collider2D transitionTrigger;
    [SerializeField] private bool startUnlocked;

    private bool isUnlocked;
    private bool isLoading;

    private void Awake()
    {
        if (transitionTrigger == null)
            transitionTrigger = GetComponent<Collider2D>();

        isUnlocked = startUnlocked;

        if (transitionTrigger != null)
            transitionTrigger.enabled = isUnlocked;
    }

    public void Unlock()
    {
        isUnlocked = true;

        if (transitionTrigger != null)
            transitionTrigger.enabled = true;

        Debug.Log("Saída para a próxima fase liberada.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isUnlocked || isLoading)
            return;

        if (!other.CompareTag("Player"))
            return;

        LoadNextScene();
    }

    private void LoadNextScene()
    {
        if (string.IsNullOrWhiteSpace(nextSceneName))
        {
            Debug.LogError(
                "O nome da próxima Scene não foi configurado."
            );

            return;
        }

        isLoading = true;

        SceneManager.LoadScene(nextSceneName);
    }
}