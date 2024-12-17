using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

using static Collectible;

public class MovementController : MonoBehaviour
{
    public static int lv = 0;
    public bool canJump = true;
    public bool a = true;
    public int score;
    public int maxScore;
    private bool levelCompleted = false;
    public TMP_Text text1;
    public TMP_Text text2;
    public TMP_Text text3;
    public TMP_Text text4;
    public GameObject button;
    public GameObject collisionImage;
    public AudioSource collisionAudio;
    public AudioSource levelCompleteAudio;

    private float thrust = 0.2f;
    private Rigidbody rb;
    private Vector3 startPosition;
    private ManagerScript gameManagerScript;
    private Transform cameraTransform;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        cameraTransform = Camera.main.transform;
        UpdateLevelBasedOnScene();
    }

    void Start()
    {
        e_CoinCollection += ScoreUpdate;
        e_CoinCollection += WinPrompt;
        CollisionComponents();
        MaxScore();
    }

    void FixedUpdate()
    {
        if (lv == 2)
        {
            FinalBossMovement();
        }
        else
        {
            StandardMovement();
        }
    }

    private void UpdateLevelBasedOnScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "NowiutkiPoziom")
        {
            lv = 1;
        }
        else if (sceneName == "FinalBoss")
        {
            lv = 2;
        }
        else if (sceneName == "menu")
        {
            lv = 0;
        }
        else
        {
            lv = 0;
        }
    }

    private void StandardMovement()
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
        if (Input.GetKey(KeyCode.Space) && a)
        {
            rb.AddForce(0, 5, 0, ForceMode.Impulse);
            a = false;
        }
    }

    private void FinalBossMovement()
    {
        float horizontalInput = 0f;
        if (Input.GetKey(KeyCode.A)) horizontalInput = -1f;
        else if (Input.GetKey(KeyCode.D)) horizontalInput = 1f;

        float verticalInput = 0f;
        if (Input.GetKey(KeyCode.W)) verticalInput = 1f;
        else if (Input.GetKey(KeyCode.S)) verticalInput = -1f;

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 movementDirection = (forward * verticalInput + right * horizontalInput).normalized;

        rb.AddForce(movementDirection * thrust, ForceMode.Impulse);

        if (Input.GetKey(KeyCode.Space) && canJump)
        {
            rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
            canJump = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("ResetPlane"))
        {
            canJump = true;
            a = true;
        }

        if (collision.gameObject.CompareTag("ResetPlane"))
        {
            if (collisionAudio != null)
            {
                collisionAudio.Play();
            }

            ShowCollisionImageAndReset();
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
        transform.position = startPosition;
    }

    private void MaxScore()
    {
        GameObject[] collectibles = GameObject.FindGameObjectsWithTag("collectible");
        maxScore = collectibles.Length;

        if (text4 != null)
        {
            text4.text = $"Aktualny poziom: {lv + 1}, Maksymalny wynik: {maxScore}";
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
        if (levelCompleted) return;

        if (score >= maxScore)
        {
            levelCompleted = true;

            if (lv == 0)
            {
                if (text2 != null) text2.text = "WYGRAŁEŚ POZIOM 1!";
                if (button != null) button.SetActive(true);
                if (levelCompleteAudio != null) levelCompleteAudio.Play();
            }
            else if (lv == 1)
            {
                if (text2 != null) text2.text = "WYGRAŁEŚ POZIOM 2!";
                if (button != null) button.SetActive(true);
                if (levelCompleteAudio != null) levelCompleteAudio.Play();
            }
            else if (lv == 2)
            {
                if (text2 != null) text2.text = "KONIEC GRY!";
                if (button != null) button.SetActive(true);
                if (levelCompleteAudio != null) levelCompleteAudio.Play();
                if (text3 != null) text3.text = "Zakończ grę";
            }
        }
    }
}
