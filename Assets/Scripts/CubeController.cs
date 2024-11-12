using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField]
    float pushForce = 2f;
    [SerializeField]
    Vector3 torqueValue;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 pushDirection = (transform.position - collision.transform.position).normalized;
            rb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
            rb.AddTorque(torqueValue, ForceMode.Impulse);
        }
    }
}
