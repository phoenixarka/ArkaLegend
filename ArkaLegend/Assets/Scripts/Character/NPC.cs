using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void HPChange(float hp);

public delegate void CharacterRemoved(); 

public class NPC : Character {

    public event HPChange hpChanged;

    public event CharacterRemoved characterRemoved;

    [SerializeField]
    private Sprite portrait;

    [SerializeField]
    private Sprite job;

    public Sprite Portrait
    {
        get
        {
            return portrait;
        }
    }

    public Sprite Job
    {
        get
        {
            return job;
        }
    }

    public virtual void DeSelect() {
        hpChanged -= new HPChange(UIManager.Instance.UpdateTargetFrame);
        characterRemoved -= new CharacterRemoved(UIManager.Instance.HideTargetFrame);
    }

    public virtual Transform Select() {
        return hitBox;
    }

    public void OnHPChanged(float hp) {
        if (hpChanged != null) {
            hpChanged(hp);
        }
    }

    public void OnCharacterRemoved()
    {
        if (characterRemoved != null) {
            characterRemoved();
        }

        Destroy(gameObject);
    }

    public virtual void Interact() {
        // TO-DO
        Debug.Log("Open dialog with NPC");
    }
	
}
