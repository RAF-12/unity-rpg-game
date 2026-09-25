using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    private int daño = 5;

    private float tiempoParaRevivir = 10f;
    private bool revivir = false;

    private GameObject player;
    public GameObject numeroDaño;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");


    }
    private void Update()
    {
        if (revivir)
        {
            tiempoParaRevivir -= Time.deltaTime;
            if (tiempoParaRevivir <= 0)
            {
                if (player != null) 
                {
                    player.GetComponent<VidaManager>().revivir();
                    tiempoParaRevivir = 10f;
                    revivir = false;
                }
            }
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            float vidaActual = collision.gameObject.GetComponent<VidaManager>().damageCharacter(daño);
            var clone2 = (GameObject)Instantiate(numeroDaño, collision.transform.position, Quaternion.Euler(Vector3.zero));
            clone2.GetComponent<DañoNumero>().numeroText.color = Color.red;
            clone2.GetComponent<DañoNumero>().numeroDaño = daño;
            if (vidaActual <= 0)
            {
                revivir = true;
                tiempoParaRevivir = 10f;
            }
        }
    }
}
