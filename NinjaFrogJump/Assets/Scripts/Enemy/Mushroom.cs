using UnityEngine;

public class Mushroom : Enemy
{
    [Header("Настройки патрулирования")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float moveSpeed = 2f;
    private Transform currentTarget;
    private bool isFacingRight = false;

    protected override void Start()
    {
        base.Start();
        currentTarget = pointA;
        UpdateDirection();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            currentTarget.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            currentTarget = (currentTarget == pointA) ? pointB : pointA;
            UpdateDirection();
        }
    }

    private void UpdateDirection()
    {
        bool shouldFaceRight = (currentTarget.position.x > transform.position.x);
        if (shouldFaceRight != isFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.contacts[0].normal.y < -0.5f)
        {
            TakeDamage();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage();
            }
        }
    }
}
