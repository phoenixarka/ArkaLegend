using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GearButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField]
    private ArmorType armorType;

    [SerializeField]
    private Image icon;

    private Armor equippedArmor;

    [SerializeField]
    private GearSocket gearSocket;

    public void Start()
    {
        icon.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            IMoveable currentMoveable = HandScript.Instance.ThisMoveable;
            if (currentMoveable is Armor && (currentMoveable as Armor).ThisArmorType == armorType)
            {
                EquipArmor(currentMoveable as Armor);
                HandScript.Instance.DropMoveable();
                UIManager.Instance.RefreshTooltip(currentMoveable as Armor);
            }
            // left click on gearbutton without item in hand
            else if (currentMoveable == null && equippedArmor != null)
            {
                HandScript.Instance.TakeMoveable(equippedArmor);
                CharacterPanel.Instance.SelectedBtn = this;
                icon.color = Color.grey;
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right) {
            InventoryScript.Instance.AddItem(equippedArmor);
            DequipArmor();
        }
    }

    public void EquipArmor(Armor armor) {
        // remove armor from inventory
        armor.Remove();
        // swap armor with equipeed armor
        if (equippedArmor != null) {
            if (equippedArmor != armor) {
                armor.ThisSlot.AddItem(equippedArmor);
            }
            UIManager.Instance.RefreshTooltip(equippedArmor);
        } else {
            UIManager.Instance.HideTooltip();
        }

        icon.enabled = true;
        icon.sprite = armor.Icon;
        icon.color = Color.white;
        equippedArmor = armor;

        // avoid to hold item in hand and right click armor
        if (HandScript.Instance.ThisMoveable == armor as IMoveable) {
            HandScript.Instance.DropMoveable();
        }

        // setup the animation for gear
        if (gearSocket != null && equippedArmor.ThisAnimationClips != null) {
            gearSocket.Equip(equippedArmor.ThisAnimationClips);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (equippedArmor != null) {
            UIManager.Instance.ShowTooltip(transform.position, equippedArmor);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideTooltip();
    }

    public void DequipArmor() {
        icon.color = Color.white;
        icon.enabled = false;

        if (gearSocket != null && equippedArmor.ThisAnimationClips != null)
        {
            gearSocket.Dequip();
        }

        equippedArmor = null;
    }
}
