using UnityEngine;

public class Hint : MonoBehaviour
{
    [Header("Настройки подсказки")]
    public GameObject hintElement;
    public float interactionDistance = 2.0f;
    public bool showOnlyOnce = false;
    
    private Transform player;
    private bool wasShown = false;
    private bool isPlayerInRange = false;

    void Start()
    {
        FindPlayer();
        if (hintElement != null) 
        {
            hintElement.SetActive(false);
        }
    }

    void Update()
    {
        if (player == null)
        {
            FindPlayer();
            if (player == null) return;
        }

        CheckPlayerDistance();
        UpdateHintVisibility();
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void CheckPlayerDistance()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        isPlayerInRange = distance < interactionDistance;
    }

    void UpdateHintVisibility()
    {
        if (wasShown) return;

        if (hintElement.activeSelf != isPlayerInRange)
        {
            hintElement.SetActive(isPlayerInRange);
            
            if (isPlayerInRange && showOnlyOnce)
            {
                wasShown = true;
            }
        }
    }
}