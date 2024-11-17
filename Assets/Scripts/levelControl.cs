using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelControl : MonoBehaviour
{
    public GameObject Button;

    public void nextLevel()
    {
        if (MovementController.lv == 1)
        {
            SceneManager.LoadScene("NowiutkiPoziom", LoadSceneMode.Single);
        }
        else if (MovementController.lv == 2)
        {
            SceneManager.LoadScene("menu", LoadSceneMode.Single);
        }
    }
}
