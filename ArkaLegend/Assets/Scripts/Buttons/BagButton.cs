using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class BagButton : MonoBehaviour,IPointerClickHandler {
    private Bag bag;

    [SerializeField]
    private Sprite full, empty;

    public Bag ThisBag
    {
        get
        {
            return bag;
        }

        set
        {
            if (value != null)
            {
                GetComponent<Image>().sprite = full;
                //show bag Icon in slot
                Image bagIcon = GetComponentsInChildren<Image>()[1];
                bagIcon.sprite = value.Icon;
                bagIcon.color = Color.white;
            }
            else {
                GetComponent<Image>().sprite = empty;
                GetComponentsInChildren<Image>()[1].color = new Color(0,0,0,0);
            }
            bag = value;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) {

            IMoveable moveableItemInHand = HandScript.Instance.ThisMoveable;
            // if click with a bag in hand
            if (InventoryScript.Instance.MoveFromSlot != null &&
                moveableItemInHand != null &&
                moveableItemInHand is Bag) {
                // if current button contains bag
                if (ThisBag != null)
                {
                    InventoryScript.Instance.SwapBags(ThisBag, moveableItemInHand as Bag);
                }
                else {
                    Bag bagInHand = moveableItemInHand as Bag;
                    bagInHand.ThisBagButton = this;
                    bagInHand.Use();
                    ThisBag = bagInHand;

                    // clear hand and movefrom
                    HandScript.Instance.DropMoveable();
                    InventoryScript.Instance.MoveFromSlot = null;
                }
            }
            // L-CTRL + LMB
            else if (Input.GetKey(KeyCode.LeftControl)) {
                HandScript.Instance.TakeMoveable(ThisBag);
            } else if (bag != null)
            {
                bag.ThisBagScript.OpenClose();
            }
        }
    }

    public void RemoveBag() {
        InventoryScript.Instance.RemoveBag(ThisBag);
        ThisBag.ThisBagButton = null;
        foreach (Item item in ThisBag.ThisBagScript.GetItems()) {
            InventoryScript.Instance.AddItem(item);
        }
        ThisBag = null;
    }
}
