using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class collectible : MonoBehaviour
{
    public AudioSource MyAudioSource;
    public static event EventHandler e_CoinCollection;

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
        MyAudioSource.Play();
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        Invoke("DeactivateObject", MyAudioSource.clip.length);
        e_CoinCollection?.Invoke(this, EventArgs.Empty);

    }
    void DeactivateObject()
    {
        gameObject.SetActive(false);
    }
}
