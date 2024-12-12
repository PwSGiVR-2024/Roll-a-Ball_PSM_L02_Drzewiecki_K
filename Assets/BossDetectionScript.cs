using System;
using UnityEngine;

public class BossDetectionScript : MonoBehaviour
{
    public static EventHandler<Collision> e_Collision;

    private void OnCollisionEnter(Collision collision)
    {
        e_Collision?.Invoke(this, collision);
    }
}
