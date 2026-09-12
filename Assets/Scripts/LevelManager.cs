using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;

    private bool levelFinished;

    private void Start()
    {
        Time.timeScale = 1f;

        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        if (levelFinished)
            return;

        levelFinished = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ShowVictory()
    {
        if (levelFinished)
            return;

        levelFinished = true;
        victoryPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }
}