using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using System.Runtime.ConstrainedExecution;

public class movementController : MonoBehaviour
{
    public float thrust = 0.01f;
    public Rigidbody rb;
    public int score;
    public static int lv = 0;
    public bool a = true;
    public bool current = false;
    public TMP_Text text;
    public TMP_Text text2;
    public TMP_Text text3;
    public GameObject Button;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(0, 0, thrust, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(0, 0, -thrust, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(thrust, 0, 0, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(-thrust, 0, 0, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.Space))
        {
           if(a == true)
           {
                rb.AddForce(0, 5, 0, ForceMode.Impulse);
           }
           a = false;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        a = true;
    }

    public void scoreUpdate()
    {
        score++;
        Debug.Log("Zdobyles punkciora!");
        Debug.Log("Twoja liczba puntkow - " + score);
        text.text = "Score: " + score;
    }
    public void winPrompt()
    {
        if (score >= 8)
        {
            if (lv == 0)
            {
                Debug.Log("WYGRALES POZIOM 1!");
                text2.text = "WYGRALES POZIOM 1!";
                Button.SetActive(true);
                score = 0;
                lv++;
            }
            else if (lv == 1)
            {
                Debug.Log("KONIEC GRY!");
                text2.text = "KONIEC GRY!";
                Button.SetActive(true);
                score = 0;
                text3.text = "Zakoncz gre";
            }
        }
    }
}
