using UnityEngine;

public class ManagerScript : MonoBehaviour
{
    GameObject[] collectibles;
    public int maxScore;

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
}