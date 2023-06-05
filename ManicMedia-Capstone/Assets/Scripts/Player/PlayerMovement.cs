using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //editable variables
    [SerializeField]
    private float playerSpeed = 1f, aimSpeed = .5f, rotationSpeed = .8f, mainTurnSpeed = 2, jumpSpeed = 2;
    [SerializeField]
    private CinemachineVirtualCamera holdingCam;
    [SerializeField]
    private GameObject hitMarker;
    [SerializeField]
    private Rigidbody rb;

    //private variables (not seen in editor)
    public bool mainCameraOn = true;
    private bool cameraDelayed = true;
    private bool canJump = true;
    private float turnV;
    private float ySpeed;
    public int playerMelee;


    private void Start() //Intial conditions
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Confined;
        hitMarker.SetActive(false);
        mainCameraOn = true;
        cameraDelayed = true;
        canJump = true;
        //Physics.gravity.y = Physics.gravity.y * 2;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchCamera();
        }

        
        if (mainCameraOn == true)
        {

            //forces the player to stop if there are contradicting inputs
            if ((Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow)) || (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow)))
            {


            }
            else
            {
                MainMove();
            }
        }
        //switches movement type when over the shoulder
        else if (mainCameraOn == false)
        {
            AimMove();
        }
        //checks for jump input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    //JUMP
    private void Jump()
    {
        if (canJump == true)
        {
            rb.AddForce(new Vector3(0, 10, 0), ForceMode.Impulse);
            StartCoroutine(JumpDelay());
        }

    }

    //the movement used when the player is zoomed out (bird's eye camera)
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
        cameraAdjustedMovement = cameraAdjustedMovement.normalized;
        this.transform.Translate(cameraAdjustedMovement * playerSpeed * Time.deltaTime, Space.World);

        if (cameraAdjustedMovement != Vector3.zero)
        {
            transform.forward = cameraAdjustedMovement;
        }


    }

    //the movement used when the player is zoomed in (for moving objects)
    private void AimMove()
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0f;
        right.y = 0f;
        forward = forward.normalized;
        right = right.normalized;

        Vector3 totalMovement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;
        totalMovement = totalMovement.x * right + totalMovement.z * forward;
        this.transform.Translate(totalMovement * aimSpeed * Time.deltaTime, Space.World);

        float cameraAngle = Camera.main.transform.rotation.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, cameraAngle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

    }

    //switches to the other camera mode
    private void SwitchCamera()
    {
        if (cameraDelayed == true)
        {
            //switches from the bird's eye to over the shoulder
            if (mainCameraOn == true)
            {
                holdingCam.Priority = 2;
                Cursor.lockState = CursorLockMode.Locked;
                hitMarker.SetActive(true);
            }
            //switches from the over the shoulder to bird's eye
            else
            {
                holdingCam.Priority = 0;
                Cursor.lockState = CursorLockMode.Confined;
                hitMarker.SetActive(false);
            }

            mainCameraOn = !mainCameraOn;
            StartCoroutine(CameraDelay());
        }
    }

    // The delay between when the player can switch camera modes (to decrease misinputs)
    private IEnumerator CameraDelay()
    {
        cameraDelayed = false;
        yield return new WaitForSeconds(1f);
        cameraDelayed = true;
    }
    // The delay between when the player can jump (prevent flying) //should be changed to use a ground checker!!
    private IEnumerator JumpDelay()
    {
        canJump = false;
        yield return new WaitForSeconds(1.75f);
        canJump = true;
    }
}
