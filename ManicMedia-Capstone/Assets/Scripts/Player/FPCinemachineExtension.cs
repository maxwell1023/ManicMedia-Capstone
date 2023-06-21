using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class FPCinemachineExtension : CinemachineExtension
{

    [SerializeField]
    private float clampAngle = 80f;
    [SerializeField]
    private float horizontalSpeed = 10f;
    [SerializeField]
    private float verticalSpeed = 10f;

    private InputManager inputManager;
    private Vector3 startingRotation;

    protected override void Awake()
    {
        inputManager = InputManager.Instance;
        base.Awake();
        //Debug.unityLogger.logEnabled = false; //!!!!DANGER!!!! This is hiding a problem with this script running while not in play mode... need to find a way to fix the editor

    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        //Debug.unityLogger.logEnabled = false; //!!!!DANGER!!!! This is hiding a problem with this script running while not in play mode... need to find a way to fix the editor

        if (vcam.Follow)
        {
            if (stage == CinemachineCore.Stage.Aim)
            {
                if (startingRotation == null) startingRotation = transform.localRotation.eulerAngles;
                Vector2 mouseInput = inputManager.GetCameraMovement();
                startingRotation.x += mouseInput.x * verticalSpeed * Time.deltaTime;
                startingRotation.y += mouseInput.y * horizontalSpeed * Time.deltaTime;
                startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngle, clampAngle);

                state.RawOrientation = Quaternion.Euler(-startingRotation.y, startingRotation.x, 0f);

            }
        }
    }
}
