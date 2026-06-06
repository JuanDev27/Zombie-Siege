using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : PlayerSelection
{

    [Header("Combat")]
    [SerializeField] public GameObject bulletPrefab;

    [Header("UI Health & Defense")]
    [SerializeField] public TMP_Text lifeText;

    [Header ("Level up")]
    public int level = 1;
    public float xp_now = 0;
    public float xp_needed = 0; //crear formula que se incremente según el nivel, ej: lvl 1 = 10, lvl 2 = 25,...

    [Header("UI XP")]
    [SerializeField] private Image xpBarFill;
    [SerializeField] public TMP_Text xpText;
    [SerializeField] public TMP_Text lvlText;

    [Header("Panels UI")]
    [Tooltip("Arrastra aquí el panel con la vida, XP, etc., para poder ocultarlo al morir.")]
    [SerializeField] private GameObject panelHUD;
    
    [Tooltip("Arrastra aquí tu panel oculto de Game Over.")]
    [SerializeField] private GameObject panelGameOver;

    private Rigidbody2D rb;
    private SpawnEnemies spawnManager; // Cacheamos el spawner para saber si hay enemigos
    private bool isInvulnerable = false;
    private bool isKnockedBack = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(AutoShoot());
        ActualizarXpBar();
        //Vida inicial
        lifeText.text = life.ToString();
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
        //Si el daño es mayor a la defensa
        if((damage - defense) > 0 )
        {
            life = life - (damage - defense);
        }
        else
        {
            life = life - 1; //Sino daño minimo
        }
        lifeText.text = life.ToString();

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

        // Esperar el resto del tiempo configurado en el inspector para quitar la invulnerabilidad
        float remainingCooldown = Mathf.Max(0f, damageCooldown - 0.15f);
        yield return new WaitForSeconds(remainingCooldown);
        isInvulnerable = false;
    }

    public bool IsInvulnerable() => isInvulnerable;
    public bool CanMove() => !isKnockedBack;

    void Die()
    {
        // Detener el tiempo del juego
        Time.timeScale = 0f;

        // Mostrar pantalla Game Over
        panelHUD.SetActive(false);
        panelGameOver.SetActive(true);
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
            LevelUp_stats(); //Llamar función para actualizar al subir de nivel las stats
            //Actualizar vida
            lifeText.text = life.ToString();
            lvlText.text = "LVL: " + level.ToString(); //Actualizar UI de nivel
            ActualizarXpBar(); //Actualizar a estado vacio despues de subir de lvl
        }
    }

    void LevelUp_stats()
    {
        //Vida
        life = level * 20; //Formula temp de vida(a futuro incrementar con 25% o algo asi)
        //
    }

    //Update stats
    public void UpdateDef()
    {
        defense++;
    }

    //Skills(FALTA AQUI)

    public void UpdateDMG()
    {
        bulletPrefab.GetComponent<Bullet>().UpdateDMG();
    }

    //Llenar XP bar
    public void ActualizarXpBar()
    {
        if (xpBarFill != null)
        {
            xpBarFill.fillAmount = (float)xp_now / xp_needed;
            xpText.text = "XP: " + xp_now.ToString() + "/" + xp_needed;
        }
    }
}
