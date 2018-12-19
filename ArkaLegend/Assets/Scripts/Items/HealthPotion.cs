using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthPotion",menuName = "Items/Potion",order = 1)]
public class HealthPotion : Item,IUseable {
    [SerializeField]
    private int health;

    public void Use()
    {
        Stats playerHp = Player.Instance.Hp;
        if (playerHp.CurrVal < playerHp.MaxVal) {
            playerHp.CurrVal += health;
            Remove();
        }
    }

    public override string GetDescription()
    {
        string functionDesc = 
            string.Format("<color={0}>Restore {1} health</color>", UIManager.FunctionalDescColor,health);
        
        return base.GetDescription() + string.Format("\n{0}\n{1}",ThisQuality+" item",functionDesc);
    }
}
