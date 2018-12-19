using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void ItemCountChanged(Item item);

public class InventoryScript : MonoBehaviour {
    public event ItemCountChanged itemCountChangedEvent;

    private static InventoryScript instance;

    public static InventoryScript Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryScript>();
            }

            return instance;
        }
    }

    private List<Bag> bagList = new List<Bag>();

    [SerializeField]
    private BagButton[] bagButtons;

    //Debug Only
    [SerializeField]
    private Item[] items;

    private SlotScript moveFromSlot;

    public int EmptySlotCount {
        get {
            int count = 0;

            foreach (Bag bag in bagList) {
                count += bag.ThisBagScript.EmptySlotCount;
            }
            return count;
        }
    }

    public int TotalSlotCount {
        get {
            int count = 0;
            foreach (Bag bag in bagList) {
                count += bag.ThisBagScript.SlotList.Count;
            }

            return count;
        }
    }

    public int UsedSlotCount {
        get {
            return TotalSlotCount - EmptySlotCount;
        }
    }

    public bool CanAddBag{
        get {
            return bagList.Count < 6;
        }
    }

    public SlotScript MoveFromSlot
    {
        get
        {
            return moveFromSlot;
        }

        set
        {
            moveFromSlot = value;

            if (value != null) {
                moveFromSlot.Icon.color = Color.gray;
            }
        }
    }


    //Debug start add bag
    private void Start()
    {
        Bag bag = Instantiate(items[0]) as Bag;
        bag.Initialize(20);
        bag.Use();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.J)){
            Bag bag = Instantiate(items[0]) as Bag;
            bag.Initialize(20);
            bag.Use();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Bag bag = Instantiate(items[0]) as Bag;
            bag.Initialize(8);
            AddItem(bag);
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            HealthPotion potion = Instantiate(items[1]) as HealthPotion;
            AddItem(potion);
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            Armor hemlet = Instantiate(items[2]) as Armor;
            AddItem(hemlet);
        }

        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            // add all gears to bag
            for (int i = 3; i < 10; i++) {
                AddItem(Instantiate(items[i]) as Armor);
            }
        }
    }

    public void AddBag(Bag bag)
    {
        foreach (BagButton bagButton in bagButtons) {
            if (bagButton.ThisBag == null) {
                bagButton.ThisBag = bag;
                bagList.Add(bag);
                bag.ThisBagButton = bagButton;
                break;
            }
        }
    }

    public void AddBag(Bag bag, BagButton bagButton) {
        bagList.Add(bag);
        bagButton.ThisBag = bag;
    }

    public void RemoveBag(Bag currBag) {
        bagList.Remove(currBag);
        Destroy(currBag.ThisBagScript.gameObject);
    }

    public void SwapBags(Bag oldBag, Bag newBag){
        // new total space calculation
        int newSlotCount = TotalSlotCount - oldBag.SlotNum + newBag.SlotNum;
        if (newSlotCount - UsedSlotCount >= 0) {
            List<Item> oldBagItems = oldBag.ThisBagScript.GetItems();

            RemoveBag(oldBag);

            newBag.ThisBagButton = oldBag.ThisBagButton;

            newBag.Use();

            foreach (Item item in oldBagItems) {
                // Avoid duplicate newBag if newBag in oldBag
                if (item != newBag) {
                    AddItem(item);
                }
            }

            AddItem(oldBag);

            HandScript.Instance.DropMoveable();
            Instance.MoveFromSlot = null;
        }
    }

    public bool AddItem(Item item) {
        if (item.StackSize > 0) {
            if (PlaceInStack(item))
            {
                return true;
            }
        }

        return PlaceInEmpty(item);
    }

    private bool PlaceInEmpty(Item item) {
        //search bag for item
        foreach (Bag bag in bagList)
        {
            if (bag.ThisBagScript.AddItem(item)) {
                OnItemCountChanged(item);
                return true;
            }
        }

        return false;
    }

    private bool PlaceInStack(Item item) {
        //search bag for item
        foreach (Bag bag in bagList) {
            foreach (SlotScript slot in bag.ThisBagScript.SlotList) {
                if (slot.StackItem(item))
                {
                    OnItemCountChanged(item);
                    return true;
                }
            }
        }

        return false;
    }

    public void OpenClose() {
        bool haveClosedBag = bagList.Find(x => !x.ThisBagScript.IsOpen);

        // open all when haveCloseBag == true
        // close all when havCloseBag == false
        foreach (Bag bag in bagList)
        {
            if (bag.ThisBagScript.IsOpen != haveClosedBag)
            {
                bag.ThisBagScript.OpenClose();
            }
        }
    }

    public Stack<IUseable> GetUseables(IUseable pickedUseable) {
        Stack<IUseable> useableStk = new Stack<IUseable>();
        // find same item in bag
        foreach (Bag bag in bagList) {
            // find same item in slot
            foreach (SlotScript slot in bag.ThisBagScript.SlotList) {
                if (!slot.IsEmpty && slot.ThisItem.GetType() == pickedUseable.GetType()) {
                    //push if item are same
                    foreach (Item item in slot.ThisItemStk) {
                        useableStk.Push(item as IUseable);
                    }
                }
            }
        }
        return useableStk;
    }

    public void OnItemCountChanged(Item item) {
        if (itemCountChangedEvent == null) {
            return;
        }

        itemCountChangedEvent.Invoke(item);
        
    }
}
