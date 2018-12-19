using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandScript : MonoBehaviour {
    private static HandScript instance;

    public static HandScript Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<HandScript>();
            }

            return instance;
        }
    }
    public IMoveable ThisMoveable { get; set; }

    private Image icon;

    [SerializeField]
    private Vector3 offset;

	// Use this for initialization
	void Start () {
        icon = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        icon.transform.position = Input.mousePosition;

        if (Input.GetMouseButton(0) &&
            !EventSystem.current.IsPointerOverGameObject() &&
            Instance.ThisMoveable != null)
        {

            DeleteItem();
        }
        
    }

    public void TakeMoveable(IMoveable moveable) {
        this.ThisMoveable = moveable;
        icon.sprite = moveable.Icon;
        icon.color = Color.white;
    }

    public IMoveable PutMoveable() {
        // get current moveable
        IMoveable currMoveable = ThisMoveable;
        // reset movingItem
        ThisMoveable = null;
        icon.color = new Color(255,255,255,0);
        // return moveable
        return currMoveable;
    }

    public void DropMoveable() {
        ThisMoveable = null;
        InventoryScript.Instance.MoveFromSlot = null;
        icon.color = new Color(0, 0, 0, 0);
    }

    public void DeleteItem() {
        if (ThisMoveable is Item && InventoryScript.Instance.MoveFromSlot != null)
        {
            (ThisMoveable as Item).ThisSlot.Clear();
        }

        DropMoveable();

        InventoryScript.Instance.MoveFromSlot = null;
        Debug.Log("Warning Text:Item in hand has been deleted");
    }
}
