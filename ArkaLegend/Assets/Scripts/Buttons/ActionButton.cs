using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerClickHandler,IClickable,IPointerEnterHandler, IPointerExitHandler
{
    public IUseable ThisUseable { get; set; }

    public Button Btn { get; private set; }

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

    public int ThisCount
    {
        get
        {
            return count;
        }
    }

    public Text StackText {
        get {
            return stkSizeText;
        }
    }

    [SerializeField]
    private Image icon;

    private Stack<IUseable> useableStk = new Stack<IUseable>();

    private int count;

    [SerializeField]
    private Text stkSizeText;


    // Use this for initialization
    void Start () {
        icon.color = new Color(0, 0, 0, 0);
        Btn = GetComponent<Button>();
        Btn.onClick.AddListener(OnClick);
        InventoryScript.Instance.itemCountChangedEvent += new ItemCountChanged(UpdateItemCount);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick() {
        if (HandScript.Instance.ThisMoveable == null) {
            
            // try to use useable(skill, etc)
            if (ThisUseable != null)
            {
                ThisUseable.Use();
            }

            // try to use itemStk(items,consumeables)
            if (useableStk != null && useableStk.Count > 0)
            {
                useableStk.Peek().Use();
            }

        }
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) {
            IMoveable currMoveable = HandScript.Instance.ThisMoveable;
            if (currMoveable != null && currMoveable is IUseable) {
                SetUsable(currMoveable as IUseable);
            }
        }
    }

    public void SetUsable(IUseable useable)
    {
        if (useable is Item)
        {
            this.ThisUseable = null;
            useableStk = InventoryScript.Instance.GetUseables(useable);
            InventoryScript.Instance.MoveFromSlot.Icon.color = Color.white;
            InventoryScript.Instance.MoveFromSlot = null;
        }
        else {
            useableStk.Clear();
            this.ThisUseable = useable;
        }

        count = useableStk.Count;
        UpdateVisual();
    }
    public void UpdateVisual() {
        icon.sprite = HandScript.Instance.PutMoveable().Icon;
        icon.color = Color.white;
        if (count > 1)
        {
            UIManager.Instance.UpdateStackSize(this);
        }
        else if(ThisUseable is Skill){
            UIManager.Instance.ClearStkCount(this);
        }
    }

    public void UpdateItemCount(Item item) {
        if (item is IUseable && useableStk.Count > 0) {
            if (useableStk.Peek().GetType() == item.GetType()) {
                useableStk = InventoryScript.Instance.GetUseables(item as IUseable);
                count = useableStk.Count;
                UIManager.Instance.UpdateStackSize(this);
            }
        }
    }

    // Mouse Hover Enter 
    public void OnPointerEnter(PointerEventData eventData)
    {
        IDescribable desc = null;
        if (ThisUseable != null && ThisUseable is IDescribable)
        {
            desc = ThisUseable as IDescribable;
        }
        else if (useableStk != null && useableStk.Count > 0) {

            desc = useableStk.Peek() as IDescribable;
        }

        if (desc != null) {
            UIManager.Instance.ShowTooltip(transform.position, desc);
        }

        
    }

    // Mouse Hover Exit
    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideTooltip();
    }
}
