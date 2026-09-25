using UnityEngine;

public class DañoArma : MonoBehaviour
{
    [SerializeField]
    private int dañoBase = 10;

    public GameObject sangre;
    public GameObject puntoEmision;
    public GameObject puntoSangre;

    private CharacterState playerState;

    private void Start()
    {
        var player = GameObject.Find("Player");
        if (player != null)
            playerState = player.GetComponent<CharacterState>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            int fuerza = 0;
            if (playerState != null && playerState.estadisticas != null && playerState.estadisticas.ContainsKey("Fuerza"))
                fuerza = Mathf.FloorToInt( playerState.estadisticas["Fuerza"]);

            int finalDamage = dañoBase + fuerza;

            var vida = collision.gameObject.GetComponent<VidaManager>();
            if (vida != null)
                vida.damageCharacter(finalDamage);

            if (sangre != null && puntoEmision != null)
                Instantiate(sangre, puntoEmision.transform.position, puntoEmision.transform.rotation);

            if (puntoSangre != null && puntoEmision != null)
            {
                var clone = Instantiate(puntoSangre, puntoEmision.transform.position, Quaternion.identity);
                var dañoNumero = clone.GetComponent<DañoNumero>();
                if (dañoNumero != null)
                {
                    dañoNumero.numeroText.color = Color.white;
                    dañoNumero.numeroDaño = finalDamage;
                }
            }
        }
    }
}
