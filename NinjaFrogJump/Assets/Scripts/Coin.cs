using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    public int experiencePoints = 50;
    [SerializeField] private string soundKey = "CherryPickUp";
    [SerializeField] private float destroyDelay = 0.45f;

    private SpriteRenderer spriteRenderer;
    private Collider2D coinCollider;
    private GameObject collectedEffect;
    private Animator collectAnimator;
    private float animationLength;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        coinCollider = GetComponent<Collider2D>();
        collectedEffect = transform.Find("Collected")?.gameObject;

        if (collectedEffect != null)
        {
            collectedEffect.SetActive(false);
            collectAnimator = collectedEffect.GetComponent<Animator>();

            if (collectAnimator != null)
            {
                RuntimeAnimatorController ac = collectAnimator.runtimeAnimatorController;
                if (ac.animationClips.Length > 0)
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
            if (AudioManager.Instance != null) AudioManager.Instance.Play(soundKey);
            if (PlayerExperience.Instance != null) PlayerExperience.Instance.AddExperience(experiencePoints);
            spriteRenderer.enabled = false;
            coinCollider.enabled = false;
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