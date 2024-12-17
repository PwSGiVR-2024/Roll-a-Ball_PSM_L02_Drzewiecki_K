using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerScript : MonoBehaviour
{
    public int maxScore;
    public GameObject ButtonNextLevel;
    public static ManagerScript Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);    // singleton
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
}
