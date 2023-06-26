using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    [SerializeField]
    private Transform gearCenter;
   

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(gearCenter.transform.position, Vector3.up, 45 * Time.deltaTime);
    }
}
