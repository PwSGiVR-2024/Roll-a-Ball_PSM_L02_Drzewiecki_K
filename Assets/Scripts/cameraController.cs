using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public GameObject Player;
    public movementController playerController;
    public Vector3 offset;
    void Start()
    {
        offset = transform.position - Player.transform.position;
    }
    void Update()
    {
        transform.position = offset + Player.transform.position;
    }
}
