using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float camSensityX;
    public float camSensityY;

    [SerializeField]
    private Transform playerOrientation;

    private float camRotationX;
    private float camRotationY;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime* camSensityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * camSensityY;

        camRotationY += mouseX;
        camRotationX -= mouseY;

        camRotationX = Mathf.Clamp(camRotationX, -90f, 90f);

        transform.rotation = Quaternion.Euler(camRotationX, camRotationY, 0f);
        playerOrientation.rotation = Quaternion.Euler(0, camRotationY, 0);

    }
}
