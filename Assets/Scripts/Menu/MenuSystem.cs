using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void Opciones()
    {
        Debug.Log("Opciones del juego");
    }

}
