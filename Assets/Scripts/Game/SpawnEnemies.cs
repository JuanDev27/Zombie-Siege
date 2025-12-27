using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class SpawnEnemies : MonoBehaviour
{
    private float minX, maxX, minY, maxY;
    [SerializeField] private Transform[] puntosSpawn;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private float spawnInterval = 5f;
    private float timer;
    private int wave = 1;
    private int totalEnemies = 6;
    private int enemiesSpawned = 0;
    private int enemiesLeft;
    public TMP_Text waveText;
    public TMP_Text enemiesLeftText;
    void Start()
    {
        maxX = puntosSpawn.Max(puntosSpawn => puntosSpawn.position.x);
        minX = puntosSpawn.Min(puntosSpawn => puntosSpawn.position.x);
        maxY = puntosSpawn.Max(puntosSpawn => puntosSpawn.position.y);
        minY = puntosSpawn.Min(puntosSpawn => puntosSpawn.position.y);
        enemiesLeft = totalEnemies;
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
        if(enemiesSpawned < totalEnemies)
        {
            int numberOfEnemies = Random.Range(0,enemies.Length); 
            Vector2 spawnPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
            Instantiate(enemies[numberOfEnemies],spawnPosition, Quaternion.identity);
            enemiesSpawned++;
        }
    }

    public void SetEnemiesLeft()
    {
        enemiesLeft--;
        if (enemiesLeft <= 0)
        {
            EndWave();
        }
        enemiesLeftText.text = "Enemies: " + enemiesLeft;
    }
    public int GetEnemiesLeft()
    {
        return enemiesLeft;
    }
    void EndWave()
    {
        wave++;
        totalEnemies = Mathf.RoundToInt(totalEnemies * (1 + (wave - 1) * 0.2f));
        enemiesLeft = totalEnemies;
        Debug.Log("Wave:" + wave);
    }

}
