using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //Stats
    public int enemyLife = 20;
    private MoneySystem money;
    private SpawnEnemies spawnEnemies;

    private void Start()
    {
        spawnEnemies = GameObject.FindFirstObjectByType<SpawnEnemies>();
        money = GameObject.FindFirstObjectByType<MoneySystem>();
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
    private void Die()
    {
        //Dar dinero
        money.AddMoney(10); 
        spawnEnemies.SetEnemiesLeft();
        // Destruir enemigo
        Destroy(gameObject);
    }

    public void SetEnemyLife(int life)
    {
        enemyLife = life;
    }
}
