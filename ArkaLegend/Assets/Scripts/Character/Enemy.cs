using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC {

    [SerializeField]
    private CanvasGroup healthGroup;

    [SerializeField]
    private float initAggroRange;

    [SerializeField]
    private LootTable lootTable;

    private IState currState;

    public float AggroRange { get; set; }

    public float AttackRange { get; set; }

    public float AttackTime { get; set; }

    public Vector3 StartPos { get; set; }

    public bool InRange {
        get {
            return Vector2.Distance(transform.position, Target.position) < AggroRange;
        }
    }

    protected void Awake()
    {
        StartPos = this.transform.position;
        AggroRange = initAggroRange;
        AttackRange = 0.7f;
        ChangeState(new IdleState());
    }

    protected override void Update()
    {
        if (IsAlive) {
            if (!IsAtk)
            {
                AttackTime += Time.deltaTime;
            }

            currState.Update();
        }
        base.Update();
    }

    public override Transform Select()
    {
        healthGroup.alpha = 1;
        return base.Select();
    }

    public override void DeSelect()
    {
        healthGroup.alpha = 0;
        base.DeSelect();
    }

    public override void TakeDamage(float dmg, Transform source) 
    {
        if (! (currState is EvadeState) ) {
            SetTarget(source);

            base.TakeDamage(dmg, source);

            OnHPChanged(hp.CurrVal);
        }
    }

    public void ChangeState(IState newState)
    {
        if (currState != null)
        {
            currState.Exit();
        }
        currState = newState;
        currState.Enter(this);
    }

    public void SetTarget(Transform target) {
        if (Target == null && !(currState is EvadeState)) {
            float distance = Vector2.Distance(transform.position, target.position);
            AggroRange = initAggroRange + distance;
            Target = target;
        }
    }

    public void Reset()
    {
        Target = null;
        this.AggroRange = initAggroRange;
        this.Hp.CurrVal = Hp.MaxVal;
        OnHPChanged(hp.CurrVal);
    }

    public override void Interact()
    {
        if (!IsAlive) {
            lootTable.ShowLoot();
        }
        
    }
}
