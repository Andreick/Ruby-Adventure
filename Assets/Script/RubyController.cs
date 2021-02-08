using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private float speed;
    [SerializeField] private float timeInvincible;
    [SerializeField] private GameObject projectilePrefab;

    private Rigidbody2D rigidbd2D;
    private Animator animator;
    private AudioSource audioSource;
    private WaitForSeconds waitForSeconds;
    private Vector2 position, move, lookDirection;
    private int currentHealth;
    private float horizontal, vertical;
    private bool isInvincible;
    private GameObject projectileObject;
    private Projectile projectile;
    private Vector2 offset;

    public int MaxHealth { get { return maxHealth; } }
    public int CurrentHealth { get { return currentHealth; } }
    public bool IsInvincible { get { return isInvincible; } }

    void Awake()
    {
        rigidbd2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        currentHealth = maxHealth;
        isInvincible = false;
        waitForSeconds = new WaitForSeconds(timeInvincible);
        lookDirection = new Vector2(1, 0);

        offset = Vector2.up * 0.5f;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbd2D.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPersonCharacter character = hit.collider.gameObject.GetComponent<NonPersonCharacter>();
                character.DisplayDialog();
            }
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            move = Vector2.zero;
            Launch();
        }
        else
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            move = new Vector2(horizontal, vertical);

            if (!move.Equals(Vector2.zero))
            {
                lookDirection.Set(move.x, move.y);
                lookDirection.Normalize();

                animator.SetFloat("Look X", lookDirection.x);
                animator.SetFloat("Look Y", lookDirection.y);
            }

            animator.SetFloat("Speed", move.magnitude);
        }
    }

    private void FixedUpdate()
    {
        if (!move.Equals(Vector2.zero))
        {
            position = rigidbd2D.position;
            //position.x += Speed * horizontal * Time.deltaTime;
            //position.y += Speed * vertical * Time.deltaTime;
            position += move * speed * Time.deltaTime;
            rigidbd2D.MovePosition(position);
        }
    }

    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0,maxHealth);
        UIHealthBar.Instance.SetValue(currentHealth / (float)maxHealth);
    }

    public void TakeDamage(int amount)
    {
        isInvincible = true;

        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        animator.SetTrigger("Hit");
        UIHealthBar.Instance.SetValue(currentHealth / (float)maxHealth);

        StartCoroutine(DisableInvincibility());
    }

    IEnumerator DisableInvincibility()
    {
        yield return waitForSeconds;

        isInvincible = false;
    }

    private void Launch()
    {
        projectileObject = Instantiate(projectilePrefab, rigidbd2D.position + offset, Quaternion.identity);

        projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
