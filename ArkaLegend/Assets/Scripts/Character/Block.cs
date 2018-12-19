using System;
using UnityEngine;

[Serializable]
public class Block {

    [SerializeField]
    private GameObject block;

    private float angle;

    public float Angle
    {
        get
        {
            return angle;
        }

        set
        {
            angle = value;
        }
    }

    public void Deactive() {
        block.SetActive(false);
    }

    public void Active() {
        block.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        block.SetActive(true);
    }
}
