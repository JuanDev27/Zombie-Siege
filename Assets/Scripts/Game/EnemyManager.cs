using UnityEngine;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    // Instancia estática (Patrón Singleton) para que cualquiera lo acceda sin buscarlo
    public static EnemyManager Instance { get; private set; }

    [Header("Wave Configuration")]
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text enemiesLeftText;
    
    private int wave = 1;
    private int totalEnemiesOfWave = 6;
    private int enemiesSpawnedInWave = 0;
    private int enemiesLeftToKill;
    private int totalEnemiesInScene = 0;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        enemiesLeftToKill = totalEnemiesOfWave;
        UpdateUI();
    }

    public bool CanSpawnEnemy()
    {
        if (enemiesSpawnedInWave < totalEnemiesOfWave)
        {
            enemiesSpawnedInWave++;
            return true;
        }
        return false;
    }

    // Métodos ultrarrápidos que llamarán los enemigos al nacer/morir
    public void RegisterEnemySpawn() => totalEnemiesInScene++;
    public void RegisterEnemyDeath()
    {
        totalEnemiesInScene--;
        enemiesLeftToKill--;

        if (enemiesLeftToKill <= 0)
        {
            EndWave();
        }
        else
        {
            UpdateUI();
        }
    }

    //Fin de oleada
    void EndWave()
    {
        wave++;
        totalEnemiesOfWave = Mathf.RoundToInt(totalEnemiesOfWave * (1 + (wave - 1) * 0.2f));
        enemiesLeftToKill = totalEnemiesOfWave;
        enemiesSpawnedInWave = 0; // Se resetea el contador global de spawns
        
        UpdateUI();
    }

    void UpdateUI()
    {
        if (waveText != null) waveText.text = "Wave: " + wave;
        if (enemiesLeftText != null) enemiesLeftText.text = "Enemies: " + enemiesLeftToKill;
    }

    // El jugador consultará esto. No importa si hay 1 o 50 spawners.
    public bool HasEnemiesInScene() => totalEnemiesInScene > 0;
    
    public int GetCurrentEnemyCount() => totalEnemiesInScene;
}