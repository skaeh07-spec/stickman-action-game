using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 8f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float mobileMoveInput = 0f;
    private bool facingRight = true;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Combat")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask enemyLayer;
    public int attackDamage = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float keyboardInput = Input.GetAxisRaw("Horizontal");
        float finalInput = keyboardInput != 0 ? keyboardInput : mobileMoveInput;

        rb.linearVelocity = new Vector2(finalInput * moveSpeed, rb.linearVelocity.y);

        // 캐릭터 좌우 방향 뒤집기
        if (finalInput > 0 && !facingRight) Flip();
        else if (finalInput < 0 && facingRight) Flip();

        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        // 키보드 테스트용 공격 (J키)
        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }
    }

    public void Jump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    public void SetMoveInput(float direction)
    {
        mobileMoveInput = direction;
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // 공격 버튼에서 호출할 함수
    public void Attack()
    {
        if (attackPoint == null) return;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("적 타격: " + enemy.name);
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(attackDamage);
            }
        }
    }

    // 씬 뷰에서 공격 범위 시각화 (디버그용)
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}