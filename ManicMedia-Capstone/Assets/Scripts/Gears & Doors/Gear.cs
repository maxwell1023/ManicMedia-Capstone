using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    [SerializeField]
    private Transform gearCenter;
   

    // This script controlls the gear movement when not attached to a door
    void Update()
    {
        transform.RotateAround(gearCenter.transform.position, Vector3.up, 45 * Time.deltaTime);
    }
}
