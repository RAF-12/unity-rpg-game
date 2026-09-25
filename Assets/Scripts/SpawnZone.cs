using System.Timers;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    private PlayerControler elPlayer;
    private CamaraFollow laCamara;
    public Vector2 direccionMira = Vector2.zero;

    public string NombreZona;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        elPlayer = FindAnyObjectByType<PlayerControler>();
        laCamara = FindAnyObjectByType<CamaraFollow>();

        if (!elPlayer.DondeIr.Equals(NombreZona))
        {
            return;
        }

        elPlayer.transform.position = this.transform.position;
        laCamara.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, laCamara.transform.position.z);

        elPlayer.movimineto = direccionMira;
    }
}
