using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float recoil = 3f; // segundos entre disparos
    private bool takingDamage = false;
    public float damageCooldown = 0.5f; // 0.5 segundos de invulnerabilidad después de recibir daño
    private Rigidbody2D rb;
    public int life = 20;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(AutoShoot());
    }

    IEnumerator AutoShoot()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(recoil);
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Init();
    }

    public void TakeDamage(Vector2 direction,int damage)
    {
        if (!takingDamage){
        //Empuje
        takingDamage = true;
        Vector2 pushForce = new Vector2(transform.position.x -direction.x, transform.position.y - direction.y).normalized;
        rb.AddForce(pushForce,ForceMode2D.Impulse);
        //Daño
        life -= damage;
        StartCoroutine(DamageCooldown());
        //Muerte
        if (life <= 0)
            {
                Die();
            }
        }
        Debug.Log("Player Life: " + life);
    }

    public void disableDamage(){
        takingDamage = false;
    }

    void Die()
    {
        // Detener el tiempo del juego
        Time.timeScale = 0f;

        // Mostrar pantalla Game Over
        //gameOverPanel.SetActive(true);

        // Opcional: desactivar al jugador
        gameObject.SetActive(false);
    }
    IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(damageCooldown);
        takingDamage = false;
    }
}
