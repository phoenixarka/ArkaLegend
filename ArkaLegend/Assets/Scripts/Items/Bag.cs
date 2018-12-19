using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bag",menuName = "Items/Bag", order = 1)]
public class Bag : Item, IUseable {
    [SerializeField]
    private int slotAmount;

    [SerializeField]
    protected GameObject bagPrefab;

    public BagScript ThisBagScript { get; set; }

    public BagButton ThisBagButton { get; set; }

    public int SlotNum
    {
        get
        {
            return slotAmount;
        }
    }

    public void Initialize(int slotNum) {
        if (slotNum != 0)
        {
            this.slotAmount = slotNum;
        }
    }

    public void Use()
    {
        if (InventoryScript.Instance.CanAddBag == true) {
            Remove();
            ThisBagScript = Instantiate(bagPrefab, InventoryScript.Instance.transform).GetComponent<BagScript>();
            ThisBagScript.AddSlot(slotAmount);

            // if bagButton is empty
            if (ThisBagButton == null)
            {
                InventoryScript.Instance.AddBag(this);
            }
            else {
                InventoryScript.Instance.AddBag(this,ThisBagButton);
            }

            
        }
    }

    public override string GetDescription()
    {
        string functionDesc = 
            string.Format("<color={0}>{1} slot bag</color>", UIManager.FunctionalDescColor, slotAmount);

        return base.GetDescription() + string.Format("\n{0}\n{1}", ThisQuality + " item", functionDesc);
    }

}
