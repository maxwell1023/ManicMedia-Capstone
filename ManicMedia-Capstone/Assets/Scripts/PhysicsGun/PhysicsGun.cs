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



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //print(cam.ScreenToWorldPoint(Input.mousePosition));
        
        //Debug.DrawRay(transform.position, cam.ScreenToWorldPoint(Input.mousePosition) - transform.position, Color.blue);

        if (Input.GetKeyDown(KeyCode.E))
        {
            GrabObject();
        }

        if (grabbedRB)
        {
            grabbedRB.MovePosition(objectHolder.position);
        }

        RotateObject();
    }

    private void FixedUpdate()
    {
        
    }

    private void GrabObject()
    {

        if (grabbedRB)
        {
            //Drop the object if one is already being held
            grabbedRB.isKinematic = false;
            grabbedRB = null;
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
                }
            }
        }
        
    }

    private void RotateObject()
    {
        if(Input.GetKeyDown(KeyCode.R))
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


        if (rotateMode)
        {
            if(Input.GetKeyDown(KeyCode.W))
            {
                transform.Rotate(90, transform.rotation.y, transform.rotation.z, Space.World);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                transform.Rotate(transform.rotation.x, 90, transform.rotation.z, Space.World);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                transform.Rotate(-90, transform.rotation.y, transform.rotation.z, Space.World);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                transform.Rotate(transform.rotation.x, -90, transform.rotation.z, Space.World);
            }

        }
    }


}
