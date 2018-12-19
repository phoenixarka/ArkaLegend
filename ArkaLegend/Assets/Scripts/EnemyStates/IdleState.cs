using UnityEngine;
using UnityEditor;

class IdleState : IState
{
    private Enemy parent;

    public void Enter(Enemy parent)
    {
        this.parent = parent;
        this.parent.Reset();
    }

    public void Exit()
    {
       
    }

    public void Update()
    {
        //Debug.Log("Idle");
        if (parent.Target != null)
        {
            parent.ChangeState(new FollowState());
        }
    }
}