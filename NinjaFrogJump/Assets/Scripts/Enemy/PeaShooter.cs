using UnityEngine;

public class PeaShooter : Enemy
{
    [Header("Настройки стрельбы")]
    [SerializeField] private GameObject peaPrefab;
    [SerializeField] private float shootCooldown = 2f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float minDistance = 1f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform shootPoint;

    private bool isFacingRight = true;
    private float lastShootTime;

    protected override void Start()
    {
        base.Start();
        if (peaPrefab == null || shootPoint == null)
        {
            Debug.LogError("Не назначены префаб или точка выстрела!");
            enabled = false;
        }
    }

    void Update()
    {
        if (player == null) return;


        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer > detectionRange || distanceToPlayer < minDistance) return;

        float playerDirection = player.position.x - transform.position.x;
        bool shouldFaceRight = playerDirection > 0;

        if (shouldFaceRight != isFacingRight)
        {
            Flip();
        }

        Vector2 rayDirection = isFacingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(
            shootPoint.position,
            rayDirection,
            detectionRange,
            playerLayer
        );

        if (hit.collider != null && hit.collider.CompareTag("Player")
            && Time.time >= lastShootTime + shootCooldown)
        {
            Shoot();
            lastShootTime = Time.time;
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void Shoot()
    {
        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
        GameObject pea = Instantiate(peaPrefab, shootPoint.position, Quaternion.identity);
        pea.GetComponent<Projectile>().SetDirection(direction);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.contacts[0].normal.y < -0.5f)
        {
            TakeDamage();
        }
    }
}