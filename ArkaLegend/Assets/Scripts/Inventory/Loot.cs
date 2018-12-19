using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Loot{
    [SerializeField]
    private Item item;

    [SerializeField]
    // min val = 0.0001
    private float dropChange;

    public Item ThisItem
    {
        get
        {
            return item;
        }
    }

    public float DropChange
    {
        get
        {
            return dropChange;
        }
    }
}
