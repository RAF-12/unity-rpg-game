
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider VidaSlider;
    public Text vidaText;
    public Slider ExpSlider;
    public Text expText;
    public Text LvText;

    private GameObject Player;

    void Start()
    {
        // Cachear referencias si no se asignaron en el Inspector
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        VidaManager vidaManager = Player.GetComponent<VidaManager>();
        // Seguridad: evitar NRE si falta alguna referencia
        if (vidaManager)
        {
            VidaSlider.maxValue = vidaManager.maximaVida;
            VidaSlider.value = vidaManager.vidaActula;

            StringBuilder stringVida = new StringBuilder("HP: ");
            stringVida.Append(Mathf.RoundToInt(vidaManager.vidaActula));
            stringVida.Append(" / ");
            stringVida.Append(Mathf.RoundToInt(vidaManager.maximaVida));
            vidaText.text = stringVida.ToString();
        }
        CharacterState characterState = Player.GetComponent<CharacterState>();
        if (characterState != null)
        {
            if (characterState.sigienteNivel != null && characterState.levelActual < characterState.sigienteNivel.Length)
            {
                ExpSlider.maxValue = characterState.sigienteNivel[characterState.levelActual];
                ExpSlider.value = characterState.experienciaActual;
                StringBuilder stringExp = new StringBuilder("");
                stringExp.Append(characterState.experienciaActual);
                stringExp.Append(" / ");
                stringExp.Append(characterState.sigienteNivel[characterState.levelActual]);
                stringExp.Append(" EXP");
                expText.text = stringExp.ToString();
            }
            else
            {
                ExpSlider.maxValue = 1;
                ExpSlider.value = 1;
                expText.text = "EXP: MAX LEVEL";
            }

            LvText.text = "Lv. " + characterState.levelActual;
        }
    }
}