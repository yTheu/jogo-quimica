using UnityEngine;

public class PlataformaMovel : MonoBehaviour
{
    [SerializeField] private Transform pontoA;
    [SerializeField] private Transform pontoB;
    [SerializeField] private float velocidade = 2f;

    private Transform destinoAtual;

    private void Start()
    {
        destinoAtual = pontoB;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            destinoAtual.position,
            velocidade * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, destinoAtual.position) < 0.01f)
        {
            destinoAtual = destinoAtual == pontoA ? pontoB : pontoA;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        foreach (ContactPoint2D contato in collision.contacts)
        {
            if (contato.normal.y < -0.5f)
            {
                collision.transform.SetParent(transform);
                break;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        collision.transform.SetParent(null);
    }
}