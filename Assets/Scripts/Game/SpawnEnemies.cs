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
            TrySpawnGroup();
        }
    }

    void TrySpawnGroup()
    {
        if (EnemyManager.Instance == null) return;

        // Le preguntamos al Manager cuántos enemigos tenemos permitidos soltar en este grupo
        int amountToSpawn = EnemyManager.Instance.GetSpawnAmountAllowed();

        // Hacemos un ciclo para instanciar exactamente esa cantidad de enemigos a la vez
        for (int i = 0; i < amountToSpawn; i++)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        
        // Instancia el enemigo
        int typeOfEnemies = Random.Range(0, enemies.Length); 
        Vector2 spawnPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
            
        Instantiate(enemies[typeOfEnemies], spawnPosition, Quaternion.identity);
        // Avisa al manager que nació (Esto ya lo tenías mapeado)
        EnemyManager.Instance.RegisterEnemySpawn();
    }
}
