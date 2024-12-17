using UnityEngine;

public class PortalController : MonoBehaviour
{
    public float moveDistance = 10f;
    public float pauseDuration = 1f;
    public float moveDuration = 2f;
    public KeyCode activationKey = KeyCode.E;

    private bool isTeleporting = false;
    private bool playerInRange = false;
    private Transform player;

    public Collider physicalBlocker;
    public FinalBossManager gameManager;

    private void Start()
    {
        if (physicalBlocker != null)
        {
            physicalBlocker.enabled = true;
        }
    }

    private void Update()
    {
        if (playerInRange && !isTeleporting && Input.GetKeyDown(activationKey))
        {
            if (physicalBlocker != null)
            {
                physicalBlocker.enabled = false;
            }
            StartCoroutine(TeleportPlayer(player));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
        }
    }

    private System.Collections.IEnumerator TeleportPlayer(Transform player)
    {
        isTeleporting = true;

        MovementController2 movement = player.GetComponent<MovementController2>();
        if (movement != null) movement.enabled = false;

        Camera mainCamera = Camera.main;
        Vector3 cameraOffset = mainCamera.transform.position - player.position;

        yield return new WaitForSeconds(pauseDuration);

        Vector3 startPosition = player.position;
        Vector3 targetPosition = startPosition + new Vector3(0, 0, moveDistance);
        float elapsedTime = 0;

        while (elapsedTime < moveDuration)
        {
            player.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);

            mainCamera.transform.position = player.position + cameraOffset;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.position = targetPosition;
        mainCamera.transform.position = player.position + cameraOffset;

        yield return new WaitForSeconds(pauseDuration);

        if (movement != null) movement.enabled = true;

        gameManager.TriggerShowGwyn();

        if (physicalBlocker != null)
        {
            physicalBlocker.enabled = true;
        }

        isTeleporting = false;
    }
}
