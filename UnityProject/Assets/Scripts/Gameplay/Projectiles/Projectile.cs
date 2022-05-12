using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float damageAmount = 10f;

    [SerializeField]
    private GameObject damageParticles;
    [SerializeField]
    private GameObject noDamageParticles;

    private string tagMask = "Untagged";

    public void SetTagMask(string tag)
    {
        if (tag != null && tag != "")
        {
            tagMask = tag;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision.gameObject);
    }

    private void HandleCollision(GameObject other)
    {
        if (!other.gameObject.CompareTag(tagMask))
        {
            IDamageable enemyHealth = other.gameObject.GetComponent<IDamageable>();
            if (enemyHealth != null)
            {
                enemyHealth.Damage(damageAmount);
                Instantiate(damageParticles, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(noDamageParticles, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}
