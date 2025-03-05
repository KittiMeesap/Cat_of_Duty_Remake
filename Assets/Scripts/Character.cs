using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Character Stats")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float moveSpeed = 5f;
    public float armor = 0f;

    [Header("Lives System")]
    public int maxLives = 3;
    public int currentLives;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        currentLives = maxLives;
    }

    public void TakeDamage(float damage)
    {
        float finalDamage = Mathf.Max(damage - armor, 1);
        currentHealth -= finalDamage;

        if (currentHealth <= 0)
        {
            LoseLife();
        }
    }

    private void LoseLife()
    {
        currentLives--;

        if (currentLives > 0)
        {
            Debug.Log(gameObject.name + " lost a life. Lives remaining: " + currentLives);
            currentHealth = maxHealth; // รีเซ็ตเลือดเมื่อเสียชีวิตหนึ่งครั้ง
        }
        else
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + " has run out of lives and died.");
        Destroy(gameObject);
    }
}
