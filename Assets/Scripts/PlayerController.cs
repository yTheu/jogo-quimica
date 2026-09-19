using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimento")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float extremeSpeedMultiplier = 2.5f;

    [Header("Gelo")]
    [SerializeField] private float aceleracaoGelo = 8f;
    [SerializeField] private float velocidadeMaximaGelo = 10f;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 16f;
    [SerializeField] private float dashDuration = 0.18f;
    [SerializeField] private float dashCooldown = 0.7f;

    [Header("Verificação do chão")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Diálogo")]
    [SerializeField] private RafaelDialogueController rafaelDialogue;

    private Rigidbody2D rb;

    private float horizontal;
    private float mobileHorizontal;

    private float facingDirection = 1f;
    private float originalGravityScale;

    private float velocidadeAtual;
    private float velocidadeGelo;

    private bool isGrounded;
    private bool estaNoGelo;
    private bool canDash = true;

    public bool IsDashing { get; private set; }

    public bool EstaSeMovendoHorizontalmente
    {
        get
        {
            return !DialogoAtivo &&
                   !IsDashing &&
                   Mathf.Abs(rb.linearVelocity.x) > 0.1f;
        }
    }

    private bool DialogoAtivo
    {
        get
        {
            return rafaelDialogue != null && rafaelDialogue.DialogoAtivo;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravityScale = rb.gravityScale;
        velocidadeAtual = moveSpeed;
    }

    private void Update()
    {
        CheckGround();

        if (DialogoAtivo)
        {
            horizontal = 0f;
            mobileHorizontal = 0f;
            return;
        }

        if (!IsDashing)
        {
            ReadMovement();

            Keyboard keyboard = Keyboard.current;

            if (keyboard != null)
            {
                bool pressedJump =
                    keyboard.spaceKey.wasPressedThisFrame ||
                    keyboard.wKey.wasPressedThisFrame ||
                    keyboard.upArrowKey.wasPressedThisFrame;

                if (pressedJump)
                {
                    TentarPular();
                }
            }
        }

        Keyboard currentKeyboard = Keyboard.current;

        if (currentKeyboard != null)
        {
            bool pressedDash =
                currentKeyboard.leftShiftKey.wasPressedThisFrame ||
                currentKeyboard.rightShiftKey.wasPressedThisFrame;

            if (pressedDash)
            {
                TentarDash();
            }
        }
    }

    private void FixedUpdate()
    {
        if (DialogoAtivo)
        {
            Vector2 velocity = rb.linearVelocity;
            velocity.x = 0f;
            rb.linearVelocity = velocity;
            return;
        }

        if (IsDashing)
            return;

        Vector2 velocityAtualRb = rb.linearVelocity;

        if (estaNoGelo && isGrounded)
        {
            if (Mathf.Abs(horizontal) > 0.01f)
            {
                float velocidadeAlvo = horizontal * velocidadeMaximaGelo;

                velocidadeGelo = Mathf.MoveTowards(
                    velocidadeGelo,
                    velocidadeAlvo,
                    aceleracaoGelo * Time.fixedDeltaTime
                );
            }

            velocityAtualRb.x = velocidadeGelo;
        }
        else
        {
            velocityAtualRb.x = horizontal * velocidadeAtual;
            velocidadeGelo = velocityAtualRb.x;
        }

        rb.linearVelocity = velocityAtualRb;
    }

    private void ReadMovement()
    {
        float keyboardHorizontal = 0f;

        Keyboard keyboard = Keyboard.current;

        if (keyboard != null)
        {
            if (keyboard.aKey.isPressed ||
                keyboard.leftArrowKey.isPressed)
            {
                keyboardHorizontal -= 1f;
            }

            if (keyboard.dKey.isPressed ||
                keyboard.rightArrowKey.isPressed)
            {
                keyboardHorizontal += 1f;
            }
        }

        if (Mathf.Abs(mobileHorizontal) > 0.01f)
            horizontal = mobileHorizontal;
        else
            horizontal = keyboardHorizontal;

        if (horizontal != 0f)
        {
            facingDirection = Mathf.Sign(horizontal);
        }
    }

    private void CheckGround()
    {
        Collider2D collider = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        isGrounded = collider != null;

        if (collider != null)
        {
            estaNoGelo = collider.CompareTag("Gelo");
        }
        else
        {
            estaNoGelo = false;
        }
    }

    private void TentarPular()
    {
        if (DialogoAtivo || !isGrounded || IsDashing)
            return;

        Vector2 velocity = rb.linearVelocity;
        velocity.y = jumpForce;
        rb.linearVelocity = velocity;
    }

    private void TentarDash()
    {
        if (DialogoAtivo || !canDash)
            return;

        StartCoroutine(Dash());
    }

    public void SetMobileHorizontal(float valor)
    {
        if (DialogoAtivo)
        {
            mobileHorizontal = 0f;
            return;
        }

        mobileHorizontal = Mathf.Clamp(valor, -1f, 1f);
    }

    public void MobileJump()
    {
        if (DialogoAtivo)
            return;

        TentarPular();
    }

    public void MobileDash()
    {
        if (DialogoAtivo)
            return;

        TentarDash();
    }

    public void AtualizarVelocidadePorTemperatura(int temperatura)
    {
        if (temperatura >= 11)
        {
            velocidadeAtual = moveSpeed * extremeSpeedMultiplier;
            return;
        }

        float multiplicador;

        if (temperatura < 4)
        {
            multiplicador = 1f - (4 - temperatura) * 0.15f;
        }
        else
        {
            multiplicador = 1f + (temperatura - 4) * 0.25f;
        }

        velocidadeAtual = moveSpeed * multiplicador;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        IsDashing = true;

        rb.gravityScale = 0f;

        rb.linearVelocity = new Vector2(
            facingDirection * dashSpeed,
            0f
        );

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravityScale;
        IsDashing = false;

        velocidadeGelo = rb.linearVelocity.x;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

    private void OnDisable()
    {
        if (rb != null)
        {
            rb.gravityScale = originalGravityScale;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            groundCheck.position,
            groundCheckRadius
        );
    }
}