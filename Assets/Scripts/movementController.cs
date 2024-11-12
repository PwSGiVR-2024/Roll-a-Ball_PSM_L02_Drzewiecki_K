using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using System.Runtime.ConstrainedExecution;
using System;

using static collectible;
using static managerScript;

public class movementController : MonoBehaviour
{
    public bool a = true;
    public bool current = false;
    private float thrust = 0.2f;
    public static int lv = 0;
    public int score;
    public int maxScore;
    public Rigidbody rb;
    public TMP_Text text;
    public TMP_Text text2;
    public TMP_Text text3;
    public TMP_Text text4;
    public GameObject manager;
    public GameObject Button;
    public GameObject collisionImage;
    public AudioSource collisionAudio;
    public AudioSource levelCompleteAudio;
    private Vector3 startPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        e_CoinCollection += scoreUpdate;
        e_CoinCollection += winPrompt;
        //maxScore = managerScript.maxScore;

        if (collisionImage != null)
        {
            collisionImage.gameObject.SetActive(false);
        }

        if (collisionAudio == null)
        {
            collisionAudio = GetComponent<AudioSource>();
        }
    }
    void FixedUpdate()
    {
        udpatePosition();
    }
    private void udpatePosition()
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
            if (a == true)
            {
                rb.AddForce(0, 4, 0, ForceMode.Impulse);
            }
            a = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        a = true;

        if (collision.gameObject.CompareTag("ResetPlane"))
        {
            if (collisionAudio != null)
            {
                collisionAudio.Play();
            }

            StartCoroutine(ShowCollisionImageAndReset());
        }
    }
    IEnumerator ShowCollisionImageAndReset()
    {
        if (collisionImage != null)
        {
            collisionImage.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(1.5f);

        if (collisionImage != null)
        {
            collisionImage.gameObject.SetActive(false);
        }

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.position = startPosition;
    }

    public void scoreUpdate(object o, EventArgs e)
    {
        if (text != null)
        {
            score++;
            text.text = "Score: " + score;
            text4.text = "Aktualny poziom: " + lv+1;
        }
    }

    public void winPrompt(object o, EventArgs e)
    {
        if (score >= 8)
        {
            if (lv == 0)
            {
                Debug.Log("WYGRALES POZIOM 1!");
                if (text2 != null) text2.text = "WYGRALES POZIOM 1!";
                if (Button != null) Button.SetActive(true);
                if (levelCompleteAudio != null)
                {
                    levelCompleteAudio.Play();
                }
                score = 0;
                lv++;
            }
            else if (lv == 1)
            {
                Debug.Log("KONIEC GRY!");
                if (text2 != null) text2.text = "KONIEC GRY!";
                if (Button != null) Button.SetActive(true);
                if (levelCompleteAudio != null)
                {
                    levelCompleteAudio.Play();
                }
                score = 0;
                if (text3 != null) text3.text = "Zakoncz gre";
                lv++;
            }
        }
    }
}
