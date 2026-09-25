using UnityEngine;

public class ThisDestuccion : MonoBehaviour
{
    public float tiempoDestruccion = 2f;
    // Update is called once per frame
    void Update()
    {
        tiempoDestruccion -= Time.deltaTime;
        if (tiempoDestruccion <= 0)
        {
            Destroy(gameObject);
        }
    }
}
