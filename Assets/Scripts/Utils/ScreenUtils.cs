using UnityEngine;

public static class ScreenUtils
{
    // Devuelve true si worldPos es visible por cam (y outScreenPos está en coordenadas GUI, si es visible).
    public static bool TryWorldToGuiPosition(Camera cam, Vector3 worldPos, out Vector2 outGuiPos)
    {
        outGuiPos = Vector2.zero;
        if (cam == null) return false;

        Vector3 screenPos = cam.WorldToScreenPoint(worldPos);

        // z <= 0 => detrás de la cámara
        if (screenPos.z <= 0f) return false;

        // Comprobar dentro de la ventana de la cámara (en píxeles)
        if (screenPos.x < 0f || screenPos.x > cam.pixelWidth || screenPos.y < 0f || screenPos.y > cam.pixelHeight)
            return false;

        // Convertir a coordenadas GUI (y invertida)
        outGuiPos = new Vector2(screenPos.x, cam.pixelHeight - screenPos.y);
        return true;
    }
}

public class Example : MonoBehaviour
{
    public Vector3 someWorldPosition;

    void OnGUI()
    {
        Vector2 guiPos;
        if (ScreenUtils.TryWorldToGuiPosition(Camera.main, someWorldPosition, out guiPos))
        {
            GUI.Label(new Rect(guiPos.x, guiPos.y, 120, 20), "Info");
        }
    }
}