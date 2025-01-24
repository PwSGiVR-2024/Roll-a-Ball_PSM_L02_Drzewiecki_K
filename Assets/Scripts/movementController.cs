using UnityEngine;

public class MovementController : MonoBehaviour
{
    public bool CanJump = true;
    public bool IsJumping = true;

    private float _thrust = 0.2f;
    private Rigidbody _rb;
    private Vector3 _startPosition;
    private Transform _cameraTransform;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _startPosition = transform.position;
        _cameraTransform = Camera.main.transform;

    }

    void FixedUpdate()
    {
        if (ManagerScript.Instance.IsDialogueActive())
            return;

        if (ManagerScript.lv == 2)
        {
            FinalBossMovement();
        }
        else
        {
            StandardMovement();
        }
    }


    private void StandardMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _rb.AddForce(0, 0, _thrust, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.S))
        {
            _rb.AddForce(0, 0, -_thrust, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.D))
        {
            _rb.AddForce(_thrust, 0, 0, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.A))
        {
            _rb.AddForce(-_thrust, 0, 0, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.Space) && IsJumping)
        {
            _rb.AddForce(0, 5, 0, ForceMode.Impulse);
            IsJumping = false;
        }
    }

    private void FinalBossMovement()
    {
        Vector3 forward = _cameraTransform.forward;
        Vector3 right = _cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection -= forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += right;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection -= right;
        }

        _rb.AddForce(moveDirection.normalized * _thrust, ForceMode.Impulse);

        if (Input.GetKey(KeyCode.Space) && IsJumping)
        {
            _rb.AddForce(Vector3.up * 5, ForceMode.Impulse);
            IsJumping = false;
        }
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("ResetPlane"))
        {
            CanJump = true;
            IsJumping = true;
        }

        if (collision.gameObject.CompareTag("ResetPlane"))
        {
            ManagerScript.Instance.HandlePlayerFall();
            ResetPosition();
        }
    }

    private void ResetPosition()
    {
        _rb.linearVelocity = Vector3.zero;
        transform.position = _startPosition;
    }

}
