using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    [SerializeField] AudioClip collectedClip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        RubyController controller = collision.GetComponent<RubyController>();

        if (controller != null)
        {
            if (controller.CurrentHealth < controller.MaxHealth)
            {
                controller.ChangeHealth(1);
                Destroy(gameObject);

                controller.PlaySound(collectedClip);
            }
        }
    }
}
