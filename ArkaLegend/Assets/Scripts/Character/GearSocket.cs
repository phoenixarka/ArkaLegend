using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearSocket : MonoBehaviour {
    public Animator ThisAnimator { get; set; }

    protected SpriteRenderer spriteRenderer;

    private Animator parentAnimator;

    private AnimatorOverrideController animatorOverrideController;

    private void Awake()
    {
        // socket layer sprite and animator
        spriteRenderer = GetComponent<SpriteRenderer>();
        ThisAnimator = GetComponent<Animator>();

        //character animator
        parentAnimator = GetComponentInParent<Animator>();

        // overrideController overrides character animator
        animatorOverrideController = new AnimatorOverrideController(ThisAnimator.runtimeAnimatorController);

        // set current runtime animator to overrided animator;
        ThisAnimator.runtimeAnimatorController = animatorOverrideController;

        // hide the layer at begining
        Color c = spriteRenderer.color;
        c.a = 0;
        spriteRenderer.color = c;
    }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void SetDirection(float x, float y) {
        ThisAnimator.SetFloat("x", x);
        ThisAnimator.SetFloat("y", y);
    }

    public void ActivateLayer(string layerName)
    {
        // disable all layer
        for (int i = 0; i < ThisAnimator.layerCount; i++)
        {
            ThisAnimator.SetLayerWeight(i, 0);
        }

        // enable layer
        ThisAnimator.SetLayerWeight(ThisAnimator.GetLayerIndex(layerName), 1);

    }

    public void Equip(AnimationClip[] animationClips) {
        if (animationClips == null || animationClips.Length < 1) {
            return;
        }

        int i = 0*8;
        if (animationClips.Length >= i + 7)
        {
            animatorOverrideController["char_atk1_d"] = animationClips[i + 0];
            animatorOverrideController["char_atk1_dl"] = animationClips[i + 1];
            animatorOverrideController["char_atk1_dr"] = animationClips[i + 2];
            animatorOverrideController["char_atk1_l"] = animationClips[i + 3];
            animatorOverrideController["char_atk1_r"] = animationClips[i + 4];
            animatorOverrideController["char_atk1_u"] = animationClips[i + 5];
            animatorOverrideController["char_atk1_ul"] = animationClips[i + 6];
            animatorOverrideController["char_atk1_ur"] = animationClips[i + 7];
        }


        i = 1 * 8;
        if (animationClips.Length >= i + 7)
        {
            animatorOverrideController["char_idle_d"] = animationClips[i + 0];
            animatorOverrideController["char_idle_dl"] = animationClips[i + 1];
            animatorOverrideController["char_idle_dr"] = animationClips[i + 2];
            animatorOverrideController["char_idle_l"] = animationClips[i + 3];
            animatorOverrideController["char_idle_r"] = animationClips[i + 4];
            animatorOverrideController["char_idle_u"] = animationClips[i + 5];
            animatorOverrideController["char_idle_ul"] = animationClips[i + 6];
            animatorOverrideController["char_idle_ur"] = animationClips[i + 7];
        }

        i = 2 * 8;
        if (animationClips.Length >= i + 7)
        {
            animatorOverrideController["char_walk_d"] = animationClips[i + 0];
            animatorOverrideController["char_walk_dl"] = animationClips[i + 1];
            animatorOverrideController["char_walk_dr"] = animationClips[i + 2];
            animatorOverrideController["char_walk_l"] = animationClips[i + 3];
            animatorOverrideController["char_walk_r"] = animationClips[i + 4];
            animatorOverrideController["char_walk_u"] = animationClips[i + 5];
            animatorOverrideController["char_walk_ul"] = animationClips[i + 6];
            animatorOverrideController["char_walk_ur"] = animationClips[i + 7];
        }

        Color c = spriteRenderer.color;
        c.a = 255;
        spriteRenderer.color = c;
    }

    public void Dequip() {
        animatorOverrideController["char_idle_d"] = null;
        animatorOverrideController["char_idle_dl"] = null;
        animatorOverrideController["char_idle_dr"] = null;
        animatorOverrideController["char_idle_l"] = null;
        animatorOverrideController["char_idle_r"] = null;
        animatorOverrideController["char_idle_u"] = null;
        animatorOverrideController["char_idle_ul"] = null;
        animatorOverrideController["char_idle_ur"] = null;

        animatorOverrideController["char_walk_d"] = null;
        animatorOverrideController["char_walk_dl"] = null;
        animatorOverrideController["char_walk_dr"] = null;
        animatorOverrideController["char_walk_l"] = null;
        animatorOverrideController["char_walk_r"] = null;
        animatorOverrideController["char_walk_u"] = null;
        animatorOverrideController["char_walk_ul"] = null;
        animatorOverrideController["char_walk_ur"] = null;
        Color c = spriteRenderer.color;
        c.a = 0;
        spriteRenderer.color = c;
    }
}
