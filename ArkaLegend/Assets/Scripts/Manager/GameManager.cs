using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {
    [SerializeField]
    private Player player;

    private NPC currTarget;

    // Update is called once per frame
    void Update() {
        clickTarget();
    }

    private void clickTarget() {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), 
                Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Clickable"));
            //Debug.Log("hit=" + hit.collider);

            if (hit.collider != null)
            {
                currTarget = hit.collider.GetComponent<NPC>();
                player.Target = currTarget.Select();
                UIManager.Instance.ShowTargetFrame(currTarget);
            }
            else
            {
                if (currTarget != null)
                {
                    currTarget.DeSelect();
                }
                currTarget = null;
                player.Target = null;
                UIManager.Instance.HideTargetFrame();
            }
        }
        else if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject()) {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), 
                Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Clickable"));
            if (hit.collider != null && hit.collider.tag == "Enemy") {
                hit.collider.GetComponent<NPC>().Interact();
            }
        }

    }
}
