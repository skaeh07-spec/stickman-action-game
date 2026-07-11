using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 8f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float mobileMoveInput = 0f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 키보드 입력 (PC 테스트용)
        float keyboardInput = Input.GetAxisRaw("Horizontal");

        // 키보드나 모바일 버튼 중 입력 있는 쪽 사용
        float finalInput = keyboardInput != 0 ? keyboardInput : mobileMoveInput;

        rb.linearVelocity = new Vector2(finalInput * moveSpeed, rb.linearVelocity.y);

        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    public void Jump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    // 버튼 누르고 있을 때 호출 (왼쪽: -1, 오른쪽: 1)
    public void SetMoveInput(float direction)
    {
        mobileMoveInput = direction;
    }
}