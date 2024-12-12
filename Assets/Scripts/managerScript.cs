using UnityEngine;
using UnityEngine.SceneManagement;
public class ManagerScript : MonoBehaviour
{
    GameObject[] collectibles;
    public int maxScore;
    public GameObject ButtonNextLevel;

    private void Awake()
    {
        collectibles = GameObject.FindGameObjectsWithTag("collectible");
        Score();
    }
    void Score()
    {
        for (int i = 0; i < collectibles.Length; i++)
        {
            maxScore++;
        }
    }
    public int GetMaxScore()
    {
        return maxScore;
    }

    public void nextLevel()
    {
        if (MovementController.lv == 1)
        {
            SceneManager.LoadScene("NowiutkiPoziom", LoadSceneMode.Single);
        }
        else if (MovementController.lv == 2)
        {
            SceneManager.LoadScene("FinalBoss", LoadSceneMode.Single);
        }
        else if (MovementController.lv == 0)
        {
            SceneManager.LoadScene("menu", LoadSceneMode.Single);
        }
    }

}