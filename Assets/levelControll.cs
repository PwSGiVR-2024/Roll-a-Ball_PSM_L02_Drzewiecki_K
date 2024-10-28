using UnityEngine;
using UnityEngine.SceneManagement;

public class levelControll : MonoBehaviour
{
    public GameObject Button;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void nextLevel()
    {
        SceneManager.LoadScene("NowiutkiPoziom", LoadSceneMode.Single);
    }
}
