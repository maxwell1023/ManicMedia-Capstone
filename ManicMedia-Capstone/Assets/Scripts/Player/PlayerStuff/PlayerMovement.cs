using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
//using System.Runtime.CompilerServices;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
//using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed, groundFriction, playerHeight, jumpForce, jumpCooldown, airMultiplier, fallMultiplier;

    [SerializeField]
    private GameObject dash0, dash1, dash2;

    [SerializeField] private GameObject groundChecker;

    [SerializeField]
    private LayerMask groundLayer;

    public bool isGrounded;
    private bool jumpReady;


    [SerializeField]
    private Transform playerOrientation;

    [SerializeField] private AudioManager playerAudio;

    private float horizInput;
    private float vertInput;

    private Vector3 moveDirection;

    private Rigidbody rb;
    private bool isDashing, footStepAlreadyStarted;
    private int dashesLeft;
    private float timeSinceDash;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        jumpReady = true;
        dashesLeft = 2;

    }

    // Update is called once per frame
    void Update()
    {
        //PlaySteps(horizInput * horizInput > 0 || vertInput * vertInput > 0);

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A)) && this.gameObject.GetComponent<IntroManager>().dialogueReached == 2)
        {
            Invoke("NextTutorial", 1.5f);
        }

        if (isGrounded == false)
        {
            print("Air Born!!!");
        }
        if (dashesLeft <= 0)
        {
            DashEmpty();
        }
        if (dashesLeft >= 2)
        {
            DashFull();
        }
        if (dashesLeft < 2 && dashesLeft > 0)
        {
            DashHalf();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashesLeft>0 && GetComponent<Swinging>().isSwinging == false && isDashing == false)
        {
            StartCoroutine(Dash());
        }

        if (isDashing == false)
        {
            isGrounded = Physics.CheckSphere(groundChecker.transform.position, .4f, groundLayer);

            MovementInput();
            SpeedLimiter();

            if(dashesLeft < 2)
            {
                timeSinceDash += Time.deltaTime;
            }
            if(timeSinceDash >= .9)
            {
                dashesLeft++;
                timeSinceDash = 0;
            }
        }

        if(rb.velocity.y < 0 && GetComponent<Swinging>().isSwinging == false) //&& get isSwinging from script
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        if (isGrounded) 
        {
            rb.drag = groundFriction;
        }

        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }
    private void MovementInput()
    {
        horizInput = Input.GetAxisRaw("Horizontal");
        vertInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(KeyCode.Space) && jumpReady && isGrounded)
        {
            jumpReady = false;

            Jump();

            Invoke(nameof(JumpRest), jumpCooldown);

        }
    }

    private void PlayerMove()
    {
        moveDirection = playerOrientation.forward * vertInput + playerOrientation.right * horizInput;
        if(isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if(!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedLimiter()
    {
        Vector3 currentVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(currentVelocity.magnitude > moveSpeed && isGrounded)
        {
            Vector3 fixedVelocity = currentVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(fixedVelocity.x, rb.velocity.y, fixedVelocity.z);
        }
    }

    private void Jump()
    {
        rb.drag = 0;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        //rb.velocity = Vector3.up * jumpForce;

    }

    private void JumpRest()
    {
        jumpReady = true;
    }

    private void DashFull()
    {
        dash2.gameObject.SetActive(true);
        dash1.gameObject.SetActive(false);
        dash0.gameObject.SetActive(false);
    }
    private void DashEmpty()
    {
        dash2.gameObject.SetActive(false);
        dash1.gameObject.SetActive(false);
        dash0.gameObject.SetActive(true);
    }
    private void DashHalf()
    {
        dash2.gameObject.SetActive(false);
        dash1.gameObject.SetActive(true);
        dash0.gameObject.SetActive(false);
    }
    private IEnumerator Dash()
    {
        timeSinceDash = 0;
        isGrounded = true;
        moveSpeed = moveSpeed * 6;
        isDashing = true;
        dashesLeft -= 1;
        yield return new WaitForSeconds(0.14f);
        moveSpeed = moveSpeed/6;
        isDashing = false;
        
    }

    private bool CheckGround()
    {
        
        bool groundHit = false;
        /*if (Physics.CheckSphere(groundChecker.transform.position, .4f, groundLayer).Length > 0)
        {
            groundHit = true;
            print(Physics.CheckSphere(groundChecker.transform.position, results[], .4f, groundLayer));
        } */
        return groundHit;
    }

    private void PlaySteps(bool isWalking)
    {
        if(isWalking && footStepAlreadyStarted == false)
        {
            playerAudio.Play("Player Footstep 2");
            footStepAlreadyStarted = true;

        }
        else
        {
            playerAudio.Stop("Player Footstep 2");
            footStepAlreadyStarted = false;
        }
    }

    private void NextTutorial()
    {
        this.gameObject.GetComponent<IntroManager>().DownDialogue();
    }
}
