using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float speed = 5f;
    private Rigidbody2D rb;
    private PlayerController playerController;
    private Vector2 moveInput;

    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {  
        rb.velocity = moveInput * speed;

        if (playerController != null && !playerController.CanMove()) return;
        moveInput = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) moveInput.y += 1f;
        if (Keyboard.current.sKey.isPressed) moveInput.y -= 1f;
        if (Keyboard.current.dKey.isPressed) moveInput.x += 1f;
        if (Keyboard.current.aKey.isPressed) moveInput.x -= 1f;

        if (moveInput.magnitude > 1) 
        {
            moveInput.Normalize();
        }
   
    }

    void FixedUpdate()
    {
        // Si el jugador está sufriendo Knockback, las físicas toman el control total y el input se bloquea
        if (playerController != null && !playerController.CanMove()) return;

        // MOVIMIENTO MEDIANTE RIGIDBODY (Fluido y respeta colisiones)
        if (rb != null)
        {
            rb.linearVelocity = moveInput * speed;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        animator.SetBool("isWalking", true);

        if(context.canceled)
        {
            animator.SetBool("isWalking", false);
            animator.SetFloat("LastInputX",moveInput.x);
            animator.SetFloat("LastInputY",moveInput.y);
        }
        animator.SetFloat("InputX",moveInput.x);
        animator.SetFloat("InputY",moveInput.y);
    }
}
