using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ArmorType {Head, Shoulders, Chest, Back, Hands, Feet, MainHand, OffHand, TwoHands}

[CreateAssetMenu(fileName = "Armor", menuName = "Items/Armor", order = 2)]
public class Armor : Item{
    [SerializeField]
    private ArmorType armorType;

    [SerializeField]
    private int strength;

    [SerializeField]
    private int intellect;

    [SerializeField]
    private int vitality;

    [SerializeField]
    private AnimationClip[] animationClips;

    internal ArmorType ThisArmorType
    {
        get
        {
            return armorType;
        }
    }

    public AnimationClip[] ThisAnimationClips
    {
        get
        {
            return animationClips;
        }
    }

    public override string GetDescription()
    {
        string stats = string.Empty;
        if (strength > 0)
        {
            stats += string.Format("\n +{0} STR", strength);
        }

        if (intellect > 0)
        {
            stats += string.Format("\n +{0} INT", intellect);
        }

        if (vitality > 0)
        {
            stats += string.Format("\n +{0} VIT", vitality);
        }


        return base.GetDescription() + string.Format("<color={0}>{1}</color>",UIManager.FunctionalDescColor,stats);
    }

    public void Equip()
    {
        CharacterPanel.Instance.EquipArmor(this);
    }
}
