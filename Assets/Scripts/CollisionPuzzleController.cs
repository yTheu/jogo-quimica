using UnityEngine;

public class CollisionPuzzleController : MonoBehaviour
{
    [SerializeField] private TemperatureChamberVisual temperatureChamber;
    [SerializeField] private PistaoPorta pistaoPorta;

    [Header("Faixa de ativação")]
    [SerializeField] private float temperaturaMinima = 78.5f;
    [SerializeField] private float temperaturaMaxima = 84f;
    [SerializeField] private float tempoNecessario = 3f;

    private bool cargaLiberada;
    private bool energiaDeAtivacaoAtingida;
    private bool puzzleConcluido;
    private float tempoNaFaixa;

    public bool PodeOcorrerColisaoEfetiva
    {
        get
        {
            return cargaLiberada &&
                   energiaDeAtivacaoAtingida &&
                   !puzzleConcluido;
        }
    }

    public void LiberarCarga()
    {
        if (cargaLiberada || puzzleConcluido)
            return;

        if (temperatureChamber == null)
            return;

        cargaLiberada = true;
        energiaDeAtivacaoAtingida = false;
        tempoNaFaixa = 0f;

        temperatureChamber.LiberarMoleculas();

        Debug.Log("Carga liberada. Mantenha a energia na faixa de ativação.");
    }

    private void Update()
    {
        if (!cargaLiberada ||
            puzzleConcluido ||
            energiaDeAtivacaoAtingida)
        {
            return;
        }

        float temperatura =
            temperatureChamber.InternalTemperature;

        bool dentroDaFaixa =
            temperatura >= temperaturaMinima &&
            temperatura <= temperaturaMaxima;

        if (dentroDaFaixa)
        {
            tempoNaFaixa += Time.deltaTime;

            if (tempoNaFaixa >= tempoNecessario)
            {
                energiaDeAtivacaoAtingida = true;
                Debug.Log("ENERGIA DE ATIVAÇÃO ATINGIDA!");
            }
        }
        else
        {
            tempoNaFaixa = 0f;
        }
    }

    public void RegistrarColisaoEfetiva(
        MoleculeChamber primeira,
        MoleculeChamber segunda)
    {
        if (!PodeOcorrerColisaoEfetiva)
            return;

        if (primeira == null || segunda == null)
            return;

        bool redBlue =
            (primeira.Type == MoleculeChamber.MoleculeType.Red &&
             segunda.Type == MoleculeChamber.MoleculeType.Blue) ||
            (primeira.Type == MoleculeChamber.MoleculeType.Blue &&
             segunda.Type == MoleculeChamber.MoleculeType.Red);

        if (!redBlue)
            return;

        temperatureChamber.CriarProduto(
            primeira,
            segunda
        );
    }

    public void ConcluirPuzzle()
    {
        if (puzzleConcluido)
            return;

        puzzleConcluido = true;

        Debug.Log("PUZZLE DE COLISÃO CONCLUÍDO!");

        if (pistaoPorta != null)
            pistaoPorta.Abrir();
    }
}