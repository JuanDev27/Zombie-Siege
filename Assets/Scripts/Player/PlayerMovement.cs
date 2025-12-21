using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    Vector2 move;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {  
        move = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) move.y += 1;
        if (Keyboard.current.sKey.isPressed) move.y -= 1;
        if (Keyboard.current.dKey.isPressed) move.x += 1;
        if (Keyboard.current.aKey.isPressed) move.x -= 1;

        Vector3 posicion = transform.position;
        posicion.x += move.x * speed * Time.deltaTime;
        posicion.y += move.y * speed * Time.deltaTime;
        transform.position = posicion;
   
    }
}
