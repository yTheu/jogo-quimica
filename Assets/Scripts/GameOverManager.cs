using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public enum TipoMorte
    {
        Afogado,
        Explosao,
        Laser,
        Espinhos,
        FrioExtremo,
        AguaFervente
    }

    [Header("Painel")]
    [SerializeField] private GameObject goPanel;

    [Header("Frase")]
    [SerializeField] private Image fraseMorte;
    [SerializeField] private Sprite fraseAfogado;
    [SerializeField] private Sprite fraseExplosao;
    [SerializeField] private Sprite fraseLaser;
    [SerializeField] private Sprite fraseEspinhos;
    [SerializeField] private Sprite fraseFrioExtremo;
    [SerializeField] private Sprite fraseAguaFervente;

    [Header("Personagem")]
    [SerializeField] private Image personagemMorto;
    [SerializeField] private Sprite spriteAfogado;
    [SerializeField] private Sprite spriteExplosao;
    [SerializeField] private Sprite spriteLaser;
    [SerializeField] private Sprite spriteEspinhos;
    [SerializeField] private Sprite spriteFrioExtremo;
    [SerializeField] private Sprite spriteAguaFervente;

    public void GameOver(TipoMorte tipoMorte)
    {
        switch (tipoMorte)
        {
            case TipoMorte.Afogado:
                fraseMorte.sprite = fraseAfogado;
                personagemMorto.sprite = spriteAfogado;
                break;

            case TipoMorte.Explosao:
                fraseMorte.sprite = fraseExplosao;
                personagemMorto.sprite = spriteExplosao;
                break;

            case TipoMorte.Laser:
                fraseMorte.sprite = fraseLaser;
                personagemMorto.sprite = spriteLaser;
                break;

            case TipoMorte.Espinhos:
                fraseMorte.sprite = fraseEspinhos;
                personagemMorto.sprite = spriteEspinhos;
                break;

            case TipoMorte.FrioExtremo:
                fraseMorte.sprite = fraseFrioExtremo;
                personagemMorto.sprite = spriteFrioExtremo;
                break;

            case TipoMorte.AguaFervente:
                fraseMorte.sprite = fraseAguaFervente;
                personagemMorto.sprite = spriteAguaFervente;
                break;
        }

        goPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void TentarNovamente()
    {
        RafaelDialogueController.PularIntroducaoAoReiniciar = true;

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void VoltarMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}