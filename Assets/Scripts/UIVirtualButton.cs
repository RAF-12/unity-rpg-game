using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

// Botón virtual configurable para UI (Canvas).
 // Tipos: Attack, Menu, Inventory, ChangeWeapon.
 // - Attack: llama a player.PressAttack() si player está asignado.
 // - ChangeWeapon: envía SendMessage("ChangeWeapon") al player (implementa ChangeWeapon en tu script si lo deseas).
 // - Menu / Inventory: dispara los UnityEvents configurables (p. ej. abrir/cerrar interfaces).
public class UIVirtualButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public enum ButtonType { Attack, Menu, Inventory, ChangeWeapon }

    [Tooltip("Tipo de acción que realiza este botón")]
    public ButtonType tipo = ButtonType.Attack;

    [Tooltip("Referencia opcional al Player (arrastrar GameObject que contiene PlayerControler)")]
    public PlayerControler player;

    [Header("Eventos (Inspector)")]
    [Tooltip("Evento disparado al presionar el botón (OnPointerDown)")]
    public UnityEvent onPressed;
    [Tooltip("Evento disparado al soltar el botón (OnPointerUp)")]
    public UnityEvent onReleased;
    [Tooltip("Evento disparado al hacer click/tap (OnPointerClick / OnPointerUp estable)")]
    public UnityEvent onClicked;

    // IPointerDownHandler
    public void OnPointerDown(PointerEventData eventData)
    {
        // Ejecutar la acción inmediata para tipos que requieren press
        switch (tipo)
        {
            case ButtonType.Attack:
                // Llamada directa a PlayerControler si existe
                if (player != null)
                {
                    player.PressAttack();
                }
                break;
            case ButtonType.ChangeWeapon:
                if (player != null)
                {
                    // Intenta invocar ChangeWeapon en el player (no obligatorio)
                    player.gameObject.SendMessage("ChangeWeapon", SendMessageOptions.DontRequireReceiver);
                }
                break;
            case ButtonType.Menu:
            case ButtonType.Inventory:
                // Normalmente manejado en click, pero permitimos acción en press si el diseñador lo desea
                break;
        }

        onPressed?.Invoke();
    }

    // IPointerUpHandler
    public void OnPointerUp(PointerEventData eventData)
    {
        onReleased?.Invoke();
    }

    // IPointerClickHandler
    public void OnPointerClick(PointerEventData eventData)
    {
        // Disparar evento de click para Menu/Inventory u otros
        switch (tipo)
        {
            case ButtonType.Menu:
            case ButtonType.Inventory:
                onClicked?.Invoke();
                break;
            case ButtonType.Attack:
            case ButtonType.ChangeWeapon:
                // ya manejados en OnPointerDown, pero también permitimos click como alternativa
                onClicked?.Invoke();
                break;
        }
    }
}
