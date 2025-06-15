using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private string leverSoundKey = "LeverSound";
    [SerializeField] private GameObject objectToMove;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float interactionDistance = 2.0f;
    [SerializeField] private float moveSpeed = 5.0f;

    private bool isAtPointA = true;
    private bool isMoving = false;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        bool isPlayerNear = distance < interactionDistance;

        if (isPlayerNear && Input.GetKeyDown(KeyCode.E) && !isMoving)
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.Play(leverSoundKey);

            isMoving = true;
            isAtPointA = !isAtPointA;

        }

        if (isMoving)
        {
            MoveObject();
        }
    }

    void MoveObject()
    {
        Vector3 targetPosition = isAtPointA ? pointA.position : pointB.position;
        objectToMove.transform.position = Vector3.MoveTowards(
            objectToMove.transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(objectToMove.transform.position, targetPosition) < 0.01f)
        {
            isMoving = false;
        }
    }
}
