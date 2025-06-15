using UnityEngine;
using System.Collections;

public class BonusLife : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 0.45f;

    private SpriteRenderer spriteRenderer;
    private Collider2D bonusCollider;
    private GameObject collectedEffect;
    private Animator collectAnimator;
    private float animationLength;
    private int collectStateHash;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        bonusCollider = GetComponent<Collider2D>();
        collectedEffect = transform.Find("Collected")?.gameObject;

        if (collectedEffect != null)
        {
            collectedEffect.SetActive(false);
            collectAnimator = collectedEffect.GetComponent<Animator>();

            if (collectAnimator != null && collectAnimator.runtimeAnimatorController != null)
            {
                RuntimeAnimatorController ac = collectAnimator.runtimeAnimatorController;
                if (ac.animationClips.Length > 0)
                {
                    animationLength = ac.animationClips[0].length;
                }

                collectStateHash = Animator.StringToHash("Collect");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (spriteRenderer != null) spriteRenderer.enabled = false;
            if (bonusCollider != null) bonusCollider.enabled = false;

            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.AddLife();
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
                if (collectAnimator.HasState(0, collectStateHash))
                {
                    collectAnimator.Play(collectStateHash, 0, 0f);
                    yield return new WaitForSeconds(animationLength);
                }
                else
                {
                    Debug.LogWarning("Состояние анимации 'Collect' не найдено!");
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