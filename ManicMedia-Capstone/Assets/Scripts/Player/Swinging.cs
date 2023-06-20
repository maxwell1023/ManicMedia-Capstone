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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) { Swing(); isSwinging = true;}
        if (Input.GetKeyUp(KeyCode.E)) { LetGo(); isSwinging = false; }
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

}
