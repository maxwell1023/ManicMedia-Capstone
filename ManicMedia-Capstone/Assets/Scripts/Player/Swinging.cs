using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Swinging : MonoBehaviour
{
    public LineRenderer swingLR;
    public Transform firePoint, cam, player, gun;
    public LayerMask grappleable;
    public bool isSwinging;

    [SerializeField]
    private float maxSwingDistance = 25;
    private Vector3 attachPoint, currentGrapplePosition;
    private SpringJoint joint;

    [SerializeField]
    private Transform playerOrientation;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private float horizThrustForce, forwardThrustForce, pullSpeed, pullTime;

    private float tempXAngle;

    private bool keepMoving, GOTCanRun;
    private Quaternion defaultGunRotation, attachRotation;
    private void Start()
    {
        defaultGunRotation = gun.transform.localRotation;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) 
        { 
            Swing(); isSwinging = true; 
            rb.drag = 0; 
 
        }


        if (Input.GetKeyUp(KeyCode.E)) { LetGo(); isSwinging = false; GOTCanRun = true; }

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
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxSwingDistance, grappleable))
        {
            attachPoint = hit.point;

            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = attachPoint;

            float distanceFromAttach = Vector3.Distance(player.position, attachPoint);

            joint.maxDistance = distanceFromAttach * 0.5f;   //CHANGE TO SHRINK OVER TIME
            joint.minDistance = distanceFromAttach * 0.25f;

            joint.spring = 4f;      //maybe not hard coded?
            joint.damper = 6f;
            joint.massScale = 4f;

            swingLR.positionCount = 2;
            currentGrapplePosition = firePoint.position;

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

    private void Grapple()
    {
        Vector3 DirectionToPoint = attachPoint - transform.position;
        rb.AddForce(DirectionToPoint.normalized * pullSpeed * Time.deltaTime);

        float distanceFromAttach = Vector3.Distance(player.position, attachPoint);
        joint.maxDistance = distanceFromAttach * 0.8f;   
        joint.minDistance = distanceFromAttach * 0.5f;
    }

  

}
