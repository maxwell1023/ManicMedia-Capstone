using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextShake : MonoBehaviour
{
    
    [SerializeField]
    private float offset, speed, length;

    private Vector3 originalPosition, leftSide, rightSide;

    private float shakeTime;
    private bool moving, moveRight;
    void Start() //gets the orginal position & positions to offset to
    {
        originalPosition = this.transform.position;
        leftSide = new Vector3(transform.position.x - offset, transform.position.y, transform.position.z);
        rightSide = new Vector3(transform.position.x + offset, transform.position.y, transform.position.z);
    }

    
    void Update()
    {
        if(moving == true) //shake text between right & left
        {
            
                if (moveRight)
                {
                    this.transform.position = Vector3.Lerp(transform.position, rightSide, Time.deltaTime * speed);
                }
                else
                {
                    this.transform.position = Vector3.Lerp(transform.position, leftSide, Time.deltaTime * speed);
                }

                if (this.transform.position.x <= leftSide.x + 2)
                {
                    moveRight = true;
                }
                if (this.transform.position.x >= rightSide.x -2)
                {
                    moveRight = false;
                }
                shakeTime += Time.deltaTime;
            

        }
    }

    public void ShakeText() //will be called by gear placement (GearCollection) script to start shaking
    {
        moving = true;
        Invoke("StopShake", length);
        
    }

    private void StopShake() //stops the shake...
    {
        moving = false;
        this.transform.position = originalPosition;
    }
}
