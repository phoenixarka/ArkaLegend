using System;
using UnityEngine;

[Serializable]
public class Skill : IUseable,IMoveable,IDescribable
{
    [SerializeField]
    private string name;

    [SerializeField]
    private float dmg;

    [SerializeField]
    private float mana;

    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float castTime;

    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private string description;

    [SerializeField]
    private Color castbarColor;

    private string skillDescColor;

    public string Name
    {
        get
        {
            return name;
        }
    }

    public float Dmg
    {
        get
        {
            return dmg;
        }
    }

    public Sprite Icon
    {
        get
        {
            return icon;
        }
    }

    public float Speed
    {
        get
        {
            return speed;
        }
    }

    public float CastTime
    {
        get
        {
            return castTime;
        }
    }

    public GameObject Prefab
    {
        get
        {
            return prefab;
        }
    }

    public Color CastbarColor
    {
        get
        {
            return castbarColor;
        }
    }

    public string GetDescription()
    {
        string titleDesc = string.Format("<size={0}>{1}</size>\n",UIManager.TooltipTitleSize, name);
        string useageDesc = string.Format("<color={0}>Mana cost: {1}\nCast time: {2} seconds</color>\n",
                                          UIManager.FunctionalDescColor, mana, castTime);
        string damageDesc = string.Format("{1} \n<color={0}>Causes {2} damage</color>\n", UIManager.FunctionalDescColor,description, dmg);

        return titleDesc + useageDesc + damageDesc;
    }

    public void Use()
    {
        Player.Instance.CastSkillAtk(Name);
    }
}