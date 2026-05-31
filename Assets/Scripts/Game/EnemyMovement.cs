using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 3f;

    private Rigidbody2D rb2D;
    private Transform player;
    //No hacen nada
    [SerializeField] private float distance;
    [SerializeField] private LayerMask groundLayer;

   private void Start()
   {
       rb2D = GetComponent<Rigidbody2D>();
       player = GameObject.FindGameObjectWithTag("Player").transform;
   }

   private void Update()
   {
       Vector2 direction = (player.position - transform.position).normalized;

        rb2D.linearVelocity = new Vector2(
            direction.x * speed,
            direction.y * speed
        );

        Flip(direction.x);
   }

    private void Flip(float directionX)
    {
        if (directionX > 0 && transform.localScale.x < 0 ||
            directionX < 0 && transform.localScale.x > 0)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

}
