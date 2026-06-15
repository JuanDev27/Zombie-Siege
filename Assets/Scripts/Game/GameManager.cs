using UnityEngine;

public class GameManager : MonoBehaviour
{
// Instancia accesible desde cualquier script
    public static GameManager Instance { get; private set; }

    // Aquí guardamos el daño real que se mantendrá entre escenas
    public int playerDamage = 5; 

    void Awake()
    {
        // Esto evita que el GameManager se destruya al cambiar de escena
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
