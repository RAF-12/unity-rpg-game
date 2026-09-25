using UnityEngine;

public class DontDistroyOnLoad : MonoBehaviour
{

    private void Awake()
    {
        if (!PlayerControler.estaCreadoPlayer)
        {
            DontDestroyOnLoad(this.transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}