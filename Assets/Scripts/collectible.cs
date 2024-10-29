using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectible : MonoBehaviour
{
    public AudioSource MyAudioSource;

    void Start()
    {
        MyAudioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        transform.Rotate(Time.deltaTime * 50, Time.deltaTime * 50, Time.deltaTime * 50, Space.Self);
    }
    void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<movementController>().scoreUpdate();
        other.gameObject.GetComponent<movementController>().winPrompt();
        MyAudioSource.Play();
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        Invoke("DeactivateObject", MyAudioSource.clip.length);
    }
    void DeactivateObject()
    {
        gameObject.SetActive(false);
    }
}
