using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagScript : MonoBehaviour {

    [SerializeField]
    private GameObject slotPrefab;

    private CanvasGroup canvasGroup;

    private List<SlotScript> slotList = new List<SlotScript>();

    public int EmptySlotCount {
        get {
            int count = 0;
            foreach (SlotScript slot in slotList) {
                if (slot.IsEmpty) {
                    count++;
                }
            }
            return count; 
        }
    }

    public bool IsOpen {
        get{
            return canvasGroup.alpha > 0;
        }
    }

    public List<SlotScript> SlotList
    {
        get
        {
            return slotList;
        }
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public List<Item> GetItems() {
        List<Item> items = new List<Item>();
        foreach (SlotScript slot in slotList) {
            if (!slot.IsEmpty) {
                foreach (Item item in slot.ThisItemStk) {
                    items.Add(item);
                }
            }
        }

        return items;
    }

    public void AddSlot(int slotNum){
        for (int i = 0; i < slotNum; i++) {
            SlotScript slot = Instantiate(slotPrefab, transform).GetComponent<SlotScript>();
            slot.ThisBagScript = this;
            SlotList.Add(slot);            
        }
    }

    public bool AddItem(Item item)
    {
        foreach (SlotScript slot in SlotList)
        {
            if (slot.IsEmpty) {
                slot.AddItem(item);
                return true;
            }
        }
        return false;
    }

    public void OpenClose()
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;

    }


}
