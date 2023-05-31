using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeLogic : MonoBehaviour
{
    
    public bool canStick = false; //bool accessed by the physics gun script
    

    void Start()
    {
        canStick = false; 
    }

 
    //checks if pipe object enters a "placeable" zone
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PipeZone")
        {
            canStick = true;
        }
    }

    //checks if pipe object exits a "placeable" zone
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "PipeZone")
        {
            canStick = false;
        }
    }
}
