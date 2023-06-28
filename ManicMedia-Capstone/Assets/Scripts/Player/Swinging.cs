using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Swinging : MonoBehaviour
{
    [SerializeField]
    private float burnoutSpeed, regenSpeed, maxGrappleCharge, grappleCharge, initialCost;


    public LineRenderer swingLR;
    public Transform firePoint, cam, player, gun;
    public LayerMask grappleable, notGrappleable;
    public bool isSwinging;

    [SerializeField]
    private float maxSwingDistance = 25;
    private Vector3 attachPoint, currentGrapplePosition;
    private SpringJoint joint;

    public Slider grappleSlider;
    [SerializeField]
    private Transform playerOrientation;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private float forwardThrustForce, horizThrustForce, pullSpring, pullScale, pullDamp;

    private float tempXAngle;

    public bool canGrapple;
    private Quaternion defaultGunRotation, attachRotation;
    private void Start()
    {
        grappleSlider.maxValue = maxGrappleCharge;
        grappleCharge = maxGrappleCharge;
        defaultGunRotation = gun.transform.localRotation;
        swingLR.enabled = false;
    }

    private void Update()
    {
        grappleSlider.value = grappleCharge;
        if (Input.GetKeyDown(KeyCode.Mouse1)) 
        { 
            if(this.gameObject.GetComponent<PhysicsGun>().isLasering == false && this.gameObject.GetComponent<PhysicsGun>().isHolding == false && canGrapple)
            {
                grappleCharge -= initialCost;
                if(grappleCharge <= 0) {grappleCharge = 10;}
                Swing();
                swingLR.enabled = true;
                if (isSwinging == true)
                {
                    rb.drag = 0;
                }
            }
  
        }

        if(this.gameObject.GetComponent<PlayerMovement>().isGrounded == true && isSwinging == false)
        {
            grappleCharge = Mathf.Lerp(grappleCharge, maxGrappleCharge, Time.deltaTime * regenSpeed);
        }
        if(isSwinging == true)
        {
            grappleCharge -= Time.deltaTime * burnoutSpeed;
        }
        if(grappleCharge <= 0)
        {
            LetGo(); 
            isSwinging = false;
            canGrapple = false;
        }

        if (canGrapple == false)
        {
            if (grappleCharge < maxGrappleCharge * .95f)// * (99/100)))
            {
                canGrapple = false;
            }
            else
            {
                canGrapple = true;
                grappleCharge = maxGrappleCharge;
            }


        }

        if (Input.GetKeyUp(KeyCode.Mouse1) || this.gameObject.GetComponent<PhysicsGun>().isHolding || this.gameObject.GetComponent<PlayerHealth>().dead) { LetGo(); isSwinging = false; }

        if(joint!= null)
        { DirectionInput();
            // gun.LookAt(attachPoint, Vector3.up);
          /*  Vector3 look = attachPoint - gun.position;
            look.z = 0;

            Quaternion q = Quaternion.LookRotation(look);
            if (Quaternion.Angle(q, defaultGunRotation) <= 60f)
            {
                attachRotation = q;

                gun.transform.localRotation = Quaternion.Slerp(gun.transform.localRotation, attachRotation, Time.deltaTime * 80.0f);
            } */
        }
       /* else
        {
            gun.transform.localRotation = defaultGunRotation;
        }

        
            Quaternion gunRotation = Quaternion.Euler(gun.transform.eulerAngles.x, Mathf.Clamp(gun.transform.eulerAngles.y, 0, 30), gun.transform.eulerAngles.z);
           gun.transform.localRotation = gunRotation; */

      




    }

    private void LateUpdate()
    {
        DrawRope();
    }
    private void Swing()
    {
        
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxSwingDistance, notGrappleable))
        {

        }
        else if (Physics.Raycast(cam.position, cam.forward, out hit, maxSwingDistance, grappleable))
        {

            attachPoint = hit.point;
            isSwinging = true;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = attachPoint;

            float distanceFromAttach = Vector3.Distance(player.position, attachPoint);

            joint.maxDistance = distanceFromAttach * 0.5f;   //CHANGE TO SHRINK OVER TIME
            joint.minDistance = distanceFromAttach * 0.25f;

            joint.spring = pullSpring;      //maybe not hard coded?
            joint.damper =  pullDamp;
            joint.massScale = pullScale;

            swingLR.positionCount = 2;
            currentGrapplePosition = firePoint.position;

        }
        else
        {
            isSwinging = false;
        }
    }

    private void LetGo()
    {
        swingLR.positionCount = 0;
        Destroy(joint);
    }

    private void DrawRope()
    {
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, attachPoint, Time.deltaTime * 800f);

        swingLR.SetPosition(0, firePoint.position);
        swingLR.SetPosition(1, currentGrapplePosition);

    }
    private void DirectionInput()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) { rb.AddForce(playerOrientation.right * horizThrustForce * Time.deltaTime); }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) { rb.AddForce( -playerOrientation.right * horizThrustForce * Time.deltaTime); }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) { rb.AddForce(playerOrientation.forward * forwardThrustForce * Time.deltaTime); }

    }

  

}
