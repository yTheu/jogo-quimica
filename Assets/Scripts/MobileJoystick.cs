using UnityEngine;
using UnityEngine.EventSystems;

public class MobileJoystick : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IDragHandler
{
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform handle;
    [SerializeField] private PlayerController player;

    private float raio;
    private Vector2 posicaoNeutra;

    private void Start()
    {
        raio = background.rect.width / 2f;
        posicaoNeutra = handle.anchoredPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 posicao
        );

        float x = Mathf.Clamp(posicao.x / raio, -1f, 1f);

        handle.anchoredPosition = new Vector2(
            posicaoNeutra.x + x * raio,
            posicaoNeutra.y
        );

        player.SetMobileHorizontal(x);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        handle.anchoredPosition = posicaoNeutra;
        player.SetMobileHorizontal(0f);
    }
}