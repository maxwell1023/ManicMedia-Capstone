using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 1f, aimSpeed = .5f, rotationSpeed = .8f, mainTurnSpeed = 2;
    [SerializeField]
    private CinemachineVirtualCamera holdingCam;
    [SerializeField]
    private GameObject hitMarker;
   

    private bool mainCameraOn = true;
    private bool cameraDelayed = true;
    float turnV;
    private void Start()
    {
        hitMarker.SetActive(false);
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
            
            if((Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow)) || (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow)))
            {
                print("STOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPSTOP");

            }
            else
            {
                MainMove();
            }
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

        if (cameraAdjustedMovement != Vector3.zero)
        {
            transform.forward = cameraAdjustedMovement;
        }
       

    }

    private void AimMove()
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0f;
        right.y = 0f;
        forward = forward.normalized;
        right = right.normalized;

        Vector3 totalMovement = new Vector3 (Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        totalMovement = totalMovement.x * right + totalMovement.z * forward;
        this.transform.Translate(totalMovement * aimSpeed * Time.deltaTime, Space.World);

        float cameraAngle = Camera.main.transform.rotation.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0,cameraAngle,0);    
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

    }

   private void SwitchCamera()
    {
        if(cameraDelayed == true) 
        {
            if (mainCameraOn == true)
            {
                holdingCam.Priority = 2;
                hitMarker.SetActive(true);
            }
            else
            {
                holdingCam.Priority = 0;
                hitMarker.SetActive(false);
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
