using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float mouseSensitivity = 100f;
    public float distanceFromPlayer = 5f;
    public Vector2 pitchLimits = new Vector2(-30f, 60f);

    private Vector3 offset;
    private float yaw = 0f;
    private float pitch = 0f;

    private bool isFinalLevel => MovementController.lv == 2;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if (isFinalLevel)
        {
            Cursor.lockState = CursorLockMode.Locked; // Ukrycie kursora i zablokowanie go na œrodku ekranu
            Cursor.visible = false;
        }
        else
        {
            offset = player.position - transform.position;
        }
    }

    void LateUpdate()
    {
        if (isFinalLevel)
        {
            FinalLevelCameraControl();
        }
        else
        {
            StandardCameraControl();
        }
    }

    private void StandardCameraControl()
    {
        transform.position = player.position - offset;
    }

    private void FinalLevelCameraControl()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, pitchLimits.x, pitchLimits.y);

        Vector3 cameraDirection = new Vector3(0, 0, -distanceFromPlayer);
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        transform.position = player.position + rotation * cameraDirection;

        transform.LookAt(player);
    }
}
