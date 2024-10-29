using UnityEngine;
using UnityEngine.SceneManagement;

public class levelControll : MonoBehaviour
{
    public GameObject Button;
    void Start()
    {
        
    }
    void Update()
    {
        
    }

    public void nextLevel()
    {
        SceneManager.LoadScene("NowiutkiPoziom", LoadSceneMode.Single);
    }

    public void endGame()
    {
        SceneManager.LoadScene("menu", LoadSceneMode.Single);
    }
}
