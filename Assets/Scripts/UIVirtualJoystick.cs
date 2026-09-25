using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Joystick virtual simple: arrastra el "handle" dentro del "background".
// Asigna el Player (GameObject con PlayerControler), el background (imagen) y el handle (imagen).
public class UIVirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform background; // imagen de fondo (circular)
    public RectTransform handle;     // imagen del joystick (movible)
    [Range(0.1f, 1f)]
    public float handleRange = 0.6f; // relación del radio del handle con respecto al background
    public PlayerControler player;    // referencia al PlayerControler (arrastrar desde Inspector)

    void Start()
    {
        if (background == null) background = GetComponent<RectTransform>();
        if (handle == null && transform.childCount > 0) handle = transform.GetChild(0) as RectTransform;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (background == null || handle == null) return;

        Vector2 localPoint;
        // Convertir posición de pantalla a punto local dentro del rect background
        RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out localPoint);

        Vector2 pivotAdjusted = new Vector2(
            localPoint.x / (background.sizeDelta.x * 0.5f),
            localPoint.y / (background.sizeDelta.y * 0.5f)
        );

        Vector2 clamped = Vector2.ClampMagnitude(pivotAdjusted, 1f);

        // Mover handle en px
        Vector2 handlePos = new Vector2(
            clamped.x * background.sizeDelta.x * 0.5f * handleRange,
            clamped.y * background.sizeDelta.y * 0.5f * handleRange
        );
        handle.anchoredPosition = handlePos;

        // Enviar valor normalizado al player
        player?.SetVirtualJoystick(clamped);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (handle != null) handle.anchoredPosition = Vector2.zero;
        player?.ReleaseVirtualJoystick();
    }
}
