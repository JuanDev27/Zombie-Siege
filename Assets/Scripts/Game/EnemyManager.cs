using UnityEngine;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    // Instancia estática (Patrón Singleton) para que cualquiera lo acceda sin buscarlo
    public static EnemyManager Instance { get; private set; }

    [Header("Wave Configuration")]
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text enemiesLeftText;

    [Header("Difficulty Progression Settings")]
    [Tooltip("¿Cuántas oleadas como mínimo debe mantenerse el tamaño actual antes de poder subir?")]
    [SerializeField] private int wavesToAdapt = 2;

    public int wave = 1;
    private int totalEnemiesOfWave = 6;
    private int enemiesSpawnedInWave = 0;
    private int enemiesLeftToKill;
    private int totalEnemiesInScene = 0;
    
    private int currentSpawnGroupSize = 1;
    private int wavesStableCount = 0;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        SetupWaveData(); 
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

    void SetupWaveData()
    {
        // Calcular total de enemigos en la oleada
        totalEnemiesOfWave = Mathf.RoundToInt(3 + (wave * 1.5f));

        if (wave == 1)
        {
            currentSpawnGroupSize = 1;
            wavesStableCount = 1;
        }
        else
        {
            // Regla 1: Si no se ha cumplido el tiempo de adaptación, obligatoriamente se mantiene igual
            if (wavesStableCount < wavesToAdapt)
            {
                wavesStableCount++;
            }
            else
            {
                int decision = Random.Range(0, 2); // Devuelve 0 o 1

                if (decision == 1 || wavesStableCount ==2)
                {
                    currentSpawnGroupSize++; // Sube un escalón de dificultad
                    wavesStableCount = 0;    // Reseteamos el contador porque el tamaño cambió
                }
                else
                {
                    // Decidió quedarse igual una oleada más
                    wavesStableCount++; 
                }
            }

            // Aseguramos por lógica que el grupo no sea mayor que el total de enemigos de la oleada
            currentSpawnGroupSize = Mathf.Clamp(currentSpawnGroupSize, 1, totalEnemiesOfWave);
        }

        // Mostrar en consola el estado actual para verificar la progresión orgánica
        Debug.Log($"[Oleada {wave}] Total enemigos: {totalEnemiesOfWave} | Grupos de: {currentSpawnGroupSize} | Rondas estables: {wavesStableCount}");

        enemiesLeftToKill = totalEnemiesOfWave;
        enemiesSpawnedInWave = 0;
    }

    // Métodos ultrarrápidos que llamarán los enemigos al nacer/morir
    public void RegisterEnemySpawn() => totalEnemiesInScene ++;
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
        SetupWaveData();
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

    // Devuelve cuántos enemigos se pueden spawnear en este instante (máximo el tamaño del grupo)
    public int GetSpawnAmountAllowed()
    {
        int remainingToSpawn = totalEnemiesOfWave - enemiesSpawnedInWave;
        
        if (remainingToSpawn <= 0) return 0;

        // Devuelve el tamaño del grupo calculado aleatoriamente para esta oleada,
        // o lo que quede disponible si ya faltan pocos enemigos para terminar la ola.
        int amountToSpawn = Mathf.Min(currentSpawnGroupSize, remainingToSpawn);
        
        enemiesSpawnedInWave += amountToSpawn;
        return amountToSpawn;
    }
}