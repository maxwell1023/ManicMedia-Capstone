using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 1f, aimSpeed = .5f;
    [SerializeField]
    private CinemachineVirtualCamera holdingCam;

    private Transform cameraTransform;

    private bool mainCameraOn = true;
    private bool cameraDelayed = true;

    private void Start()
    {
        mainCameraOn = true;
        cameraDelayed = true;
    }

   private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            SwitchCamera();
        }

        if(mainCameraOn == true)
        {
            MainMove();
        }
        else if (mainCameraOn == false) 
        {
            AimMove();
        }
    }


   private void MainMove()
    {
        float playerVertical = Input.GetAxis("Vertical");
        float playerHorizontal = Input.GetAxis("Horizontal");

       

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0f;
        right.y = 0f;
        forward = forward.normalized;
        right = right.normalized;

        Vector3 forwardFromCam = playerVertical * forward;
        Vector3 rightFromCam = playerHorizontal * right;

        Vector3 cameraAdjustedMovement = forwardFromCam + rightFromCam;
        this.transform.Translate(cameraAdjustedMovement * playerSpeed*Time.deltaTime, Space.World);

    }

    private void AimMove()
    {
        Vector3 totalMovement = new Vector3 (Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        this.transform.Translate(totalMovement * aimSpeed * Time.deltaTime, Space.World);
    }

   private void SwitchCamera()
    {
        if(cameraDelayed == true) 
        {
            if (mainCameraOn == true)
            {
                holdingCam.Priority = 2;
            }
            else
            {
                holdingCam.Priority = 0;
            }

            mainCameraOn = !mainCameraOn;
            StartCoroutine(CameraDelay());
        }
    }

    private IEnumerator CameraDelay()
    {
        cameraDelayed = false;
        yield return new WaitForSeconds(1f);
        cameraDelayed = true;
    }
}
