using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBehavior : StateMachineBehaviour {

    private float timePassed;
    private float fadeoutDelay = 10;
    private float fadeoutTime = 3;
    

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        Destroy(animator.transform.GetChild(0).gameObject);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponentInChildren<CanvasGroup>().alpha = 0;
        timePassed += Time.deltaTime;
        if (timePassed >= fadeoutDelay) {
            Color objColor = animator.gameObject.GetComponent<SpriteRenderer>().material.color;
            if (objColor.a >= 0 && (timePassed - fadeoutDelay) >= 0)
            {
                objColor.a = 1.0f - ((timePassed - fadeoutDelay) / fadeoutTime);
                animator.gameObject.GetComponent<SpriteRenderer>().material.color = objColor;
            }
            else
            {
                animator.GetComponent<NPC>().OnCharacterRemoved();
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
