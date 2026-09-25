using UnityEngine;
using UnityEngine.UI;

public class DañoNumero : MonoBehaviour
{
    private float tiempoDestruccion = 5f;

    public float numeroSpeed;
    public float numeroDaño;

    public Text numeroText;

    // Update is called once per frame
    void Update()
    {
        numeroText.text = numeroDaño.ToString();
        this.transform.position = new Vector3(this.transform.position.x,
                                              this.transform.position.y + numeroSpeed * Time.deltaTime,
                                              this.transform.position.z);
        tiempoDestruccion -= Time.deltaTime;
        if (tiempoDestruccion <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
