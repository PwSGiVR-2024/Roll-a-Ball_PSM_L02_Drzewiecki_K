using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelControl : MonoBehaviour
{
    public GameObject Button;

    public void nextLevel()
    {
        if (movementController.lv == 1)
        {
            SceneManager.LoadScene("NowiutkiPoziom", LoadSceneMode.Single);
        }
        else if (movementController.lv == 2)
        {
            SceneManager.LoadScene("menu", LoadSceneMode.Single);
        }
    }
}
