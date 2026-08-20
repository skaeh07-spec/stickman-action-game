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

    [Header("Checkpoint")]
    private Vector3 respawnPoint;
    private Rigidbody2D rb;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        rb = GetComponent<Rigidbody2D>();
        respawnPoint = transform.position;
    }

    public void SetCheckpoint(Vector3 position)
    {
        respawnPoint = position;
        Debug.Log("체크포인트 저장: " + position);
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

    // 구덩이 낙사 전용: 체력 1만 깎고 체크포인트로 리스폰
    public void FallDeath()
    {
        if (isInvincible) return;

        currentHealth -= 1;
        if (currentHealth < 0) currentHealth = 0;

        Debug.Log("낙사! 남은 체력: " + currentHealth);
        UpdateHealthUI();

        if (audioSource != null && hurtSound != null)
        {
            audioSource.PlayOneShot(hurtSound);
        }

        if (currentHealth <= 0)
        {
            GameOver();
        }
        else
        {
            Respawn();
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
        Debug.Log("Player 사망! 체력이 0이 됨");
        GameOver();
    }

    void Respawn()
    {
        transform.position = respawnPoint;
        UpdateHealthUI();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void GameOver()
    {
        Debug.Log("게임 오버!");
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        Time.timeScale = 0f;
    }
}