using UnityEngine;

public class CameraController2 : MonoBehaviour
{
    public Transform player; // Odwo³anie do gracza
    public float mouseSensitivity = 100f; // Czu³oœæ myszy
    public float distanceFromPlayer = 5f; // Odleg³oœæ kamery od gracza
    public Vector2 pitchLimits = new Vector2(-30f, 60f); // Ograniczenia ruchu kamery w osi Y (góra/dó³)

    private float yaw = 0f; // K¹t obrotu w poziomie
    private float pitch = 0f; // K¹t obrotu w pionie

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Ukrycie kursora i zablokowanie go na œrodku ekranu
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // Pobieranie ruchu myszy
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Aktualizacja k¹tów obrotu
        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, pitchLimits.x, pitchLimits.y); // Ograniczenie k¹ta patrzenia w osi pionowej

        // Obrót kamery wokó³ gracza
        Vector3 cameraDirection = new Vector3(0, 0, -distanceFromPlayer);
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        transform.position = player.position + rotation * cameraDirection;

        // Ustawienie kamery, aby patrzy³a na gracza
        transform.LookAt(player);
    }
}
