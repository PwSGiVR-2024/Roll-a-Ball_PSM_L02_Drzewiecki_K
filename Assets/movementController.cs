using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class movementController : MonoBehaviour
{
    public float thrust = 0.01f;
    public Rigidbody rb;
    public int score;
    public bool a = true;
    public TMP_Text text;
    public TMP_Text text2;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
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
        if (score >= 5)
        {
            Debug.Log("WYGRALES!");
            text2.text = "WYGRALES!";
        }
    }
}
