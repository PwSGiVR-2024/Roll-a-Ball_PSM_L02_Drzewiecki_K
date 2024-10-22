using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class Text2 : MonoBehaviour
{
    public TMP_Text text2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void winPrompt()
    {
        int score = gameObject.GetComponent<movementController>().score;
        if(score == 5)
        {
            text2.text = "WYGRALES!";
        }
    }
}
