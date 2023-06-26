using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextShake : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float offset, speed, length;

    private Vector3 originalPosition, leftSide, rightSide;

    private float shakeTime;
    private bool moving, moveRight;
    void Start()
    {
        originalPosition = this.transform.position;
        leftSide = new Vector3(transform.position.x - offset, transform.position.y, transform.position.z);
        rightSide = new Vector3(transform.position.x + offset, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if(moving == true)
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

    public void ShakeText()
    {
        moving = true;
        Invoke("StopShake", length);
        
    }

    private void StopShake() 
    {
        moving = false;
        this.transform.position = originalPosition;
    }
}
