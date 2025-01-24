using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float mouseSensitivity = 75f;
    public float distanceFromPlayer = 5f;
    public Vector2 pitchLimits = new Vector2(-30f, 60f);

    private Vector3 _offset;
    private float _yaw = 0f;
    private float _pitch = 0f;

    private bool _isFinalLevel => ManagerScript.lv == 2;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if (_isFinalLevel)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            _offset = player.position - transform.position;
        }
    }

    void LateUpdate()
    {
        if (_isFinalLevel)
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
        transform.position = player.position - _offset;
    }

    private void FinalLevelCameraControl()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        _yaw += mouseX;
        _pitch -= mouseY;
        _pitch = Mathf.Clamp(_pitch, pitchLimits.x, pitchLimits.y);

        Vector3 cameraDirection = new Vector3(0, 0, -distanceFromPlayer);
        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0);
        transform.position = player.position + rotation * cameraDirection;

        transform.LookAt(player);
    }
}
