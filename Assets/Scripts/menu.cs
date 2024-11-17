using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject OptionsPanel;
    public bool cur;
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
    public void ShowOptions(bool isActive)
    {
        OptionsPanel.SetActive(isActive);
        cur = isActive;
    }
    public void GoBack()
    {
        cur = false;
        OptionsPanel.SetActive(cur);
    }
    public void ExitGame()
    {
        Application.Quit(0);
    }
}
