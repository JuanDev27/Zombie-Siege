using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Combat Settings")]
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] private float recoil = 0.5f;

    [Header("Health & Defense")]
    public int life = 20;
    [SerializeField] private float damageCooldown = 0.5f; // Invulnerabilidad tras recibir daño
    [SerializeField] private float knockbackForce = 25f;

    [Header ("Level up")]
    public int level = 1;
    public float xp_now = 0;
    public float xp_needed = 0; //crear formula que se incremente según el nivel, ej: lvl 1 = 10, lvl 2 = 25,...

    private Rigidbody2D rb;
    private SpawnEnemies spawnManager; // Cacheamos el spawner para saber si hay enemigos
    private bool isInvulnerable = false;
    private bool isKnockedBack = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(AutoShoot());
    }

    IEnumerator AutoShoot()
    {
        while (true)
        {
            if (EnemyManager.Instance != null && EnemyManager.Instance.HasEnemiesInScene())
            {
                Shoot();
                yield return new WaitForSeconds(recoil);
            }
            else
        {
            yield return new WaitForSeconds(0.1f);
        }
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null) return;
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>()?.Init();
    }

    public void TakeDamage(Vector2 enemyPosition,int damage)
    {
        if (isInvulnerable) return;
        life -= damage;
        Debug.Log("Player Life: " + life);

        if (life <= 0)
        {
            Die();
            return;
        }

        // Aplicar Knockback de forma efectiva
        StartCoroutine(KnockbackRoutine(enemyPosition));
    }

    IEnumerator KnockbackRoutine(Vector2 enemyPosition)
    {
        isInvulnerable = true;
        isKnockedBack = true;

        // Calcular dirección opuesta al enemigo
        Vector2 pushDirection = ((Vector2)transform.position - enemyPosition).normalized;
        
        // Aplicar fuerza física real
        rb.linearVelocity = Vector2.zero; // Limpiamos velocidades previas
        rb.AddForce(pushDirection * knockbackForce, ForceMode2D.Impulse);

        // Duración del descontrol por el golpe (0.15 segundos de empuje puro)
        yield return new WaitForSeconds(0.15f);
        isKnockedBack = false;

        // Esperar el resto del tiempo de invulnerabilidad
        yield return new WaitForSeconds(damageCooldown - 0.15f);
        isInvulnerable = false;
    }

    public bool CanMove() => !isKnockedBack;

    void Die()
    {
        // Detener el tiempo del juego
        Time.timeScale = 0f;

        // Mostrar pantalla Game Over
        //gameOverPanel.SetActive(true);

        // Desactivar al jugador
        gameObject.SetActive(false);
    }

    public void Level_up(float xp)
    {
        xp_now += xp;
        if(xp_now >= xp_needed)
        {
            level += 1;
            xp_now = 0;
            xp_needed = level * 10; //Formula temp de xp necesaria
            Update_stats(); //Llamar función para actualizar al subir de nivel las stats
        }
    }

    void Update_stats()
    {
        //Vida
        life = level * 20; //Formula temp de vida(a futuro incrementar con 25% o algo asi)
        //
    }
}
