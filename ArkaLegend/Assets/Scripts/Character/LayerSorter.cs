using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSorter : MonoBehaviour {

    private SpriteRenderer currentSpriteRenderer;

    private int initSoringOrder = 0;

    private List<Obstacle> obstacles;

	// Use this for initialization
	void Start () {
        obstacles = new List<Obstacle>();
        currentSpriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
        initSoringOrder = currentSpriteRenderer.sortingOrder;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle") {
            Obstacle o = collision.GetComponent<Obstacle>();
            o.FadeOut();

            if (obstacles.Count == 0 || o.spriteRenderer.sortingOrder - 1 < currentSpriteRenderer.sortingOrder) {
                currentSpriteRenderer.sortingOrder = o.spriteRenderer.sortingOrder - 1;
            }

            obstacles.Add(o);
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle")
        {
            Obstacle o = collision.GetComponent<Obstacle>();
            o.FadeIn();
            obstacles.Remove(o);

            if (obstacles.Count == 0)
            {
                currentSpriteRenderer.sortingOrder = initSoringOrder;
            }
            else {
                obstacles.Sort();
                currentSpriteRenderer.sortingOrder = obstacles[0].spriteRenderer.sortingOrder - 1;
            }
        }
    }
}