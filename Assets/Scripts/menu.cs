using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject OptionsPanel;
    private bool _cur;

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    public void ShowOptions(bool isActive)
    {
        OptionsPanel.SetActive(isActive);
        _cur = isActive;
    }

    public void GoBack()
    {
        _cur = false;
        OptionsPanel.SetActive(_cur);
    }

    public void ExitGame()
    {
        Application.Quit(0);
    }
}
