using UnityEngine;

public class MovementController2 : MonoBehaviour
{
    private Rigidbody rb;
    private float thrust = 0.2f;
    private Transform cameraTransform;

    public bool canJump = true; // Czy gracz mo�e skaka�

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform; // Pobranie transformu g��wnej kamery
    }

    void FixedUpdate()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        // Pobierz osie ruchu od gracza
        float horizontalInput = 0f;
        if (Input.GetKey(KeyCode.A))
            horizontalInput = -1f;
        else if (Input.GetKey(KeyCode.D))
            horizontalInput = 1f;

        float verticalInput = 0f;
        if (Input.GetKey(KeyCode.W))
            verticalInput = 1f;
        else if (Input.GetKey(KeyCode.S))
            verticalInput = -1f;

        // Oblicz wektor kierunku wzgl�dem kamery
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Wyzeruj komponent Y (wysoko��), by ograniczy� ruch do p�aszczyzny XZ
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        // Kierunek ruchu
        Vector3 movementDirection = (forward * verticalInput + right * horizontalInput).normalized;

        // Zastosuj si�� ruchu
        rb.AddForce(movementDirection * thrust, ForceMode.Impulse);

        // Skok
        if (Input.GetKey(KeyCode.Space) && canJump)
        {
            rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
            canJump = false; // Blokada wielokrotnego skoku
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Odblokuj skok przy dotkni�ciu powierzchni
        canJump = true;
    }
}
