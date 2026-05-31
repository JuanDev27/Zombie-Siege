using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private int maxLife = 20;
    [SerializeField] private int moneyReward = 10;
    [SerializeField] private int damageToPlayer = 1;

    public int currentLife;
    private MoneySystem money;
    private SpawnEnemies spawnEnemies;

    //Stats old
    //public int enemyLife = 20;

    private void Start()
    {
        currentLife = maxLife;
        spawnEnemies = GameObject.FindFirstObjectByType<SpawnEnemies>();
        money = GameObject.FindFirstObjectByType<MoneySystem>();

        if (EnemyManager.Instance != null)
        {
        EnemyManager.Instance.RegisterEnemySpawn();
        }
    }

    // Update is called once per frame (old)
    //void Update()
    //{
      //  if (enemyLife <= 0)
        //    {
          //      Die();
            //}
    //}

    // Método para recibir daño y actualizar la vida del enemigo hasta que muera
    public void TakeDamage(int damage)
    {
        currentLife -= damage;
        Debug.Log("Enemy Life: " + currentLife);

        if (currentLife <= 0)
        {
            Die();
        }
    }


    //Detectar colisiones con el jugador
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(collision.transform.position, damageToPlayer);
            }
        }
        Debug.Log("Enemy Life: " + currentLife);
    }

    private void Die()
    {
        //Dar dinero
        if (money != null) money.AddMoney(moneyReward); 
        if (EnemyManager.Instance != null)
        {
        EnemyManager.Instance.RegisterEnemyDeath();
        }
        // Destruir enemigo
        Destroy(gameObject);
    }
}
