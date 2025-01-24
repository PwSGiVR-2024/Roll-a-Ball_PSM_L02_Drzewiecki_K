using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System;

public class ManagerScript : MonoBehaviour
{
    public static ManagerScript Instance;

    public static int lv = 0;

    public GameObject dialogueBox;
    public TMP_Text dialogueText;
    public Camera mainCamera;
    public Camera secondaryCamera;
    public AudioSource textAudio;

    private string[] _dialogueMessages = {
        "Witaj w grze WiedŸmin 5! W najnowszej ods³onie tej uwielbianej serii czekaj¹ na ciebie niesamowite wyzwania platformowe, a na koñcu zawalczysz z bossem!",
        "Poruszaj siê klawiszami W, A, S, D, skacz Spacj¹ - pamiêtaj, mo¿esz odbiæ siê od œciany lub obiektu, je¿eli wystarczaj¹co szybko wciœniesz ponownie spacjê!",
        "Jakieœ pytania? Brak? W takim razie - mi³ej zabawy! Do zobaczenia w Raymanie 4!"
    };
    private int _currentMessageIndex = 0;
    private bool _isDialogueActive = true;

    public int score = 0;
    public int maxScore;
    public TMP_Text Text1;
    public TMP_Text Text2;
    public TMP_Text Text3;
    public GameObject Button;

    public GameObject CollisionImage;
    public AudioSource CollisionAudio;
    public AudioSource LevelCompleteAudio;

    private bool _levelCompleted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // singleton
        }

        UpdateLevelBasedOnScene();

        if (lv == 0)
        {
            if (secondaryCamera != null) secondaryCamera.gameObject.SetActive(true);
            if (mainCamera != null) mainCamera.gameObject.SetActive(false);
        }
        else
        {
            if (dialogueBox != null) dialogueBox.SetActive(false);
            if (secondaryCamera != null) secondaryCamera.gameObject.SetActive(false);
            if (mainCamera != null) mainCamera.gameObject.SetActive(true);
            _isDialogueActive = false;
        }
    }

    private void Start()
    {
        if (lv == 0)
        {
            ShowDialogue();
        }
        else
        {
            _isDialogueActive = false;
            CalculateScore();
            UpdateUI();
        }

        Collectible.e_CoinCollection += ScoreUpdate;
        Collectible.e_CoinCollection += WinPrompt;

        InitializeCollisionComponents();
    }

    private void Update()
    {
        if (_isDialogueActive && Input.GetMouseButtonDown(0))
        {
            NextDialogueMessage();
        }
    }
    private void ShowDialogue()
    {
        if (dialogueBox != null)
        {
            dialogueBox.SetActive(true);
            StartCoroutine(TypeText(_dialogueMessages[_currentMessageIndex]));
        }
    }
    private IEnumerator TypeText(string message)
    {
        dialogueText.text = "";

        if (textAudio != null)
        {
            textAudio.Play();
        }

        foreach (char letter in message)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }
    private void NextDialogueMessage()
    {
        if (dialogueText.text != _dialogueMessages[_currentMessageIndex])
        {
            StopAllCoroutines();
            dialogueText.text = _dialogueMessages[_currentMessageIndex];
        }
        else
        {
            _currentMessageIndex++;
            if (_currentMessageIndex < _dialogueMessages.Length)
            {
                StartCoroutine(TypeText(_dialogueMessages[_currentMessageIndex]));
            }
            else
            {
                EndDialogue();
            }
        }
    }

    private void EndDialogue()
    {
        _isDialogueActive = false;
        if (dialogueBox != null) dialogueBox.SetActive(false);

        if (secondaryCamera != null) secondaryCamera.gameObject.SetActive(false);
        if (mainCamera != null) mainCamera.gameObject.SetActive(true);

        CalculateScore();
        UpdateUI();
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
        else if (sceneName == "Menu")
        {
            lv = 0;
        }
        else
        {
            lv = 0;
        }
    }

    public void NextLevel()
    {
        if (lv == 0)
        {
            SceneManager.LoadScene("NowiutkiPoziom", LoadSceneMode.Single);
        }
        else if (lv == 1)
        {
            SceneManager.LoadScene("FinalBoss", LoadSceneMode.Single);
        }
        else if (lv == 2)
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }
    }

    public void CalculateScore()
    {
        GameObject[] collectibles = GameObject.FindGameObjectsWithTag("collectible");
        maxScore = collectibles.Length;
    }

    public void ScoreUpdate(object o, EventArgs e)
    {
        score++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (Text1 != null)
        {
            Text1.text = "Wynik: " + score;
        }
        if (Text3 != null)
        {
            Text3.text = $"Aktualny poziom: {lv + 1}, Maksymalny wynik: {maxScore}";
        }
    }

    public void WinPrompt(object o, EventArgs e)
    {
        if (_levelCompleted) return;

        if (score >= maxScore)
        {
            _levelCompleted = true;

            if (Text2 != null)
                Text2.text = $"POZIOM {lv + 1} FINITO!";
            if (Button != null)
                Button.SetActive(true);
            if (LevelCompleteAudio != null)
                LevelCompleteAudio.Play();
        }
    }

    public void HandlePlayerFall()
    {
        if (CollisionAudio != null)
        {
            CollisionAudio.Play();
        }

        if (CollisionImage != null)
        {
            CollisionImage.SetActive(true);
            Invoke(nameof(HideCollisionImage), 1.5f);
        }
    }

    private void HideCollisionImage()
    {
        if (CollisionImage != null)
        {
            CollisionImage.SetActive(false);
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

    public bool IsDialogueActive()
    {
        return _isDialogueActive;
    }

    private void OnDestroy()
    {
        Collectible.e_CoinCollection -= ScoreUpdate;
        Collectible.e_CoinCollection -= WinPrompt;
    }
}
