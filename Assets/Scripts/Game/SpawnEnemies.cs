using UnityEngine;
using System.Linq;

public class SpawnEnemies : MonoBehaviour
{
    [Header("Spawn Configuration")]
    [SerializeField] private Transform[] puntosSpawn;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private float spawnInterval = 5f;

    private float minX, maxX, minY, maxY;
    private float timer;

    void Start()
    {
        if (puntosSpawn == null || puntosSpawn.Length == 0)
        {
            Debug.LogError($"¡El objeto {gameObject.name} no tiene puntos de spawn asignados!");
            enabled = false;
            return;
        }

        // Cada clon calcula sus propios límites basados en SUS propios puntos del inspector
        maxX = puntosSpawn.Max(p => p.position.x);
        minX = puntosSpawn.Min(p => p.position.x);
        maxY = puntosSpawn.Max(p => p.position.y);
        minY = puntosSpawn.Min(p => p.position.y);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            TrySpawnEnemy();
        }
    }

    void TrySpawnEnemy()
    {
        if (EnemyManager.Instance == null) return;

        // Le pedimos permiso al Manager Central. Si la ola aún necesita enemigos, nos dará el 'true'
        if (EnemyManager.Instance.CanSpawnEnemy())
        {
            int numberOfEnemies = Random.Range(0, enemies.Length); 
            Vector2 spawnPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
            
            Instantiate(enemies[numberOfEnemies], spawnPosition, Quaternion.identity);
        }
    }
}
