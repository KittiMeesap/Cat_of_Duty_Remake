using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Character Stats")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float moveSpeed = 5f;
    public float armor = 0f;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        float finalDamage = Mathf.Max(damage - armor, 1);
        currentHealth -= finalDamage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + " has died.");
        Destroy(gameObject);
    }
}
