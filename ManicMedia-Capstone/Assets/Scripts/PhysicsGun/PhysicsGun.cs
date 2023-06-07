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
    private bool laserMode = false;

    [SerializeField] private LineRenderer laserRender;

    // Update is called once per frame
    void Update()
    {
        
        //Debug.DrawRay(transform.position, cam.ScreenToWorldPoint(Input.mousePosition) - transform.position, Color.blue);

        if (Input.GetMouseButtonDown(0))
        {
            GrabObject();
        }

        if (Input.GetMouseButtonDown(1))
        {
            FlingObject();
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            ChangeObjectDistance();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(laserMode)
            {
                laserRender.enabled = false;
                laserMode = false;
            }
            else
            {
                laserRender.enabled = true;
                laserMode = true;
            }
        }

        

        /*
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
        */



        //If rotate mode is on this checks for keyboard inputs
        //RotateObject();
    }

    private void FixedUpdate()
    {
        //Moves the held objects rigidbody to the specificed position
        if (grabbedRB)
        {

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 newObjectPos = ray.GetPoint(objectHolder.transform.localPosition.z);

            objectHolder.transform.LookAt(newObjectPos, Vector3.up);
            grabbedRB.position = Vector3.Lerp(grabbedRB.transform.position, newObjectPos, Time.deltaTime * 10);
            grabbedRB.rotation = Quaternion.Slerp(grabbedRB.transform.rotation, objectHolder.transform.rotation, Time.deltaTime * 10);

        }

        LaserMode();
    }

    private void GrabObject()
    {

        if (grabbedRB)
        {
            //Drop the object if one is already being held
            ReleaseObject();
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
                    grabbedRB.useGravity = false;
                    //grabbedRB.isKinematic = true;
                    //grabbedRB.transform.rotation = Quaternion.identity;

                }
            }
        }
        
    }

    /*
    private void RotateObject()
    {

        if (rotateMode && grabbedRB)
        {
            if(Input.GetKeyDown(KeyCode.W))
            {
                Quaternion rotation = Quaternion.Euler(grabbedRB.transform.localEulerAngles + new Vector3(45, 0, 0));
                grabbedRB.MoveRotation(rotation);
                //grabbedRB.transform.Rotate(45, transform.rotation.y, transform.rotation.z, Space.Self);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                Quaternion rotation = Quaternion.Euler(grabbedRB.transform.localEulerAngles + new Vector3(0, 45, 0));
                grabbedRB.MoveRotation(rotation);
                //grabbedRB.transform.Rotate(transform.rotation.x, 45, transform.rotation.z, Space.Self);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Quaternion rotation = Quaternion.Euler(grabbedRB.transform.localEulerAngles + new Vector3(-45, 0, 0));
                grabbedRB.MoveRotation(rotation);
                //grabbedRB.transform.Rotate(-45, transform.rotation.y, transform.rotation.z, Space.Self);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                Quaternion rotation = Quaternion.Euler(grabbedRB.transform.localEulerAngles + new Vector3(0, -45, 0));
                grabbedRB.MoveRotation(rotation);
                //grabbedRB.transform.Rotate(transform.rotation.x, -45, transform.rotation.z, Space.Self);
            }

        }
    }
    */

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
            //grabbedRB.isKinematic = false;
            grabbedRB.useGravity = true;
            grabbedRB = null;
        }
        
    }


    private void ChangeObjectDistance()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && objectHolder.transform.localPosition.z < 25)
        {
            objectHolder.transform.localPosition = Vector3.Lerp(objectHolder.transform.localPosition, objectHolder.transform.localPosition + new Vector3(0, 0, 20), Time.deltaTime * 10);

            //objectHolder.transform.localPosition = objectHolder.transform.localPosition + new Vector3(0, 0, 0.5f);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && objectHolder.transform.localPosition.z > 7)
        {
            objectHolder.transform.localPosition = Vector3.Lerp(objectHolder.transform.localPosition, objectHolder.transform.localPosition - new Vector3(0, 0, 20), Time.deltaTime * 10);
        }
    }

    private void FlingObject()
    {
        if (grabbedRB)
        {

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 newObjectPos = ray.GetPoint(objectHolder.transform.localPosition.z);
            

            grabbedRB.useGravity = true;
            //Vector3 direction = objectHolder.transform.position - grabbedRB.transform.position;
            Vector3 direction = (newObjectPos - cam.transform.position).normalized;
            grabbedRB.AddForce(direction * 100, ForceMode.Impulse);
            grabbedRB = null;
        }
    }


    private void LaserMode()
    {
        if (laserMode)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, maxGrabDistance, physicsInteractableObjectMask))
            {

            }

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 100f;

            laserRender.SetPosition(0, transform.position);
            laserRender.SetPosition(1, cam.ScreenToWorldPoint(mousePos));
        }
        
    }
}
