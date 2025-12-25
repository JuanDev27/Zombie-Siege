using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int enemyLife = 20;
    private MoneySystem money;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyLife <= 0)
            {
                Die();
            }
    }

    //Detectar colisiones con el jugador
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 playerPosition = collision.transform.position;
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(playerPosition, 1); //Daño 1
        }
        Debug.Log("Enemy Life: " + enemyLife);
    }
    void Die()
    {
        money = GameObject.FindFirstObjectByType<MoneySystem>();
        // Destruir enemigo
        Destroy(gameObject);
        //Dar dinero
        money.AddMoney(10); 
    }
}
