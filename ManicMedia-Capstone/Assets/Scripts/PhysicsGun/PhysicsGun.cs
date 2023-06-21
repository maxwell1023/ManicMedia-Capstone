using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhysicsGun : MonoBehaviour
{
    public Camera cam;
    public LayerMask physicsInteractableObjectMask;
    public float maxGrabDistance = 30f;

    public Transform objectHolder;
    private Rigidbody grabbedRB = null;

    private bool rotateMode = false;
    private bool laserMode = false;

    public Slider flingSlider;
    private int sliderNum = 0;

    [SerializeField] private LineRenderer laserRender;
    [SerializeField] private LineRenderer grabRender;

    private void Start()
    {
        flingSlider.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            GrabObject();
        }

        if (Input.GetMouseButton(0))
        {
            ObjectVelocity();
        }

        if (Input.GetMouseButtonUp(0))
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

    }

    private void FixedUpdate()
    {
        //Moves the held objects rigidbody to the specificed position
        if (grabbedRB)
        {

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 newObjectPos = ray.GetPoint(objectHolder.transform.localPosition.z);

            objectHolder.transform.LookAt(cam.transform.position, Vector3.up);

            grabbedRB.position = Vector3.Lerp(grabbedRB.transform.position, newObjectPos, Time.deltaTime * 10);
            grabbedRB.rotation = Quaternion.Slerp(grabbedRB.transform.rotation, objectHolder.transform.rotation, Time.deltaTime * 10);

            grabRender.SetPosition(0, transform.position);
            grabRender.SetPosition(laserRender.positionCount - 1, grabbedRB.position);

        }
        else
        {
            grabRender.SetPosition(0, new Vector3(0,0,0));
            grabRender.SetPosition(laserRender.positionCount - 1, new Vector3(0, 0, 0));
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
                grabbedRB = hit.collider.gameObject.GetComponent<Rigidbody>();
                if (grabbedRB)
                {
                    grabbedRB.useGravity = false;

                }
            }
        }
        
    }

    //this lets go of the current object grabbed by the player
    private void ReleaseObject()
    {
        grabbedRB.useGravity = true;
        grabbedRB = null;

        objectHolder.localPosition = objectHolder.localPosition - (objectHolder.localPosition - new Vector3(0,0,8));
    }


    private void ChangeObjectDistance()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && objectHolder.transform.localPosition.z < 25)
        {
            objectHolder.transform.localPosition = Vector3.Lerp(objectHolder.transform.localPosition, objectHolder.transform.localPosition + new Vector3(0, 0, 20), Time.deltaTime * 30);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && objectHolder.transform.localPosition.z > 8)
        {
            objectHolder.transform.localPosition = Vector3.Lerp(objectHolder.transform.localPosition, objectHolder.transform.localPosition - new Vector3(0, 0, 20), Time.deltaTime * 30);
        }
    }

    private void FlingObject()
    {
        if (grabbedRB)
        {

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 newObjectPos = ray.GetPoint(objectHolder.transform.localPosition.z);


            grabbedRB.useGravity = true;
            Vector3 direction = (newObjectPos - cam.transform.position).normalized;
            grabbedRB.AddForce(direction * (sliderNum * 300), ForceMode.Impulse);
            grabbedRB = null;

            sliderNum = 0;
            flingSlider.gameObject.SetActive(false);
        }


    }

    private void ObjectVelocity()
    {
        if (grabbedRB)
        {
            sliderNum = sliderNum + 1;
            if (sliderNum >= 100)
            {
                sliderNum = 100;
            }

            flingSlider.value = sliderNum;
            flingSlider.gameObject.SetActive(true);
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
            laserRender.SetPosition(laserRender.positionCount - 1, cam.ScreenToWorldPoint(mousePos));
        }
        
    }
}
