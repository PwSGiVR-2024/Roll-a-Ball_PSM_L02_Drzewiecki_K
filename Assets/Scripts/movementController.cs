using UnityEngine;
using TMPro;
using System;

using static Collectible;

public class MovementController : MonoBehaviour
{
    public bool a = true;
    public bool current = false;
    private float thrust = 0.2f;
    public static int lv = 0;
    public int score;
    public int maxScore;
    public Rigidbody rb;
    public TMP_Text text1;
    public TMP_Text text2;
    public TMP_Text text3;
    public TMP_Text text4;
    public GameObject manager;
    public GameObject button;
    public GameObject collisionImage;
    public AudioSource collisionAudio;
    public AudioSource levelCompleteAudio;
    private ManagerScript gameManagerScript;
    private Vector3 startPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        e_CoinCollection += ScoreUpdate;
        e_CoinCollection += WinPrompt;

        CollisionComponents();
        MaxScore();
    }
    void FixedUpdate()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
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
                rb.AddForce(0, 5, 0, ForceMode.Impulse);
            }
            a = false;
        }
    }

    private void MaxScore()
    {
        if (manager != null)
        {
            gameManagerScript = manager.GetComponent<ManagerScript>();
            if (gameManagerScript != null)
            {
                maxScore = gameManagerScript.GetMaxScore();
            }
        }
    }
    private void CollisionComponents()
    {
        if (collisionImage != null)
        {
            collisionImage.gameObject.SetActive(false);
        }

        if (collisionAudio == null)
        {
            collisionAudio = GetComponent<AudioSource>();
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

            ShowCollisionImageAndReset();
        }
    }

    private void ScoreUpdate(object o, EventArgs e)
    {
        if (text1 != null)
        {
            score++;
            text1.text = "Score: " + score;
            text4.text = "Aktualny poziom: " + (lv + 1);
        }
    }

    private void WinPrompt(object o, EventArgs e)
    {
        if (score >= maxScore)
        {
            if (lv == 0)
            {
                if (text2 != null) text2.text = "WYGRA£EŒ POZIOM 1!";
                if (button != null) button.SetActive(true);
                if (levelCompleteAudio != null) levelCompleteAudio.Play();
                lv++;
                score = 0;
            }
            else if (lv == 1)
            {
                if (text2 != null) text2.text = "KONIEC GRY!";
                if (button != null) button.SetActive(true);
                if (levelCompleteAudio != null) levelCompleteAudio.Play();
                if (text3 != null) text3.text = "Zakoñcz grê";
                lv = 0;
                score = 0;
            }
        }
    }
    private void ShowCollisionImageAndReset()
    {
        if (collisionImage != null)
        {
            collisionImage.gameObject.SetActive(true);
        }

        Invoke(nameof(ResetPosition), 1.5f);
    }

    private void ResetPosition()
    {
        if (collisionImage != null)
        {
            collisionImage.gameObject.SetActive(false);
        }

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = startPosition;
    }

}