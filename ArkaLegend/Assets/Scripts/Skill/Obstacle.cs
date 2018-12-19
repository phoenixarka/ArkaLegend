using System;
using UnityEngine;

public class Obstacle : MonoBehaviour,IComparable<Obstacle> {

    public SpriteRenderer spriteRenderer { get; set; }

    private Color defaultColor;

    private Color fadedColor;

    public int CompareTo(Obstacle other)
    {
        return spriteRenderer.sortingOrder - other.spriteRenderer.sortingOrder;
    }

    // Use this for initialization
    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();

        defaultColor = spriteRenderer.color;
        fadedColor = defaultColor;
        fadedColor.a = 0.5f;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void FadeOut() {
        spriteRenderer.color = fadedColor;
    }

    public void FadeIn()
    {
        spriteRenderer.color = defaultColor;
    }
}
