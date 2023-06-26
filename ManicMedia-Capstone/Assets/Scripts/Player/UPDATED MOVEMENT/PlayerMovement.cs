using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed, groundFriction, playerHeight, jumpForce, jumpCooldown, airMultiplier, fallMultiplier;


    [SerializeField]
    private LayerMask groundLayer;

    public bool isGrounded;
    private bool jumpReady;


    [SerializeField]
    private Transform playerOrientation;

    private float horizInput;
    private float vertInput;

    private Vector3 moveDirection;

    private Rigidbody rb;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        jumpReady = true;


    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);
        MovementInput();
        SpeedLimiter();

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

}
