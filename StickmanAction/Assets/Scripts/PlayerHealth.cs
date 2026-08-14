using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;

    [Header("Damage Settings")]
    public float invincibilityDuration = 1f;
    private bool isInvincible = false;

    [Header("UI")]
    public TextMeshProUGUI healthText;
    public GameObject gameOverPanel;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip hurtSound;

    [Header("Visual Feedback")]
    public SpriteRenderer spriteRenderer;
    public float flashInterval = 0.1f;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        Debug.Log("Player 체력: " + currentHealth);

        if (audioSource != null && hurtSound != null)
        {
            audioSource.PlayOneShot(hurtSound);
        }

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityFrame());
        }
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth;
        }
    }

    System.Collections.IEnumerator InvincibilityFrame()
    {
        isInvincible = true;

        float elapsed = 0f;
        while (elapsed < invincibilityDuration)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
            }
            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }

        isInvincible = false;
    }

    void Die()
    {
        Debug.Log("Player 사망!");
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        Time.timeScale = 0f;
    }
}