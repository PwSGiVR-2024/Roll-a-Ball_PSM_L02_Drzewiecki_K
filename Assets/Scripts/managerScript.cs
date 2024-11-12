using UnityEngine;

public class managerScript : MonoBehaviour
{
    GameObject[] collectibles;
    public int maxScore;

    private void Start()
    {
        collectibles = GameObject.FindGameObjectsWithTag("collectible");
        Score();
    }
    void Score()
    {
        for (int i =0; i < collectibles.Length; i++)
        {
            maxScore++;
        }
    }

}
