using UnityEngine;

public class ShopSystem : MonoBehaviour
{

    public PlayerController playerController;
    public MoneySystem moneySys;
    //Idea: Sistema simple de mejoras: Mejorar arma(más daño), mejorar o comprar armadura(def-escudo) y skills(regeneración,recoil,etc.)

    public void armorUpdate()
    {
        int respuesta = moneySys.SubstractMoney(20); //0 fallo, 1 exito
        if(respuesta == 1)
        {
            playerController.UpdateDef();
        }
        else
        {
            Debug.Log("No tienes suficiente dinero");
        }
    } 

    public void skillBuy()
    {
        Debug.Log("Skill(New)");
    }

    public void gunUpdate()
    {
        int respuesta = moneySys.SubstractMoney(30); //0 fallo, 1 exito
        if(respuesta == 1)
        {
            playerController.UpdateDMG();
        }
        else
        {
            Debug.Log("No tienes suficiente dinero");
        }
    }

}
