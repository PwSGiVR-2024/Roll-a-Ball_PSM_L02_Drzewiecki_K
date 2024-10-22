using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
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
        //SceneManager.LoadScene("menu", LoadSceneMode.Single);
    }

    public void exitGame()
    {

    }
}
