using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    private Transform target;
    public int damage = 5; // Daño que causa el proyectil
    private EnemyController enemy;

    public void Init(int daño)
    {
        damage = daño;
        FindNearestEnemy();
    }

    void Update()
    {
        if (target == null) {
            Destroy(gameObject);
            return;
        }
        Vector2 direction = (target.position - transform.position).normalized;
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        // Si llega al enemigo
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {

            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    void FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null)
        {
            target = nearestEnemy.transform;
            enemy = nearestEnemy.GetComponent<EnemyController>();
        }
    }
}
