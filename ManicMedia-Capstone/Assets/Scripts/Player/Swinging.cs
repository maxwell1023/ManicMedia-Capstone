using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swinging : MonoBehaviour
{
    public LineRenderer swingLR;
    public Transform firePoint, cam, player;
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
    private float horizThrustForce, forwardThrustForce, extendCableSpeed;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) { Swing(); isSwinging = true;}
        if (Input.GetKeyUp(KeyCode.E)) { LetGo(); isSwinging = false; }

        if(joint!= null) { directionInput(); }
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

            joint.maxDistance = distanceFromAttach * 0.8f;   //CHANGE TO SHRINK OVER TIME
            joint.minDistance = distanceFromAttach * 0.25f;

            joint.spring = 4.5f;      //maybe not hard coded?
            joint.damper = 7f;
            joint.massScale = 4.5f;

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
    private void directionInput()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) { rb.AddForce(playerOrientation.right * horizThrustForce * Time.deltaTime); }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) { rb.AddForce( -playerOrientation.right * horizThrustForce * Time.deltaTime); }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) { rb.AddForce(playerOrientation.forward * forwardThrustForce * Time.deltaTime); }

        if(Input.GetKey(KeyCode.Space)) 
        {
            Vector3 DirectionToPoint = attachPoint - transform.position;
            rb.AddForce(DirectionToPoint.normalized * forwardThrustForce * Time.deltaTime);

            float distanceFromAttach = Vector3.Distance(player.position, attachPoint);
            joint.maxDistance = distanceFromAttach * 0.8f;   //CHANGE TO SHRINK OVER TIME
            joint.minDistance = distanceFromAttach * 0.25f;



        }
        if (Input.GetKey(KeyCode.S))
        {

        }
    }

}
