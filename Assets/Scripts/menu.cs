using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    void Start()
    {
        
    }
    void Update()
    {
        
    }

    public GameObject OptionsPanel;
    public bool cur;
    public void startGame()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
    public void showOptions(bool isActive)
    {
        OptionsPanel.SetActive(isActive);
        cur = isActive;
    }
    public void goBack()
    {
        cur = false;
        OptionsPanel.SetActive(cur);
    }
    public void exitGame()
    {

    }
}
