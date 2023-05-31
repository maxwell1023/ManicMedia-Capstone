using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsGun : MonoBehaviour
{
    public Camera cam;
    public LayerMask physicsInteractableObjectMask;
    public float maxGrabDistance = 30f;

    public Transform objectHolder;
    private Rigidbody grabbedRB = null;

    private bool rotateMode = false;

    // Update is called once per frame
    void Update()
    {
        
        //Debug.DrawRay(transform.position, cam.ScreenToWorldPoint(Input.mousePosition) - transform.position, Color.blue);

        if (Input.GetMouseButtonDown(0))
        {
            GrabObject();
        }

        if (grabbedRB)
        {
            grabbedRB.MovePosition(objectHolder.position);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (rotateMode)
            {
                //Turn on rotate mode
                print("Rotate mode is OFF");
                rotateMode = false;
            }
            else
            {
                //Turn off rotate mode
                print("Rotate mode is ON");
                rotateMode = true;
            }
        }

        //If rotate mode is on this checks for keyboard inputs
        RotateObject();
    }

    private void GrabObject()
    {

        if (grabbedRB)
        {
            //Drop the object if one is already being held
            ReleaseObject();
            //grabbedRB.isKinematic = false;
            //grabbedRB = null;
        }
        else
        {
            //Pick up an object the player is pointing at with the mouse

            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, maxGrabDistance, physicsInteractableObjectMask))
            {
                print("raycast shot");
                grabbedRB = hit.collider.gameObject.GetComponent<Rigidbody>();
                if (grabbedRB)
                {
                    print("Changing object behavior");
                    grabbedRB.isKinematic = true;
                    grabbedRB.transform.rotation = Quaternion.identity;
                }
            }
        }
        
    }

    private void RotateObject()
    {

        if (rotateMode && grabbedRB)
        {
            if(Input.GetKeyDown(KeyCode.W))
            {
                grabbedRB.transform.Rotate(45, transform.rotation.y, transform.rotation.z, Space.Self);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                grabbedRB.transform.Rotate(transform.rotation.x, 45, transform.rotation.z, Space.Self);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                grabbedRB.transform.Rotate(-45, transform.rotation.y, transform.rotation.z, Space.Self);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                grabbedRB.transform.Rotate(transform.rotation.x, -45, transform.rotation.z, Space.Self);
            }

        }
    }

    //this lets go of the current object grabbed by the player
    private void ReleaseObject()
    {
        //check if an object has been released in "placeable" zone
        if (grabbedRB.gameObject.GetComponent<PipeLogic>().canStick == true)
        {
            print("placed!");
            grabbedRB.velocity = Vector3.zero;
            grabbedRB.isKinematic = false;
            grabbedRB = null;
        }
        else
        {
            grabbedRB.isKinematic = false;

            grabbedRB = null;
        }
        
    }


}
