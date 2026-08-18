using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public float detectionRange = 5f;
    public float attackCooldown = 2f;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 5f;

    private Transform player;
    private float lastAttackTime = -999f;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.Log("Player를 찾지 못함! 태그 확인 필요");
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        Debug.Log("거리: " + distance + " / 감지범위: " + detectionRange + " / 쿨다운체크: " + (Time.time > lastAttackTime + attackCooldown));

        if (distance <= detectionRange && Time.time > lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }

    void Attack()
    {
        lastAttackTime = Time.time;
        Debug.Log("공격 시도!");

        if (projectilePrefab != null && firePoint != null)
        {
            GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D projRb = proj.GetComponent<Rigidbody2D>();

            if (projRb != null)
            {
                Vector2 direction = (player.position - firePoint.position).normalized;
                projRb.linearVelocity = direction * projectileSpeed;
            }
            else
            {
                Debug.Log("Projectile에 Rigidbody2D가 없음!");
            }
        }
        else
        {
            Debug.Log("Projectile Prefab 또는 Fire Point가 비어있음!");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}