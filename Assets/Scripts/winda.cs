using UnityEngine;

public class WindaControl : MonoBehaviour
{
    public float Speed = 2.5f;

    private float _range = 6f;
    private int _direction = 1;
    private float _startPositionY;

    private void Start()
    {
        _startPositionY = transform.position.y;
    }

    private void Update()
    {
        transform.position += new Vector3(0, Speed * _direction * Time.deltaTime, 0);

        if (transform.position.y >= _startPositionY + _range || transform.position.y <= _startPositionY - _range)
        {
            _direction *= -1;
        }
    }
}
