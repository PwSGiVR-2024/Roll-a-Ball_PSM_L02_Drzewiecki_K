using UnityEngine;

public class CubeController : MonoBehaviour
{
    private Rigidbody _rb;

    [SerializeField]
    private float pushForce = 2f;
    [SerializeField]
    private Vector3 _torqueValue;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 pushDirection = (transform.position - collision.transform.position).normalized;
            _rb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
            _rb.AddTorque(_torqueValue, ForceMode.Impulse);
        }
    }
}
