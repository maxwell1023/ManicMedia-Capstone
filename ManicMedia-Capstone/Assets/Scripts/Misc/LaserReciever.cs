using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserReciever : MonoBehaviour
{
    // Start is called before the first frame update
    private bool hasDroppedGear;

    [SerializeField]
    private GameObject gearToSpawn;
    [SerializeField]
    private Transform gearDropper;
    public void DropGear() //drops the gear
    {

        if (!hasDroppedGear)
        {
            Instantiate(gearToSpawn, gearDropper.position, Quaternion.Euler(90,0,0));
            hasDroppedGear = true;
        }

    }
}
