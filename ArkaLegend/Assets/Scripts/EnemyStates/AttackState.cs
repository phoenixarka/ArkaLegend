using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState {

    private Enemy parent;

    private float attackCooldown = 3;

    private float extraRange = 0.2f;

    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {

    }

    public void Update()
    {
        //Debug.Log("Attack");

        if (parent.AttackTime >= attackCooldown && !parent.IsAtk) {
            //reset attack time
            parent.AttackTime = 0;
            parent.StartCoroutine(Attack());
        }

        if (parent.Target != null)
        {
            float distance = Vector2.Distance(parent.Target.position, parent.transform.position);

            if (distance >= parent.AttackRange + extraRange && !parent.IsAtk) {
                parent.ChangeState(new FollowState());
            }

            // Check range

            // Attack
        }
        else {
            parent.ChangeState(new IdleState());
        }
    }

    public IEnumerator Attack() {
        parent.IsAtk = true;
        parent.ThisAnimator.SetTrigger("atk");

        yield return new WaitForSeconds(parent.ThisAnimator.GetCurrentAnimatorStateInfo(2).length);

        parent.IsAtk = false;
    }
}
