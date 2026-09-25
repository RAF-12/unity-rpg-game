using UnityEngine;

public class Atake : MonoBehaviour
{
    //este escript llama a ataque del playercontroler, se llama desde el boton virtual de la ui
    //si se presiona el objeto de cambas que tenga este escript, se llama a la función PressAttack() del playercontroler, si el playercontroler no es null
    [SerializeField]
    public PlayerControler player;
    
    public void PressAttack()
    {
        if (player != null)
        {
            player.PressAttack();
        }
    }

}
