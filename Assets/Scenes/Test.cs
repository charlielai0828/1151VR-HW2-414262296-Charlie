
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    // Vector2 陣列：記錄各平台與終點的位置
    public Vector2[] routePoints;

    // 目前要抵達的路線點
    private int currentPoint = 0;

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool isFinished = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isFinished) return;

        // 使用 Vector2 表示人物的移動方向
        float horizontal = Input.GetAxisRaw("Horizontal");
        Vector2 movement = new Vector2(horizontal, 0f);

        // 使用 Vector2 控制水平速度
        rb.linearVelocity = new Vector2(
            movement.x * moveSpeed,
            rb.linearVelocity.y
        );

        // 按空白鍵跳躍
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );

            isGrounded = false;
        }

        // 使用 Vector2 陣列檢查目前的路線點
        if (routePoints != null &&
            currentPoint < routePoints.Length)
        {
            Vector2 target = routePoints[currentPoint];

            if (Vector2.Distance(transform.position, target) < 0.5f)
            {
                currentPoint++;
            }
        }

        // 抵達最後一個路線點
        if (routePoints != null &&
            routePoints.Length > 0 &&
            currentPoint >= routePoints.Length)
        {
            isFinished = true;
            rb.linearVelocity = Vector2.zero;
            Debug.Log("恭喜！抵達終點！");
        }
    }

    // 碰到地面或平台時，可以再次跳躍
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts.Length > 0 &&
            collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    // 繪製路線點，方便在 Scene 視窗查看
    void OnDrawGizmosSelected()
    {
        if (routePoints == null) return;

        Gizmos.color = Color.yellow;

        for (int i = 0; i < routePoints.Length; i++)
        {
            Gizmos.DrawSphere(routePoints[i], 0.2f);

            if (i < routePoints.Length - 1)
            {
                Gizmos.DrawLine(
                    routePoints[i],
                    routePoints[i + 1]
                );
            }
        }
    }
}
