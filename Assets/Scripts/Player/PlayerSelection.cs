using UnityEngine;

public class PlayerSelection : MonoBehaviour
{
    //Variables a completar
    [Header("Combat Settings")]
    [SerializeField] protected float recoil = 0.5f;

    [Header("Health & Defense")]
    public int life = 20;
    [SerializeField] protected float damageCooldown = 0.5f; // Invulnerabilidad tras recibir daño
    [SerializeField] protected float knockbackForce = 25f;

}