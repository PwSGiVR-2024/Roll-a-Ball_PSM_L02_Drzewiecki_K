using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            Time.timeScale += 0.2f;
        }

        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            Time.timeScale -= 0.2f;
        }

    }
}
