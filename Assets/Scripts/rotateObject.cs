using UnityEngine;

public class RotateObject : MonoBehaviour
{
    private float _rotationSpeed = 300.0f;

    void Update()
    {
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
    }
}
