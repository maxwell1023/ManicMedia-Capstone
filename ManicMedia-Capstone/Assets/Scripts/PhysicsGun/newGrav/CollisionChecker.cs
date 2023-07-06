using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    public bool isColliding = false;
    [SerializeField]
    private int collidersHit = 0;

    private void Update()
    {
        if (collidersHit == 0)
        {
            isColliding = false;
        }

        else
        {
            isColliding = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        collidersHit++;
    }
    private void OnCollisionExit(Collision collision)
    {
        collidersHit--;
    }
}
