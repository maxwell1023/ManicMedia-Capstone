using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
   
    [SerializeField]
    private Animator doorAnim;
    // script for animating the winged doors

    public void OpenDoor() 
    {
        doorAnim.Play("Door Open");
       // doorAnimationControl.SetTrigger("OpenDoor");
    }
    public void CloseDoor() 
    {
        doorAnim.Play("Door Close");
        // doorAnimationControl.SetTrigger("CloseDoor");
    }
}
