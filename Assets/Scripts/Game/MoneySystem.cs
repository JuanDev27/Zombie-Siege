using UnityEngine;
using TMPro;

public class MoneySystem : MonoBehaviour
{
    [SerializeField] public TMP_Text moneyText;

    public int money = 0;
    private float bonusMultiplier = 1.0f; // Multiplicador de bonificación

    public void AddMoney(int amount)
    {
        int totalAmount = Mathf.RoundToInt(amount * bonusMultiplier);
        money += totalAmount;
    }
    public int SubstractMoney(int amount)
    {
        if(money < amount)
        {
            return 0; //No hay suficiente dinero
        }
        //Sino, descontar
        money -= amount;
        return 1; //Exito
    }
    public int GetMoney()
    {
        return money;
    }

    void Update()
    {
        moneyText.text = "$" + money.ToString();
    }

}
