using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Vector2 velocity;
    [SerializeField] private float changeTime;
    [SerializeField] private ParticleSystem smokeEffect;

    private Rigidbody2D rigidbd2D;
    private Animator animator;
    private WaitForSeconds startTime, repeatRate;
    private Vector2 direction;
    private RubyController player;


    void Awake()
    {
        rigidbd2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        startTime = new WaitForSeconds(changeTime / 2);
        repeatRate = new WaitForSeconds(changeTime);

        direction = velocity.normalized;
        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);

        rigidbd2D.velocity = velocity;
    }

    void Start()
    {
        //InvokeRepeating("ReverseDirection", changeTime / 2, changeTime);
        StartCoroutine(ReverseDirect());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ruby"))
        {
            player = collision.gameObject.GetComponent<RubyController>();

            if (!player.IsInvincible)
            {
                player.TakeDamage(2);
            }
        }

        if (collision.gameObject.CompareTag("Projectile"))
        {
            Fix();
        }
    }

    private void ReverseDirection()
    {
        rigidbd2D.velocity = -rigidbd2D.velocity;

        direction = -direction;
        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);
    }

    private void Fix()
    {
        //CancelInvoke();
        StopAllCoroutines();
        rigidbd2D.simulated = false;
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
    }

    IEnumerator ReverseDirect()
    {
        yield return startTime;

        for(;;)
        {
            rigidbd2D.velocity = -rigidbd2D.velocity;

            direction = -direction;
            animator.SetFloat("MoveX", direction.x);
            animator.SetFloat("MoveY", direction.y);

            yield return repeatRate;
        }
    }
}
