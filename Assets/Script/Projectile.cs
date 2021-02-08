using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rigidbd2D;

    void Awake()
    {
        rigidbd2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Invoke("Cleanup", 2);
    }


    void Update()
    {
        
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbd2D.AddForce(direction * force);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }

    private void Cleanup()
    {
        Destroy(gameObject);
    }
}
