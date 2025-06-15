using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float horizontal;
    private bool isFacingRight = true;
    private bool isGrounded;
    [Header("Общие настройки")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpingPower = 16f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private TMP_Text livesText;
    [Header("Настройки аудио")]
    [SerializeField] private string damageSound = "PlayerDamage";
    [SerializeField] private string jumpSound = "PlayerJump";
    [SerializeField] private string bonusSound= "PlayerBonus";
    [SerializeField] private string bonusLifeSound= "PlayerLifeBonus";

    private float currentJumpPower;
    private float currentSpeed;
    private int lives = 3;
    private bool isRespawning;
    private bool isImmune;

    private Animator animator;
    void Start()
    {
        currentJumpPower = jumpingPower;
        currentSpeed = speed;
        lives = 3;
        animator = GetComponent<Animator>();

        if (GameState.LivesOnGameOver == 0)
        {
            lives = 3;
        }
        else
        {
            lives = GameState.LivesOnGameOver;
        }

        UpdateLivesUI();
    }

    void Update()
    {
        if (isRespawning) return;

        horizontal = Input.GetAxisRaw("Horizontal");

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetBool("isJumping", !isGrounded);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            if (AudioManager.Instance != null) AudioManager.Instance.Play(jumpSound);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, currentJumpPower);
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        Flip();

        if (transform.position.y < -10f)
        {
            TakeDamage();
        }
    }

    private void FixedUpdate()
    {
        if (isRespawning) return;
        rb.linearVelocity = new Vector2(horizontal * currentSpeed, rb.linearVelocity.y);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public void TakeDamage()
    {
        if (isRespawning || isImmune) return;

        lives--;
        UpdateLivesUI();

        if (AudioManager.Instance != null)
            AudioManager.Instance.Play(damageSound);

        if (lives <= 0)
        {
            GameOver();
            return;
        }

        StartCoroutine(Respawn());
    }

    private void GameOver()
    {
        GameState.LivesOnGameOver = lives;
        GameState.ScoreOnGameOver = PlayerExperience.Instance.GetExperience();
        SceneManager.LoadScene("EndGame");
    }

    private IEnumerator Respawn()
    {
        isRespawning = true;
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;
        transform.position = spawnPoint.position;
        yield return new WaitForSeconds(0.5f);
        rb.simulated = true;
        isRespawning = false;
    }

    public void DoubleJumpPower(float jumpBonus, float speedBonus)
    {
        if (AudioManager.Instance != null) AudioManager.Instance.Play(bonusSound);
        currentJumpPower += jumpBonus;
        currentSpeed += speedBonus;
    }

    public void AddLife()
    {
        lives++;
        UpdateLivesUI();
        if (AudioManager.Instance != null) AudioManager.Instance.Play(bonusLifeSound);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Lava"))
        {
            TakeDamage();
        }
    }
    private void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = $"{lives}";
    }

}