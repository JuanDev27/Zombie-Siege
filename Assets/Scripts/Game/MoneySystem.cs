using UnityEngine;
using TMPro;

public class MoneySystem : MonoBehaviour
{
    private int money = 0;
    private float bonusMultiplier = 1.0f; // Multiplicador de bonificación
    public TMP_Text moneyText;


    public void AddMoney(int amount)
    {
        int totalAmount = Mathf.RoundToInt(amount * bonusMultiplier);
        money += totalAmount;
    }
    public void SubstractMoney(int amount)
    {
        money -= amount;
        if (money < 0) money = 0;
        Debug.Log("Dinero actual: " + money);
    }
    public int GetMoney()
    {
        return money;
    }

    void Update()
    {
        moneyText.text = "Money: " + money.ToString();
    }

}
