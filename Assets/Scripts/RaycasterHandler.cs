using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycasterHandler : MonoBehaviour
{
    [SerializeField]
    LayerMask layerMask;

    [SerializeField]
    Color color;

    [SerializeField]
    float hitDistance;

    [SerializeField]
    bool testRaycasts;

    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer));

    }

    // Update is called once per frame
    void Update()
    {
        if (!testRaycasts) return;

        Ray ray = new Ray(transform.position, Vector3.down);
        
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 30, layerMask))
        {
            transform.localScale = Vector3.one * 0.2f;
            hitDistance = hit.distance;
        }

        Debug.DrawRay(ray.origin, ray.direction * hit.distance, color);
    }
}
