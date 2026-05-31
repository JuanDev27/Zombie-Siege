using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float speed = 5f;
    private Rigidbody2D rb;
    private PlayerController playerController;
    private Vector2 move;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {  
        if (playerController != null && !playerController.CanMove()) return;
        move = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) move.y += 1f;
        if (Keyboard.current.sKey.isPressed) move.y -= 1f;
        if (Keyboard.current.dKey.isPressed) move.x += 1f;
        if (Keyboard.current.aKey.isPressed) move.x -= 1f;

        if (move.magnitude > 1) 
        {
            move.Normalize();
        }
   
    }

    void FixedUpdate()
    {
        // Si el jugador está sufriendo Knockback, las físicas toman el control total y el input se bloquea
        if (playerController != null && !playerController.CanMove()) return;

        // MOVIMIENTO MEDIANTE RIGIDBODY (Fluido y respeta colisiones)
        if (rb != null)
        {
            rb.linearVelocity = move * speed;
        }
    }
}
