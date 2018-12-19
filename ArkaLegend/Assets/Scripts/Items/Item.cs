using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public enum Quality { Common, Uncommon, Rare, Epic, Legend}

public abstract class Item : ScriptableObject, IMoveable, IDescribable {
    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private int stackSize;

    [SerializeField]
    private string title;

    [SerializeField]
    private Quality quality;

    private SlotScript thisSlot;

    public Sprite Icon
    {
        get
        {
            return icon;
        }
    }


    public int StackSize
    {
        get
        {
            return stackSize;
        }
    }

    public SlotScript ThisSlot
    {
        get
        {
            return thisSlot;
        }

        set
        {
            thisSlot = value;
        }
    }

    public Quality ThisQuality
    {
        get
        {
            return quality;
        }
    }

    public string Title
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

    public virtual string GetDescription()
    {
        string color = QualityColor.Quality2colorMap[quality];

        string afterFormat = string.Format("<size={0}><color={1}>{2}</color></size>", UIManager.TooltipTitleSize, color, Title);
        return afterFormat;

        //return title;
    }

    public void Remove() {
        if (thisSlot != null) {
            thisSlot.RemoveItem(this);
        }
    }
}
