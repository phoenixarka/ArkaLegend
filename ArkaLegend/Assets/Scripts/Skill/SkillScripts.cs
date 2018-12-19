using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillScripts : MonoBehaviour
{

    private Rigidbody2D rigidbody2d;

    [SerializeField]
    private float speed;

    private float dmg;

    public Transform Target
    {
        get; private set;
    }

    private Transform source;

    private ParticleSystem particle;

    // Use this for initialization
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        particle = GetComponentInChildren<ParticleSystem>();
    }

    public void Initialized(Transform target, float dmg, Transform source)
    {
        this.source = source;
        this.Target = target;
        this.dmg = dmg;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (Target != null) {
            Vector2 direction = Target.position - transform.position;
            rigidbody2d.velocity = direction.normalized * speed;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hitbox" && collision.transform == Target) {
            Character c = collision.GetComponentInParent<Character>();
            speed = 0;
            c.TakeDamage(dmg, source);
            //Debug.Log("collideEnemy");
            rigidbody2d.velocity = Vector2.zero;

            PlayParticle();

            Target = null;

            if (particle.isStopped) {
                Destroy(gameObject);
            }
        }
    }

    private void PlayParticle() {
        GetComponent<SpriteRenderer>().enabled = false;
        particle.Play();
    }

}
