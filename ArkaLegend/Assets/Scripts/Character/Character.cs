using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public abstract class Character : MonoBehaviour {

    [SerializeField]
    protected Transform hitBox;

    [SerializeField]
    protected Stats hp;

    public Stats Hp
    {
        get { return hp; }
    }

    [SerializeField]
    private float initHp;

    [SerializeField]
    protected float speed;
    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
        }
    }

    private Rigidbody2D charRigidbody2d;

    protected Coroutine atkRoutine;
    protected Vector2 direction;
    public Vector2 Direction
    {
        get
        {
            return direction;
        }

        set
        {
            direction = value;
        }
    }
    public Transform Target { get; set; }
    public bool IsAtk { get; set; }
    public Animator ThisAnimator { get; set; }
    public bool IsAlive {
        get
        {
            return hp.CurrVal > 0;
        }
    }

    // Use this for initialization
    protected virtual void Start () {
        hp.Initialize(initHp, initHp);

        ThisAnimator = GetComponent<Animator>();
        charRigidbody2d = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	protected virtual void Update () {
        HandleLayers();
    }

    public void FixedUpdate()
    {
        Move();
    }

    public bool isMove
    {
        get
        {
            if (direction.x != 0 || direction.y != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public void Move()
    {
        if (IsAlive) {
            charRigidbody2d.velocity = direction.normalized * speed;
        }
        
    }

    public virtual void ActivateLayer(string layerName)
    {
        // disable all layer
        for (int i = 0; i < ThisAnimator.layerCount; i++)
        {
            ThisAnimator.SetLayerWeight(i, 0);
        }

        // enable layer
        ThisAnimator.SetLayerWeight(ThisAnimator.GetLayerIndex(layerName), 1);
    }

    public virtual void HandleLayers()
    {
        if (IsAlive)
        {
            if (isMove)
            {
                ActivateLayer("walk");
                ThisAnimator.SetFloat("x", direction.x);
                ThisAnimator.SetFloat("y", direction.y);
            }
            else if (IsAtk)
            {
                ActivateLayer("atk");
            }
            else
            {
                ActivateLayer("idle");
            }
        }
        else {
            ActivateLayer("death");
        }
    }

    public virtual void TakeDamage(float dmg, Transform source)
    {
        // reduce health
        hp.CurrVal -= dmg;

        if (hp.CurrVal <= 0) {

            direction = Vector2.zero;
            charRigidbody2d.velocity = direction;
            // die
            ThisAnimator.SetTrigger("die");
        }
    }
}
