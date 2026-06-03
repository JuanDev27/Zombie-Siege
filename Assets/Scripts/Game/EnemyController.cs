using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private int maxLife = 20;
    [SerializeField] private int moneyReward = 10;
    [SerializeField] private int damageToPlayer = 1;

    public int currentLife;
    private MoneySystem money;
    private SpawnEnemies spawnEnemies;


    private PlayerController playerController;

    [Header("UI")]
    [SerializeField] private Image healthBarFill;

    private void Start()
    {
        currentLife = maxLife;
        spawnEnemies = GameObject.FindFirstObjectByType<SpawnEnemies>();
        money = GameObject.FindFirstObjectByType<MoneySystem>();
        playerController = GameObject.FindFirstObjectByType<PlayerController>();

        if (EnemyManager.Instance != null)
        {
        EnemyManager.Instance.RegisterEnemySpawn();
        }
        ActualizarBarraVida();
    }

    // Método para recibir daño y actualizar la vida del enemigo hasta que muera
    public void TakeDamage(int damage)
    {
        currentLife -= damage;
        ActualizarBarraVida();

        if (currentLife <= 0)
        {
            Die();
        }
    }


    //Detectar colisiones con el jugador
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(transform.position, damageToPlayer);
            }
        }
        ActualizarBarraVida();
    }

// Nueva función encargada de hacer el cálculo matemático para "desllenar" la barra
    private void ActualizarBarraVida()
    {
        if (healthBarFill != null)
        {
            // El Fill Amount va de 0.0 a 1.0, por lo que dividimos la vida actual entre la máxima.
            // Usamos (float) para que Unity haga la división con decimales exactos.
            healthBarFill.fillAmount = (float)currentLife / maxLife;
        }
    }

    private void Die()
    {
        //Dar dinero
        if (money != null) money.AddMoney(moneyReward); 
        //Dar xp
        if (playerController != null)
        {
            float xp_enemy = Random_xp();
            playerController.Level_up(xp_enemy);
        }
        if (EnemyManager.Instance != null)
        {
        EnemyManager.Instance.RegisterEnemyDeath();
        }
        //Actualizar XP Bar del jugador al matar un enemigo
        playerController.ActualizarXpBar();
        // Destruir enemigo
        Destroy(gameObject);
    }

    float Random_xp()
    {
        float xp = Random.Range(1, 7); //El 7 no se incluye es como [1,7) se toma hasta el 6
        return xp;
    }
}
