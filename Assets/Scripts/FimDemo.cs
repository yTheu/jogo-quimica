using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class FimDemoController : MonoBehaviour
{
    public static bool MostrarFimDemo { get; set; } = false;

    [SerializeField] private GameObject overlayFimDemo;
    [SerializeField] private TMP_Text textoFimDemo;
    [SerializeField] private Image imagemRafael;

    [SerializeField] private Sprite rafaelApresentando;
    [SerializeField] private Sprite rafaelExplicando;
    [SerializeField] private Sprite rafaelOrgulhoso;

    [SerializeField] private float velocidadeTexto = 0.03f;

    private string textoCompleto;
    private bool textoFinalizado;
    private bool overlayAtivo;

    private void Start()
    {
        if (!MostrarFimDemo)
        {
            overlayFimDemo.SetActive(false);
            return;
        }

        MostrarFimDemo = false;

        textoCompleto = textoFimDemo.text;
        imagemRafael.sprite = rafaelApresentando;

        overlayFimDemo.SetActive(true);
        overlayAtivo = true;
        textoFinalizado = false;

        StartCoroutine(EscreverTexto());
    }

    private void Update()
    {
        if (!overlayAtivo || !textoFinalizado)
            return;

        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            FecharOverlay();
            return;
        }

        if (Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            FecharOverlay();
        }
    }

    private IEnumerator EscreverTexto()
    {
        textoFimDemo.text = textoCompleto;
        textoFimDemo.maxVisibleCharacters = 0;
        textoFimDemo.ForceMeshUpdate();

        int totalCaracteres = textoFimDemo.textInfo.characterCount;

        int inicioExplicando = textoCompleto.IndexOf(
            "Esta demonstração termina aqui"
        );

        int inicioProximasFases = textoCompleto.IndexOf(
            "Nas próximas fases"
        );

        int inicioOrgulhoso = textoCompleto.IndexOf(
            "Nos vemos em breve!"
        );

        for (int i = 0; i <= totalCaracteres; i++)
        {
            if (i >= inicioOrgulhoso && inicioOrgulhoso >= 0)
            {
                imagemRafael.sprite = rafaelOrgulhoso;
            }
            else if (i >= inicioProximasFases && inicioProximasFases >= 0)
            {
                imagemRafael.sprite = rafaelApresentando;
            }
            else if (i >= inicioExplicando && inicioExplicando >= 0)
            {
                imagemRafael.sprite = rafaelExplicando;
            }

            textoFimDemo.maxVisibleCharacters = i;

            yield return new WaitForSeconds(velocidadeTexto);
        }

        textoFinalizado = true;
    }

    private void FecharOverlay()
    {
        overlayAtivo = false;
        overlayFimDemo.SetActive(false);
    }
}