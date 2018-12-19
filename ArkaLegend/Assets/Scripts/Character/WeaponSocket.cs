using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSocket :GearSocket {
    private float currY;

    [SerializeField]
    private SpriteRenderer parentRenderer;

    public override void SetDirection(float x, float y) {
        base.SetDirection(x,y);

        // used for whole weapon set(hide and see)
        /*
        if (currY != y)
        {
            spriteRenderer.sortingOrder = parentRenderer.sortingOrder - 1;
        }
        else {
            spriteRenderer.sortingOrder = parentRenderer.sortingOrder + 1;
        }
        */
    }

}
