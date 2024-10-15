using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectible : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Time.deltaTime * 50, Time.deltaTime * 50, Time.deltaTime * 50, Space.Self);
    }
    void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<movementController>().score++;
        Debug.Log("Zdobyles punkciora!");
        Debug.Log("Twoja liczba puntkow - " + other.gameObject.GetComponent<movementController>().score);
        
        gameObject.SetActive(false);
    }
}
