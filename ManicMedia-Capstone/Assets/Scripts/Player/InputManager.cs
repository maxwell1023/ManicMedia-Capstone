using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance
    {
        get { return _instance; }
    }
    private FirstPerson firstPerson;

    private void Awake()
    {
        if(_instance != null && _instance != this) 
        {
            Destroy(this.gameObject);

        }
        else
        {
            _instance = this;
        }
        firstPerson = new FirstPerson();
    }

    
    private void OnEnable()
    {
        firstPerson.Enable();
    }

    private void OnDisable()
    {
        firstPerson.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return firstPerson.Player.Move.ReadValue<Vector2>();
    }
    public Vector2 GetCameraMovement()
    {
        return firstPerson.Player.Look.ReadValue<Vector2>();
    }
    public bool GetPlayerJump()
    {
        return firstPerson.Player.Jump.triggered;
    }
}
