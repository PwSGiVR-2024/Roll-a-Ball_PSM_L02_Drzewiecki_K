using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trampolina : MonoBehaviour
{
    public int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //Upon collision with another GameObject, this GameObject will reverse direction
    void OnCollisionStay(Collision collision)
    {
        if (i <= 50)
        {
            transform.position = collision.transform.position;
            i++;
        }
    }
}
