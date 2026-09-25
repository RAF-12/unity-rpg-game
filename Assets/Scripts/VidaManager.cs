using UnityEngine;

public class VidaManager : MonoBehaviour
{

    public float maximaVida;
    public float vidaActula;

    private int ExpPorMuerte = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vidaActula = maximaVida;
    }

    // Update is called once per frame
    void Update()
    {
        if (vidaActula<= 0)
        {
            morir();
        }

    }
    // Devuelve la vida actual despues de recibir el daño
    public float damageCharacter(int damage)
    {
        vidaActula -= damage;
        return vidaActula;
    }
    public void updateMaxVida(int newMaximaVida)
    {
        Debug.Log("Aumentando vida maxima en: " + newMaximaVida);
        maximaVida += newMaximaVida;
        vidaActula = maximaVida;
    }
    public void morir()
    {
        if (gameObject.tag.Equals("Enemy"))
        {
            GameObject.Find("Player").GetComponent<CharacterState>().addExperiencia(ExpPorMuerte);
        }
        gameObject.SetActive(false);
    }
    public void revivir() 
    {

        gameObject.SetActive(true);
        vidaActula = maximaVida;
    }
}
