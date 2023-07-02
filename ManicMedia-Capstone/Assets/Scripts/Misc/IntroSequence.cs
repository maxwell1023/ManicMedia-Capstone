using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSequence : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject wasdText, laserText, grappleText, flyingText;
    private bool wasdIsCurrent, laserIsCurrent, grappleIsCurrent, beatRM1, flyingIsCurrent;

    //
    //
    //
    //    THIS SCRIPT IS A STARTING POINT BUT NEEDS TO BE ENTIRELY RE-DONE!!!
    //
    //
    //
    //
    void Start()
    {
        wasdText.SetActive(false);
        laserText.SetActive(false);
        grappleText.SetActive(false);
        flyingText.SetActive(false);

        Invoke("DisplayWASD", 3f);
    }

    // Update is called once per frame
    void Update()
    {
        if(wasdIsCurrent)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
            {
                TakeTextDown(wasdText);
                Invoke("DisplayLaserText", 2f);
                wasdIsCurrent = false;
            }
        }
        if (laserIsCurrent)
        {
            if (Input.GetMouseButtonDown(0))
            {
                TakeTextDown(laserText);
                Invoke("DisplayGrappleText", 2f);
                laserIsCurrent = false;
            }
        }
        if (grappleIsCurrent)
        {
            if (Input.GetMouseButtonDown(1))
            {
                TakeTextDown(grappleText);
                grappleIsCurrent = false;
            }
        }
        if (flyingIsCurrent)
        {
            if (Input.GetMouseButtonDown(1))
            {
                TakeTextDown(flyingText);
                flyingIsCurrent = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "checkpoint" && beatRM1 == false)
        {
            flyingText.SetActive(true);
            flyingIsCurrent = true;
            
        }
    }

    private void DisplayWASD()
    {
        wasdText.SetActive(true);
        wasdIsCurrent = true;
    }

    private void DisplayLaserText()
    {
        laserText.SetActive(true);
        laserIsCurrent = true;
    }

    private void DisplayGrappleText()
    {
        grappleText.SetActive(true);
        grappleIsCurrent = true;
    }

    private void TakeTextDown(GameObject textToRid)
    {
        textToRid.SetActive(false);
    }
    
}
