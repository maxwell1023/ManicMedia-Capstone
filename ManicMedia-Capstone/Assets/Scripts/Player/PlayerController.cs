using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;

    private Transform cameraTransform;

    private InputManager inputManager;

    [SerializeField]
    private GameObject hitbox;
    private bool canAttack = true;
    [SerializeField]
    private float meleeCoolDown = .5f;
    public int playerMelee = 30;

    private void Start()
    {
        hitbox.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
        controller = GetComponent<CharacterController>();
        inputManager = InputManager.Instance;
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 movement = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0f, movement.y);
        move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
        move.y = 0f;
        controller.Move(move * Time.deltaTime * playerSpeed);

       

        // Changes the height position of the player..
        if (inputManager.GetPlayerJump() && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    private IEnumerator Attack()
    {
        if (canAttack == true)
        {
            canAttack = false;
            hitbox.SetActive(true);
            yield return new WaitForSeconds(.3f);
            hitbox.SetActive(false);
            yield return new WaitForSeconds(meleeCoolDown);
            canAttack = true;
        }

    }
}
