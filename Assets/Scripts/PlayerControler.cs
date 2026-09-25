using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    public float speed = 4.0f;
    private bool caminando = false;
    public Vector2 movimineto = Vector2.zero;

    private const string horizontal = "Horizontal";
    private const string vertical = "Vertical";
    private const string verVertical = "VerVertical";
    private const string verHorizontal = "VerHorizontal";
    private const string moviendo = "Moviendo";
    private const string ataque = "Ataque";

    private Animator animator;
    private Rigidbody2D playerRb;
    private Vector2 moveindose = Vector2.zero;
    public string DondeIr;

    public static bool estaCreadoPlayer;
    private bool estadoDeAtaque = false;
    public float tiempoDeAtaque;
    private float contadorTiempoDeAtaque;

    // Input System: asignar en Inspector (opcional)
    public InputActionReference moveAction;              // Vector2 (joystick / teclado)
    public InputActionReference attackAction;            // Button (fire / touch)
    public InputActionReference buttonUpAction;          // botón táctil arriba (opcional)
    public InputActionReference buttonDownAction;        // botón táctil abajo (opcional)
    public InputActionReference buttonLeftAction;        // botón táctil izquierda (opcional)
    public InputActionReference buttonRightAction;       // botón táctil derecha (opcional)

    // Internos para combinar entradas
    private Vector2 buttonsInput = Vector2.zero;
    private Vector2 virtualJoystickInput = Vector2.zero; // nueva entrada desde joystick virtual

    void OnEnable()
    {
        if (moveAction?.action != null)
            moveAction.action.Enable();

        if (attackAction?.action != null)
        {
            attackAction.action.Enable();
            attackAction.action.performed += OnAttackPerformed;
        }

        EnableButtonAction(buttonUpAction, ctx => buttonsInput.y = 1, ctx => { if (buttonsInput.y > 0) buttonsInput.y = 0; });
        EnableButtonAction(buttonDownAction, ctx => buttonsInput.y = -1, ctx => { if (buttonsInput.y < 0) buttonsInput.y = 0; });
        EnableButtonAction(buttonLeftAction, ctx => buttonsInput.x = -1, ctx => { if (buttonsInput.x < 0) buttonsInput.x = 0; });
        EnableButtonAction(buttonRightAction, ctx => buttonsInput.x = 1, ctx => { if (buttonsInput.x > 0) buttonsInput.x = 0; });
    }

    void OnDisable()
    {
        if (moveAction?.action != null)
            moveAction.action.Disable();

        if (attackAction?.action != null)
        {
            attackAction.action.performed -= OnAttackPerformed;
            attackAction.action.Disable();
        }

        DisableButtonAction(buttonUpAction, ctx => { }, ctx => { });
        DisableButtonAction(buttonDownAction, ctx => { }, ctx => { });
        DisableButtonAction(buttonLeftAction, ctx => { }, ctx => { });
        DisableButtonAction(buttonRightAction, ctx => { }, ctx => { });
    }

    private void EnableButtonAction(InputActionReference actionRef, System.Action<InputAction.CallbackContext> performed, System.Action<InputAction.CallbackContext> canceled)
    {
        if (actionRef?.action == null) return;
        actionRef.action.Enable();
        actionRef.action.performed += performed;
        actionRef.action.canceled += canceled;
    }

    private void DisableButtonAction(InputActionReference actionRef, System.Action<InputAction.CallbackContext> performed, System.Action<InputAction.CallbackContext> canceled)
    {
        if (actionRef?.action == null) return;
        actionRef.action.performed -= performed;
        actionRef.action.canceled -= canceled;
        actionRef.action.Disable();
    }

    private void OnAttackPerformed(InputAction.CallbackContext ctx)
    {
        if (!estadoDeAtaque)
        {
            estadoDeAtaque = true;
            contadorTiempoDeAtaque = tiempoDeAtaque;
            if (animator != null)
                animator.SetBool(ataque, true);
        }
    }

    // Métodos públicos para botones UI (Canvas). Asignar desde EventTrigger o desde el componente auxiliares.
    public void PressUp()    => buttonsInput.y = 1;
    public void ReleaseUp()  { if (buttonsInput.y > 0) buttonsInput.y = 0; }
    public void PressDown()  => buttonsInput.y = -1;
    public void ReleaseDown(){ if (buttonsInput.y < 0) buttonsInput.y = 0; }
    public void PressLeft()  => buttonsInput.x = -1;
    public void ReleaseLeft(){ if (buttonsInput.x < 0) buttonsInput.x = 0; }
    public void PressRight() => buttonsInput.x = 1;
    public void ReleaseRight(){ if (buttonsInput.x > 0) buttonsInput.x = 0; }

    // Opcional: método para ataque desde UI
    public void PressAttack()
    {
        if (!estadoDeAtaque)
        {
            estadoDeAtaque = true;
            contadorTiempoDeAtaque = tiempoDeAtaque;
            if (animator != null) animator.SetBool(ataque, true);
        }
    }

    // Nuevos métodos para joystick virtual
    public void SetVirtualJoystick(Vector2 value) => virtualJoystickInput = value;
    public void ReleaseVirtualJoystick() => virtualJoystickInput = Vector2.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody2D>();

        if (!estaCreadoPlayer)
        {
            estaCreadoPlayer = true;
            DontDestroyOnLoad(this.transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        caminando = false;

        if (estadoDeAtaque)
        {
            contadorTiempoDeAtaque -= Time.deltaTime;
            if (contadorTiempoDeAtaque <= 0)
            {
                estadoDeAtaque = false;
                if (animator != null)
                    animator.SetBool(ataque, false);
            }
        }
        else
        {
            // Fallback: si no hay attackAction asignada permite Input clásico
            if (attackAction == null && (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame || Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame))
            {
                estadoDeAtaque = true;
                contadorTiempoDeAtaque = tiempoDeAtaque;
                if (animator != null)
                    animator.SetBool(ataque, true);
            }
        }

        // Leer entrada de movimiento (Input System) y combinar con botones táctiles y joystick virtual
        Vector2 inputMove = Vector2.zero;
        if (moveAction?.action != null)
            inputMove = moveAction.action.ReadValue<Vector2>();
        else
        {
            // Fallback a Input clásico si no hay action asignada
            inputMove = new Vector2(Input.GetAxisRaw(horizontal), Input.GetAxisRaw(vertical));
        }

        // Combina: action joystick (físico), + joystick virtual (UI), + botones D-pad
        Vector2 combinedMove = inputMove + virtualJoystickInput + buttonsInput;
        // Normalizar para evitar exceso de magnitud diagonal
        if (combinedMove.magnitude > 1f) combinedMove = combinedMove.normalized;

        // Aplicar al sistema existente
        moveindose = combinedMove;

        // Si la magnitud del vector es mayor a un umbral (ej. 0.5)
        if (moveindose.magnitude > 0.5f)
        {
            // Marcamos que está caminando
            caminando = true;

            // Guardamos el último vector de movimiento
            movimineto = moveindose;
        }

        if (!caminando)
        {
            // Mantengo linearVelocity por tu preferencia (línea original)
            playerRb.linearVelocity = Vector2.zero;
        }

        if (animator != null)
        {
            animator.SetFloat(horizontal, moveindose.x);
            animator.SetFloat(vertical, moveindose.y);

            animator.SetBool(moviendo, caminando);

            animator.SetFloat(verVertical, movimineto.y);
            animator.SetFloat(verHorizontal, movimineto.x);
        }
    }

    private void FixedUpdate()
    {
        // Si la magnitud del vector es mayor a un umbral (ej. 0.5)
        if (moveindose.magnitude > 0.5f && playerRb != null)
        {
            // Calculamos el movimiento
            Vector2 move = moveindose.normalized * speed * Time.deltaTime;
            // Movemos el Rigidbody
            playerRb.MovePosition(playerRb.position + move);
        }
    }
}
