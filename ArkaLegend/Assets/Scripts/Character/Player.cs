using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {
    private static Player instance;

    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }

            return instance;
        }
    }

    [SerializeField]
    private Stats mp;

    //initial val
    private float initMp = 100;

    [SerializeField]
    private Transform[] exitPoints;
    private int exitPointIdx;

    [SerializeField]
    private Block sightBlock;

    private Vector3 min, max;

    [SerializeField]
    private GearSocket[] gearSockets;

    protected override void Start() {
        base.Start();
        mp.Initialize(initMp, initMp);
    }

    // Update is called once per frame
    protected override void Update()
    {
        GetInput();
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x), 
                                         Mathf.Clamp(transform.position.y, min.y, max.y),
                                         transform.position.z);

        base.Update();
    }

    //
    private void GetInput()
    {
        
        // Debug Hp/Mp
        if (Input.GetKeyDown(KeyCode.I))
        {
            hp.CurrVal -= 10;
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            hp.CurrVal += 10;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            mp.CurrVal -= 10;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            mp.CurrVal += 10;
        }
        

        direction.x = 0;
        direction.y = 0;

        // Movement        
        if (Input.GetKey(KeyBindManager.Instance.KeyBinds["UP"]))
        {
            direction.y = 1;
            exitPointIdx = calculateExitIdx();
        }

        if (Input.GetKey(KeyBindManager.Instance.KeyBinds["DOWN"]))
        {
            direction.y = -1;
            exitPointIdx = calculateExitIdx();
        }

        if (Input.GetKey(KeyBindManager.Instance.KeyBinds["LEFT"]))
        {
            direction.x = -1;
            exitPointIdx = calculateExitIdx();
        }

        if (Input.GetKey(KeyBindManager.Instance.KeyBinds["RIGHT"]))
        {
            direction.x = 1;
            exitPointIdx = calculateExitIdx();
        }

        if (isMove) {
            StopAtk();
        }

        foreach (string action in KeyBindManager.Instance.ActionBinds.Keys)
        {
            if (Input.GetKeyDown(KeyBindManager.Instance.ActionBinds[action]))
            {
                UIManager.Instance.ClickActionBtn(action);
            }
        }
    }

    //
    private IEnumerator SkillAtk(string skillName)
    {
        Transform currTarget = Target;

        Skill skill = SkillBook.Instance.CastSkill(skillName);

        IsAtk = true;

        ThisAnimator.SetBool("skillAtk", IsAtk);

        // enable attack for each equipment socket layer
        foreach (GearSocket gearSocket in gearSockets) {
            gearSocket.ThisAnimator.SetBool("skillAtk", IsAtk);
        }

        yield return new WaitForSeconds(skill.CastTime);

        if (currTarget != null && InSight()) {
            SkillScripts skillManager = Instantiate(skill.Prefab, exitPoints[exitPointIdx].position, Quaternion.identity).GetComponent<SkillScripts>();
            skillManager.Initialized(currTarget, skill.Dmg,transform);
        }

        StopAtk();
    }

    public void StopAtk()
    {
        SkillBook.Instance.StopCasting();

        IsAtk = false;

        if (atkRoutine != null)
        {
            StopCoroutine(atkRoutine);
            IsAtk = false;
            ThisAnimator.SetBool("skillAtk", IsAtk);

            // disable attack for each equipment socket layer
            foreach (GearSocket gearSocket in gearSockets)
            {
                gearSocket.ThisAnimator.SetBool("skillAtk", IsAtk);
            }
        }
    }

    //
    public void CastSkillAtk(string skillName) {
        //Debug.Log(exitPointIdx);
        Block();

        if (Target != null && !IsAtk && !isMove && InSight() && Target.GetComponentInParent<Character>().IsAlive)
        {
            //Debug.Log("atk_routined");
            atkRoutine = StartCoroutine(SkillAtk(skillName));
        }
    }

    //
    private bool InSight() {

        if (Target != null) {
            Vector2 targetDirection = (Target.position - transform.position).normalized;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, Target.transform.position), LayerMask.GetMask("Block"));
            if (hit.collider == null)
            {
                return true;
            }
        }

        return false;
    }

    //
    private void Block() {
        sightBlock.Deactive();
        sightBlock.Angle = exitPointIdx * -45;
        sightBlock.Active();
    }

    private int calculateExitIdx() {
        /*
           1 2 3
           0   4
           7 6 5
         */
        int idx = 0;
        if (direction.y == 0)
        {
            if (direction.x < 0)
            {
                idx = 0;
            }
            else
            {
                idx = 4;
            }
        }
        else if (direction.x == 0)
        {
            if (direction.y > 0)
            {
                idx = 2;
            }
            else
            {
                idx = 6;
            }
        }
        else
        {
            if (direction.x < 0)
            {
                if (direction.y > 0)
                {
                    idx = 1;
                }
                else
                {
                    idx = 7;
                }
            }
            else {
                if (direction.y > 0)
                {
                    idx = 3;
                }
                else
                {
                    idx = 5;
                }
            }
        }
        return idx;
    }

    public void SetLimits(Vector3 min, Vector3 max) {
        this.min = min;
        this.max = max;
    }

    public override void HandleLayers()
    {
        base.HandleLayers();
        if (isMove) {
            foreach (GearSocket gearSocket in gearSockets) {
                gearSocket.SetDirection(direction.x,direction.y);
            }
        }
    }

    public override void ActivateLayer(string layerName)
    {
        base.ActivateLayer(layerName);

        foreach (GearSocket gearSocket in gearSockets)
        {
            gearSocket.ActivateLayer(layerName);
        }
    }
}
