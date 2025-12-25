using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpawnEnemies : MonoBehaviour
{
    private float minX, maxX, minY, maxY;
    [SerializeField] private Transform[] puntosSpawn;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private float spawnInterval = 5f;
    private float timer;
    void Start()
    {
        maxX = puntosSpawn.Max(puntosSpawn => puntosSpawn.position.x);
        minX = puntosSpawn.Min(puntosSpawn => puntosSpawn.position.x);
        maxY = puntosSpawn.Max(puntosSpawn => puntosSpawn.position.y);
        minY = puntosSpawn.Min(puntosSpawn => puntosSpawn.position.y);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        int numberOfEnemies = Random.Range(0,enemies.Length); 
        Vector2 spawnPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        Instantiate(enemies[numberOfEnemies],spawnPosition, Quaternion.identity);
    }

}
