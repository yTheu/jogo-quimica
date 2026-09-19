using UnityEngine;

public class CollisionPuzzleController : MonoBehaviour
{
    [SerializeField] private int cargasMaximas = 3;

    private int cargasRestantes;

    public int CargasRestantes
    {
        get
        {
            return cargasRestantes;
        }
    }

    private void Start()
    {
        cargasRestantes = cargasMaximas;
    }

    public void LiberarCarga()
    {
        if (cargasRestantes <= 0)
        {
            Debug.Log("Sem cargas de reagentes disponíveis.");
            return;
        }

        cargasRestantes--;

        Debug.Log(
            "Carga liberada. Cargas restantes: " +
            cargasRestantes
        );
    }
}