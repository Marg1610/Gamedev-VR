using UnityEngine;
using System.Collections;

public class Bonus : MonoBehaviour
{
    [Header("Настройки бонуса")]
    [SerializeField] private float jumpBonusAmount = 5f;
    [SerializeField] private float speedBonusAmount = 5f;
    [SerializeField] private float destroyDelay = 0.45f;

    private SpriteRenderer spriteRenderer;
    private Collider2D bonusCollider;
    private GameObject collectedEffect;
    private Animator collectAnimator;
    private float animationLength;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        bonusCollider = GetComponent<Collider2D>();
        collectedEffect = transform.Find("Collected")?.gameObject;

        if (collectedEffect != null)
        {
            collectedEffect.SetActive(false);
            collectAnimator = collectedEffect.GetComponent<Animator>();

            if (collectAnimator != null)
            {
                RuntimeAnimatorController ac = collectAnimator.runtimeAnimatorController;
                if (ac != null && ac.animationClips.Length > 0)
                {
                    animationLength = ac.animationClips[0].length;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (spriteRenderer != null) spriteRenderer.enabled = false;
            if (bonusCollider != null) bonusCollider.enabled = false;

            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.DoubleJumpPower(jumpBonusAmount, speedBonusAmount);
            }

            StartCoroutine(CollectRoutine());
        }
    }

    private IEnumerator CollectRoutine()
    {
        if (collectedEffect != null)
        {
            collectedEffect.SetActive(true);

            if (collectAnimator != null)
            {
                collectAnimator.Play("Collect", 0, 0f);

                yield return new WaitForSeconds(animationLength);
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