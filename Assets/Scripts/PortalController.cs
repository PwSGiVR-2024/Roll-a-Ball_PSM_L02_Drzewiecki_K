using UnityEngine;
using UnityEngine.UI;

public class PortalController : MonoBehaviour
{
    private float _moveDistance = 25f;
    private float _pauseDuration = 1f;
    private float _moveDuration = 3f;
    private KeyCode _activationKey = KeyCode.E;

    private bool _isTeleporting = false;
    private bool _isPlayerInRange = false;
    private Transform _player;

    public Collider PhysicalBlocker;
    public FinalBossManager GameManager;

    public RawImage EButton;

    private void Start()
    {
        if (PhysicalBlocker != null)
        {
            PhysicalBlocker.enabled = true;
        }

        if (EButton != null)
        {
            EButton.enabled = false;
        }
    }

    private void Update()
    {
        if (_isPlayerInRange && !_isTeleporting && Input.GetKeyDown(_activationKey))
        {
            if (PhysicalBlocker != null)
            {
                PhysicalBlocker.enabled = false;
            }

            if (EButton != null)
            {
                EButton.enabled = false;
            }

            StartCoroutine(TeleportPlayer(_player));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = true;
            _player = other.transform;

            if (EButton != null)
            {
                EButton.enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = false;
            _player = null;

            if (EButton != null)
            {
                EButton.enabled = false;
            }
        }
    }

    private System.Collections.IEnumerator TeleportPlayer(Transform player)
    {
        _isTeleporting = true;

        MovementController2 movement = player.GetComponent<MovementController2>();
        if (movement != null) movement.enabled = false;

        Camera mainCamera = Camera.main;
        Vector3 cameraOffset = mainCamera.transform.position - player.position;

        yield return new WaitForSeconds(_pauseDuration);

        Vector3 startPosition = player.position;
        Vector3 targetPosition = startPosition + new Vector3(0, 0, _moveDistance);
        float elapsedTime = 0;

        while (elapsedTime < _moveDuration)
        {
            player.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / _moveDuration);

            mainCamera.transform.position = player.position + cameraOffset;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.position = targetPosition;
        mainCamera.transform.position = player.position + cameraOffset;

        yield return new WaitForSeconds(_pauseDuration);

        if (movement != null) movement.enabled = true;

        GameManager.TriggerShowGwyn();

        if (PhysicalBlocker != null)
        {
            PhysicalBlocker.enabled = true;
        }

        _isTeleporting = false;
    }
}
