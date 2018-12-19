using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Each enemy will have its loot table
public class LootTable : MonoBehaviour {
    [SerializeField]
    private Loot[] loots;

    private List<Item> droppedItemList = new List<Item>();

    private bool lootRolled = false;

    public void ShowLoot() {
        if (!lootRolled) {
            RollLoot();
            lootRolled = true;
        }
        
        LootWindow.Instance.CreateLootList(droppedItemList);
    }

    private void RollLoot() {
        int maxRange = 10000;
        foreach (Loot loot in loots)
        {
            int roll = Random.Range(0, maxRange+1);
            if(roll <= loot.DropChange * maxRange)
            {
                droppedItemList.Add(loot.ThisItem);
            }
        }
    }
}
