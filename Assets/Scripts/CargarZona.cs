using UnityEngine;
using UnityEngine.SceneManagement;

public class CargarZona : MonoBehaviour
{
    public string nombreScena = "MI Scena";
    public string ZonaDondeIr;
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            FindAnyObjectByType<PlayerControler>().DondeIr = ZonaDondeIr;
            SceneManager.LoadScene(nombreScena);

        }
    }
}
