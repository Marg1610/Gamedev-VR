using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("Настройки портала")]
    [SerializeField] private Portal linkedPortal;
    [SerializeField] private float teleportCooldown = 0.5f;
    [SerializeField] private string soundKey = "PlayerTeleport";

    private Transform teleportPoint;
    private bool isOnCooldown;

    void Start()
    {
        teleportPoint = transform.Find("Destination");
        if (teleportPoint == null)
        {
            Debug.LogError($"Телепортная точка не найдена у портала {name}!");
        }

        if (linkedPortal == null)
        {
            Debug.LogError($"Портал {name} не имеет парной связи!");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
        {
            if (isOnCooldown) return;

            if (collision.CompareTag("Player"))
            {
                TeleportPlayer(collision.transform);
                AudioManager.Instance.Play(soundKey);
        }
    }

    private void TeleportPlayer(Transform player)
    {
        if (linkedPortal == null || linkedPortal.teleportPoint == null) return;

        player.position = linkedPortal.teleportPoint.position;
        StartCoroutine(TeleportCooldown());
        linkedPortal.StartCoroutine(linkedPortal.TeleportCooldown());

        Debug.Log($"Игрок телепортирован к {linkedPortal.name}");
    }

    private System.Collections.IEnumerator TeleportCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(teleportCooldown);
        isOnCooldown = false;
    }
}