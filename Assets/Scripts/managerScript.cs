using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class ManagerScript : MonoBehaviour
{
    public int maxScore;
    public GameObject buttonNextLevel;
    public static ManagerScript Instance;

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

        if (MovementController.lv == 0)
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
        if (MovementController.lv == 0)
        {
            ShowDialogue();
        }
    }

    private void Update()
    {
        if (_isDialogueActive && Input.GetMouseButtonDown(0))
        {
            NextDialogueMessage();
        }
    }

    public void CalculateScore()
    {
        GameObject[] collectibles = GameObject.FindGameObjectsWithTag("collectible");
        maxScore = collectibles.Length;
    }

    public int GetMaxScore()
    {
        return maxScore;
    }

    public void NextLevel()
    {
        if (MovementController.lv == 0)
        {
            SceneManager.LoadScene("NowiutkiPoziom", LoadSceneMode.Single);
        }
        else if (MovementController.lv == 1)
        {
            SceneManager.LoadScene("FinalBoss", LoadSceneMode.Single);
        }
        else if (MovementController.lv == 2)
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }
    }

    public bool IsDialogueActive()
    {
        return _isDialogueActive;
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
    }
}
