using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
   
    [SerializeField]
    private Animation doorAnim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
