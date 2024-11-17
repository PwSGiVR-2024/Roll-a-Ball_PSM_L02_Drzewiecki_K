using UnityEngine;

public class WindaControl : MonoBehaviour
{
    public float speed = 2.5f;
    private float range = 5.0f;
    private int direction = 1;

    private float startPositionY;

    void Start()
    {
        startPositionY = transform.position.y;
    }

    void Update()
    {
        transform.position += new Vector3(0, speed * direction * Time.deltaTime, 0);

        if (transform.position.y >= startPositionY + range || transform.position.y <= startPositionY - range)
        {
            direction *= -1;
        }
    }
}
