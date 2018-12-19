﻿using UnityEngine;
using UnityEditor;

public class FollowState : IState
{
    private Enemy parent;

    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {
        parent.Direction = Vector2.zero;
    }

    public void Update()
    {
        //Debug.Log("Follow");
        if (parent.Target != null)
        {
            parent.Direction = (parent.Target.transform.position - parent.transform.position).normalized;
            parent.transform.position = Vector2.MoveTowards(parent.transform.position, parent.Target.position, parent.Speed * Time.deltaTime);

            float distance = Vector2.Distance(parent.Target.position, parent.transform.position);

            if (distance <= parent.AttackRange) {
                parent.ChangeState(new AttackState());
            }
        }

        if (!parent.InRange) {
            parent.ChangeState(new EvadeState());
        }

        
    }
}