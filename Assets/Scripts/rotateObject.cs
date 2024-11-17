using UnityEngine;

public class RotateObject : MonoBehaviour
{
    private float rotationSpeed = 300.0f;

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
