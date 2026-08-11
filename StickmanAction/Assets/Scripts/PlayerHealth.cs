using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;

    [Header("Damage Settings")]
    public float invincibilityDuration = 1f;
    private bool isInvincible = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        Debug.Log("Player 체력: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityFrame());
        }
    }

    System.Collections.IEnumerator InvincibilityFrame()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }

    void Die()
    {
        Debug.Log("Player 사망!");
        // 나중에 게임오버 화면 연결
    }
}