using UnityEngine;
public class CameraController : MonoBehaviour
{
    private Transform player;
    Vector3 offset;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        offset = player.position - transform.position;
    }
    void Update()
    {
        transform.position = player.position - offset;
    }
}
