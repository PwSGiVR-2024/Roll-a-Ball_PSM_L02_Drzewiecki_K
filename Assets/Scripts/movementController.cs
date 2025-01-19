using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

using static Collectible;

public class MovementController : MonoBehaviour
{
    public static int lv = 0;
    public bool CanJump = true;
    public bool IsJumping = true;
    public int Score;
    public int MaxScore;
    private bool LevelCompleted = false;
    public TMP_Text Text1;
    public TMP_Text Text2;
    public TMP_Text Text3;
    public GameObject Button;
    public GameObject CollisionImage;
    public AudioSource CollisionAudio;
    public AudioSource LevelCompleteAudio;

    private float Thrust = 0.2f;
    private Rigidbody Rb;
    private Vector3 StartPosition;
    private Transform CameraTransform;

    void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        StartPosition = transform.position;
        CameraTransform = Camera.main.transform;
        UpdateLevelBasedOnScene();
    }

    void Start()
    {
        e_CoinCollection += ScoreUpdate;
        e_CoinCollection += WinPrompt;
        InitializeCollisionComponents();
        UpdateMaxScore();
    }

    void FixedUpdate()
    {
        if (ManagerScript.Instance.IsDialogueActive())
            return;
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
            Rb.AddForce(0, 0, Thrust, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Rb.AddForce(0, 0, -Thrust, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Rb.AddForce(Thrust, 0, 0, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.A))
        {
            Rb.AddForce(-Thrust, 0, 0, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.Space) && IsJumping)
        {
            Rb.AddForce(0, 5, 0, ForceMode.Impulse);
            IsJumping = false;
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

        Vector3 forward = CameraTransform.forward;
        Vector3 right = CameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 movementDirection = (forward * verticalInput + right * horizontalInput).normalized;

        Rb.AddForce(movementDirection * Thrust, ForceMode.Impulse);

        if (Input.GetKey(KeyCode.Space) && CanJump)
        {
            Rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
            CanJump = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("ResetPlane"))
        {
            CanJump = true;
            IsJumping = true;
        }

        if (collision.gameObject.CompareTag("ResetPlane"))
        {
            if (CollisionAudio != null)
            {
                CollisionAudio.Play();
            }

            ShowCollisionImageAndReset();
        }
    }

    private void ShowCollisionImageAndReset()
    {
        if (CollisionImage != null)
        {
            CollisionImage.SetActive(true);
        }

        Invoke(nameof(ResetPosition), 1.5f);
    }

    private void ResetPosition()
    {
        if (CollisionImage != null)
        {
            CollisionImage.SetActive(false);
        }

        Rb.linearVelocity = Vector3.zero;
        transform.position = StartPosition;
    }

    private void UpdateMaxScore()
    {
        GameObject[] collectibles = GameObject.FindGameObjectsWithTag("collectible");
        MaxScore = collectibles.Length;

        if (Text3 != null)
        {
            Text3.text = $"Aktualny poziom: {lv + 1}, Maksymalny wynik: {MaxScore}";
        }
    }

    private void InitializeCollisionComponents()
    {
        if (CollisionImage != null)
        {
            CollisionImage.SetActive(false);
        }

        if (CollisionAudio == null)
        {
            CollisionAudio = GetComponent<AudioSource>();
        }
    }

    private void ScoreUpdate(object o, EventArgs e)
    {
        if (Text1 != null)
        {
            Score++;
            Text1.text = "Wynik: " + Score;
            Text3.text = "Aktualny poziom: " + (lv + 1);
        }
    }

    private void WinPrompt(object o, EventArgs e)
    {
        if (LevelCompleted) return;

        if (Score >= MaxScore)
        {
            LevelCompleted = true;

            if (lv == 0)
            {
                if (Text2 != null) Text2.text = "POZIOM 1 FINITO!";
                if (Button != null) Button.SetActive(true);
                if (LevelCompleteAudio != null) LevelCompleteAudio.Play();
            }
            else if (lv == 1)
            {
                if (Text2 != null) Text2.text = "POZIOM 2 FINITO!";
                if (Button != null) Button.SetActive(true);
                if (LevelCompleteAudio != null) LevelCompleteAudio.Play();
            }
        }
    }
}
