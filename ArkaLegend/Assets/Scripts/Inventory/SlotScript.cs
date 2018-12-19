using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotScript : MonoBehaviour,IPointerClickHandler,IClickable,IPointerEnterHandler,IPointerExitHandler
{
    private EventStack<Item> itemStk = new EventStack<Item>(); 

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Image icon_background;

    [SerializeField]
    private Text stackSizeText;

    // Which bag this slot belongs to
    public BagScript ThisBagScript { get; set; }

    public bool IsEmpty {
        get{
            return ThisItemStk.Count == 0;
        }
    }

    public bool IsFull {
        get {
            return !IsEmpty && ThisCount >= ThisItem.StackSize;
        }
    }

    public Item ThisItem {
        get {
            if (!IsEmpty)
            {
                return ThisItemStk.Peek();
            }
            return null;
        }
    }

    public Image Icon
    {
        get
        {
            return icon;
        }

        set
        {
            icon = value;
        }
    }

    public int ThisCount {
        get {
            return ThisItemStk.Count;
        }
    }

    public Text StackText
    {
        get
        {
            return stackSizeText;
        }
    }

    public EventStack<Item> ThisItemStk
    {
        get
        {
            return itemStk;
        }
    }

    public void Awake()
    {
        ThisItemStk.OnPop += new UpdateStackEvent(UpdateSlot);
        ThisItemStk.OnPush += new UpdateStackEvent(UpdateSlot);
        ThisItemStk.OnClear += new UpdateStackEvent(UpdateSlot);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            IMoveable currInHandMoveable = HandScript.Instance.ThisMoveable;
            // if inventory not mark "move from" and clicked on slot "Contains Something"
            if (InventoryScript.Instance.MoveFromSlot == null && !IsEmpty)
            {
                // if carried something
                if (currInHandMoveable != null) {
                    if (currInHandMoveable is Bag && (ThisItem is Bag))
                    {
                        // check clicked item is bag
                        //swap bag
                        InventoryScript.Instance.SwapBags(currInHandMoveable as Bag, ThisItem as Bag);
                    }
                    else if (currInHandMoveable is Armor && ThisItem is Armor && 
                             (ThisItem as Armor).ThisArmorType == (currInHandMoveable as Armor).ThisArmorType) {
                        (ThisItem as Armor).Equip();
                        HandScript.Instance.DropMoveable();
                    }
                } 
                // if not carry anything
                else {
                    // pickup
                    HandScript.Instance.TakeMoveable(ThisItem as IMoveable);
                    InventoryScript.Instance.MoveFromSlot = this;
                }
            }
            // if inventory not mark "move from" and clicked on "Empty Slot" and have "Bag" on hand
            else if (InventoryScript.Instance.MoveFromSlot == null && IsEmpty) {
                if (currInHandMoveable is Bag)
                {
                    // Dequip a bag from inventory
                    Bag bag = currInHandMoveable as Bag;
                    bool canDeequipBag = InventoryScript.Instance.EmptySlotCount - bag.SlotNum > 0;

                    if (bag.ThisBagScript != ThisBagScript && canDeequipBag)
                    {
                        AddItem(bag);
                        bag.ThisBagButton.RemoveBag();
                        HandScript.Instance.DropMoveable();

                    }
                    else if (!canDeequipBag)
                    {
                        Debug.Log("Warning Text: Insufficient space to de-equip bag");
                    }
                    else
                    {
                        Debug.Log("Warning Text: You cannot put bag into its own slot");
                    }
                }
                // Dequip armor
                else if (currInHandMoveable is Armor) {
                    Armor armor = currInHandMoveable as Armor;
                    AddItem(armor);
                    CharacterPanel.Instance.SelectedBtn.DequipArmor();
                    HandScript.Instance.DropMoveable();
                }                
            }
            // if we already carry something in hand
            else if (InventoryScript.Instance.MoveFromSlot != null)
            {
                if (PutItemBack() ||
                    MergeItems(InventoryScript.Instance.MoveFromSlot) ||
                    SwapItems(InventoryScript.Instance.MoveFromSlot) ||
                    AddItems(InventoryScript.Instance.MoveFromSlot.ThisItemStk))
                {
                    HandScript.Instance.DropMoveable();
                    InventoryScript.Instance.MoveFromSlot = null;
                }
            }

        }

        if (eventData.button == PointerEventData.InputButton.Right && HandScript.Instance.ThisMoveable == null)
        {
            UseItem();
        }
    }

    public bool AddItem(Item item) {
        ThisItemStk.Push(item);
        icon.sprite = item.Icon;
        icon.color = Color.white;
        item.ThisSlot = this;
        return true;
    }

    public bool AddItems(EventStack<Item> currItems) {
        if (IsEmpty || currItems.Peek().GetType() == ThisItem.GetType()) {
            while (currItems.Count > 0) {
                if (IsFull)
                {
                    return false;
                }
                AddItem(currItems.Pop());
            }
            return true;
        }

        return false;
    }

    public void RemoveItem(Item item) {
        if (!IsEmpty) {
            InventoryScript.Instance.OnItemCountChanged(ThisItemStk.Pop());
        }
    }

    public void Clear() {
        if (ThisItemStk.Count > 0) {
            InventoryScript.Instance.OnItemCountChanged(ThisItemStk.Pop());
            ThisItemStk.Clear();
        }
    }

    public void UseItem() {
        if (ThisItem is IUseable) {
            (ThisItem as IUseable).Use();
        }

        if (ThisItem is Armor) {
            (ThisItem as Armor).Equip();
        }
    }

    public bool StackItem(Item item)
    {
        if (!IsEmpty && item.name == ThisItem.name && ThisItemStk.Count < ThisItem.StackSize) {
            ThisItemStk.Push(item);
            item.ThisSlot = this;
            return true;
        }

        return false;
    }

    private bool PutItemBack() {
        if (InventoryScript.Instance.MoveFromSlot == this) {
            InventoryScript.Instance.MoveFromSlot.Icon.color = Color.white;
            return true;
        }

        return false;
    }

    private bool SwapItems(SlotScript from) {
        if (IsEmpty) {
            return false;
        }

        if (from.ThisItem.GetType() != ThisItem.GetType() || from.ThisCount+ThisCount > ThisItem.StackSize) {

            // from -> tmp
            EventStack<Item> tmpFrom = new EventStack<Item>(from.ThisItemStk);
            from.ThisItemStk.Clear();
            // curr -> from
            from.AddItems(ThisItemStk);
            ThisItemStk.Clear();
            // tmp -> curr 
            AddItems(tmpFrom);
            return true;
        }

        return false;
    }

    private bool MergeItems(SlotScript from) {
        if (IsEmpty) {
            return false;
        }

        if (from.ThisItem.GetType() == ThisItem.GetType() && !IsFull) {
            while (ThisItem.StackSize > ThisCount) {
                AddItem(from.ThisItemStk.Pop());
            }
            /*
            int freeSpace = ThisItem.StackSize - Count;
            for (int i = 0; i < freeSpace; i++) {
                AddItem(from.itemStk.Pop());
            }
            */
            return true;
        }

        return false;
    }

    private void UpdateSlot() {
        UIManager.Instance.UpdateStackSize(this);
    }

    // Mouse Hover Enter 
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsEmpty) {
            return;
        }

        UIManager.Instance.ShowTooltip(transform.position,ThisItem);
    }

    // Mouse Hover Exit
    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideTooltip();
    }
}
