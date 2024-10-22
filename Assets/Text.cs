using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class Text : MonoBehaviour
{
    public TMP_Text text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void scoreUpdate()
    {
        int score = gameObject.GetComponent<movementController>().score;
        text.text = "Score: " + score;
    }
}
