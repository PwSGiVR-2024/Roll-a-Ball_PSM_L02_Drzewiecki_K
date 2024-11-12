using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject brickPrefab;

    [SerializeField]
    int rows;
    [SerializeField]
    int columns;

    [SerializeField]
    Vector3 wallCorner;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GameObject go = Instantiate(brickPrefab, wallCorner + new Vector3(j * 1.5f + (i%2), i + 0.5f, 0), Quaternion.identity, transform);
                go.GetComponent<Rigidbody>().Sleep();
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
