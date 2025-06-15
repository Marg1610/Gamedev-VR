using UnityEngine;
using System.Collections;

public abstract class Enemy : MonoBehaviour
{
    [Header("Общие настройки")]
    [SerializeField] protected int health = 1;
    [SerializeField] private string damageSoundKey = "EnemyDamage";
    [SerializeField] private float destroyDelay = 0.5f;
    [SerializeField] private int experiencePoints = 75;

    protected Transform player;
    protected Rigidbody2D rb;

    private SpriteRenderer spriteRenderer;
    private Collider2D[] enemyColliders;
    private GameObject hitEffect;
    private Animator hitAnimator;
    private float animationLength;
    private int hitStateHash;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();

        CacheDeathComponents();
    }

    private void CacheDeathComponents()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyColliders = GetComponents<Collider2D>();

        hitEffect = transform.Find("Hit")?.gameObject;

        if (hitEffect != null)
        {
            hitEffect.SetActive(false);
            hitAnimator = hitEffect.GetComponent<Animator>();

            if (hitAnimator != null && hitAnimator.runtimeAnimatorController != null)
            {
                RuntimeAnimatorController ac = hitAnimator.runtimeAnimatorController;
                if (ac.animationClips.Length > 0)
                {
                    animationLength = ac.animationClips[0].length;
                }

                hitStateHash = Animator.StringToHash("Hit");
            }
        }
    }

    public virtual void TakeDamage()
    {
        health--;
        StartCoroutine(DeathRoutine());

    }

    private IEnumerator DeathRoutine()
    {
        if (spriteRenderer != null) spriteRenderer.enabled = false;
        foreach (Collider2D col in enemyColliders)
        {
            col.enabled = false;
        }

        if (rb != null) rb.simulated = false;

        if (AudioManager.Instance != null)
            AudioManager.Instance.Play(damageSoundKey);

        if (PlayerExperience.Instance != null) PlayerExperience.Instance.AddExperience(experiencePoints);

        if (hitEffect != null)
        {
            hitEffect.SetActive(true);

            if (hitAnimator != null)
            {
                if (hitAnimator.HasState(0, hitStateHash))
                {
                    hitAnimator.Play(hitStateHash, 0, 0f);
                    yield return new WaitForSeconds(animationLength);
                }
                else
                {
                    Debug.LogWarning("Состояние анимации 'Hit' не найдено!");
                    yield return new WaitForSeconds(destroyDelay);
                }
            }
            else
            {
                yield return new WaitForSeconds(destroyDelay);
            }
        }
        else
        {
            yield return new WaitForSeconds(destroyDelay);
        }

        Destroy(gameObject);
    }
}