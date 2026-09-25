using UnityEngine;
using UnityEngine.Rendering;

public class EnemyControler : MonoBehaviour
{
    public float enemySpeed = 1.0f;
    private Rigidbody2D enemyRb;

    private bool estaMoviendose;

    public Vector2 DireccionMovimiento;

    public float tiempoDeMovimiento;
    private float tiempoAMoverse;

    public float tiempoEntreMovimientos;
    private float tiempoEntreUltimoM;

    private Animator enemyAnimator;
    private string horizontal = "EnemyHorizontal";
    private string vertical = "EnemyVertical";
    private string enemyMove = "MoveEnemy";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();

        tiempoAMoverse = tiempoDeMovimiento * Random.Range(0.5f,2.0f);
        tiempoEntreUltimoM = tiempoEntreMovimientos * Random.Range(0.5f, 2.0f);


    }
    private void FixedUpdate()
    {
        if (estaMoviendose)
        {
            tiempoEntreUltimoM -= Time.deltaTime;

            Vector2 nuevaPos = enemyRb.position + DireccionMovimiento * Time.fixedDeltaTime;
            enemyRb.MovePosition(nuevaPos);

            if (tiempoEntreUltimoM < 0)
            {
                estaMoviendose = false;
                tiempoAMoverse = tiempoDeMovimiento;
            }
        }
        else
        {
            tiempoAMoverse -= Time.deltaTime;
            if (tiempoAMoverse < 0)
            {
                estaMoviendose = true;
                tiempoEntreUltimoM = tiempoEntreMovimientos;

                DireccionMovimiento = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)).normalized * enemySpeed;

            }
            if (DireccionMovimiento == Vector2.zero)
            {
                DireccionMovimiento.x = 0;
                DireccionMovimiento.y = 0;
                estaMoviendose = false;
            }
        }
        enemyAnimator.SetBool(enemyMove, estaMoviendose);
        enemyAnimator.SetFloat(horizontal, DireccionMovimiento.x);
        enemyAnimator.SetFloat(vertical, DireccionMovimiento.y);
    }
}
