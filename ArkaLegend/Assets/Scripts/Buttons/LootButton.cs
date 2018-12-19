using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LootButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler{

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text title;

    private LootWindow lootWindow;


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

    public Text Title
    {
        get
        {
            return title;
        }

        set
        {
            title = value;
        }
    }

    public Item ThisLoot { get; set; }

    private void Awake()
    {
        lootWindow = GetComponentInParent<LootWindow>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (InventoryScript.Instance.AddItem(ThisLoot))
        {
            gameObject.SetActive(false);
            int btnIdx = Int32.Parse(name.Replace("item", ""));
            lootWindow.TakeLoot(btnIdx-1);
            UIManager.Instance.HideTooltip();
        }
        else {
            Debug.Log("Warning Text: Insufficient inventory space");
        }
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.ShowTooltip(transform.position, ThisLoot);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideTooltip();
    }
}
