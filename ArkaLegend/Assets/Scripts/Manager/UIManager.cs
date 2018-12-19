using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    private static UIManager instance;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }

            return instance;
        }
    }

    [SerializeField]
    private ActionButton[] actionBtns;

    [SerializeField]
    private GameObject targetFrame;

    private Stats hp;

    [SerializeField]
    private Image protraitFrame;

    [SerializeField]
    private CanvasGroup keybindMenu;

    [SerializeField]
    private CanvasGroup skillBook;

    [SerializeField]
    private GameObject tooltip;

    [SerializeField]
    private CharacterPanel characterPanel;

    private Text tooltipText;

    private GameObject[] keyBindBtns;

    public static string FunctionalDescColor{
        get {
            return "#66CCFF";
        }
    }

    public static int TooltipTitleSize {
        get {
            return 16;
        }
    }

    private void Awake()
    {
        keyBindBtns = GameObject.FindGameObjectsWithTag("KeyBinds");

        tooltipText = tooltip.GetComponentInChildren<Text>();

        //initial menus
        keybindMenu.alpha = 0;
        keybindMenu.blocksRaycasts = false;

        skillBook.alpha = 0;
        skillBook.blocksRaycasts = false;
    }

    // Use this for initialization
    void Start () {
        hp = targetFrame.GetComponentsInChildren<Stats>()[0];
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F10)) {
            OpenClose(keybindMenu);
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            OpenClose(skillBook);
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.B)) {    
            InventoryScript.Instance.OpenClose();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            characterPanel.OpenClose();
        }

    }

    public void ShowTargetFrame(NPC target) {
        hp.Initialize(target.Hp.CurrVal, target.Hp.MaxVal);
        targetFrame.GetComponentsInChildren<Image>()[1].sprite = target.Portrait;
        targetFrame.GetComponentsInChildren<Image>()[2].sprite = target.Job;


        targetFrame.SetActive(true);
        target.hpChanged += new HPChange(UpdateTargetFrame);
        target.characterRemoved += new CharacterRemoved(HideTargetFrame);
    }

    public void HideTargetFrame()
    {
        targetFrame.SetActive(false);
    }

    public void UpdateTargetFrame(float hpVal) {
        hp.CurrVal = hpVal;
    }

    public void UpdateKey(string key, KeyCode keyBind) {
        Text btn_text = Array.Find(keyBindBtns, x => x.name == key).GetComponentInChildren<Text>();
        btn_text.text = keyBind.ToString().Replace("Alpha","").Replace("Keypad","");
    }

    public void ClickActionBtn(string btnName) {
        ActionButton currActBtn = Array.Find(actionBtns, x => x.gameObject.name == btnName);
        if (currActBtn != null) {
            currActBtn.Btn.onClick.Invoke();
        }
    }



    public void OpenClose(CanvasGroup canvasGroup) {
        // show/hide
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        // block/non-block
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;

        // pause/un-pause
        //Time.timeScale = Time.timeScale > 0 ? 0 : 1;
    }

    public void UpdateStackSize(IClickable clickable) {
        if (clickable.ThisCount > 1)
        {
            clickable.StackText.text = clickable.ThisCount.ToString();
            clickable.StackText.color = Color.white;
            clickable.Icon.color = Color.white;
        }
        else {
            ClearStkCount(clickable);
        }

        if (clickable.ThisCount == 0) {
            clickable.StackText.color = new Color(0, 0, 0, 0);
            clickable.Icon.color = new Color(0, 0, 0, 0);
        }
    }

    public void ClearStkCount(IClickable clickable) {
        clickable.StackText.color = new Color(0, 0, 0, 0);
        clickable.Icon.color = Color.white;
    }

    private Vector3 CalculateOffset(Vector3 currPosition, Vector3 baseOffset) {
        // set default offset
        Vector3 currOffset = baseOffset;

        // get canvas size and tooltip size
        RectTransform mainCanvasRect = GameObject.FindGameObjectWithTag("MainCanvas").transform as RectTransform;
        RectTransform tooltipRect = tooltip.transform as RectTransform;

        // calculate tooltip position
        Vector3 currTooltipPos = currOffset + currPosition + new Vector3(tooltipRect.rect.width, tooltipRect.rect.height, 0f);

        // adjust startpoint
        if (currTooltipPos.x > mainCanvasRect.rect.width - baseOffset.x)
        {
            currOffset = new Vector3(-currOffset.x - tooltipRect.rect.width, currOffset.y, 0f);
        }

        if (currTooltipPos.y > mainCanvasRect.rect.height - -baseOffset.y)
        {
            currOffset = new Vector3(currOffset.x, -currOffset.y - tooltipRect.rect.height, 0f);
        }

        return currOffset;
    }

    public void ShowTooltip(Vector3 position, IDescribable description) {
        tooltip.transform.position = position + CalculateOffset(position, new Vector3(10f, 10f, 0f));
        tooltipText.text = description.GetDescription();
        tooltip.SetActive(true);
    }

    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }

    public void RefreshTooltip(IDescribable description) {
        tooltipText.text = description.GetDescription();
    }
}
