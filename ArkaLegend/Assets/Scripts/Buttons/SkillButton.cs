using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private string skillName;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) {
            // pick up skill
            HandScript.Instance.TakeMoveable(SkillBook.Instance.GetSkill(skillName));
        }


    }
}
