using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhysicsGun : MonoBehaviour
{
    [SerializeField]
    private float burnoutSpeed, regenSpeed, maxLaserCharge, laserCharge;
    [SerializeField]
    private bool laserNeedsRecharge;

    [SerializeField]
    private float damageRate = 10;
    [SerializeField] private AudioManager gunAudio;
    [SerializeField]
    private float sliderMultiplier = 1f;
    //private float laserCharge, lastLaserCharge;

    public Camera cam;
    public LayerMask physicsInteractableObjectMask;
    public float maxGrabDistance = 30f;
    public float maxLaserDistance = 10000f;
    public Transform objectHolder;
    private Rigidbody grabbedRB = null;

    [SerializeField]
    private Transform firePoint;
    private Vector3 grabPoint, currentShotPosition, laserPoint;

    private bool rotateMode = false;
    private bool laserMode = false;

    public Slider flingSlider, laserSlider;
    private float sliderNum = 0;

    [SerializeField] private LineRenderer laserRender;
    [SerializeField] private LineRenderer grabRender;

    private bool laserStartedDecreasing;//, laserNeedsRecharge; //MAYBE
    private float secondsLasered = 0;
    private GameObject lastThingLasered;
    public bool isHolding, isLasering;

    private float releaseTime;

    private bool hasPlayedLow, hasPlayedEmpty;
    /// <updatedstuff>
    /// 
    /// 
    /// 
    /// 
    /// 
    /// </summary>

    private float grabbedDistance;
    private Vector3 grabbedTargetPosition;
  //  private



    /// <updatedstuff>
    /// 
    /// 
    /// 
    /// 
    /// 
    /// </summary>

    private void Start()
    {
        laserSlider.maxValue = maxLaserCharge;
        flingSlider.gameObject.SetActive(false);
        laserCharge = maxLaserCharge;
        laserRender.enabled = false;
        laserRender.positionCount = 0;
        grabRender.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (Input.GetMouseButtonDown(1) && this.gameObject.GetComponent<PlayerMelee>().canAttack == true && isLasering == false)
        {
            GrabObject();
            StopLaserMode();
        }

        if (Input.GetMouseButton(0) && isHolding)
        {
            releaseTime += Time.deltaTime * sliderMultiplier;
            ObjectVelocity(releaseTime);
        }

        if (Input.GetMouseButtonUp(0) && isHolding)
        {
            releaseTime = 0;
            FlingObject();
            isHolding = false;
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            ChangeObjectDistance();
        }

        if (!isHolding && this.gameObject.GetComponent<PlayerMelee>().canAttack == true && this.gameObject.GetComponent<Swinging>().isSwinging == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!laserNeedsRecharge)
                {
                    laserRender.enabled = true;
                    isLasering = true;
                    
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            laserStartedDecreasing = false;
            isLasering = false;
            secondsLasered = 0;
            StopLaserMode();
        }
        
        if(!isLasering)
        {
            if (laserCharge < maxLaserCharge)
            {
                laserCharge = Mathf.Lerp(laserCharge, maxLaserCharge, Time.deltaTime * regenSpeed);
            }
        } 
        laserSlider.value = laserCharge;

        if (laserCharge < 0.3 * maxLaserCharge && laserCharge > 0)
        {
            if (hasPlayedLow == false)
            {
                gunAudio.Play("Weapon Low Whir");
                hasPlayedLow = true;
            }
        }
        if(laserCharge > .75f)            //CHANGE CHANGE CHANGE TO MATCH THE ACTUAL LENGTH OF OVERHEAT SOUND + A SMALL DELAY
        {
            hasPlayedLow = false;   
        }

        if (laserCharge <= 0)
        {
            hasPlayedLow = false;
            isLasering = false;
            secondsLasered = 0;
            StopLaserMode();
            laserNeedsRecharge = true;
            if(hasPlayedEmpty == false)
            {
                gunAudio.Play("Weapon Overheat");
                hasPlayedEmpty = true;
            }
        }

        if(laserNeedsRecharge == true)
        {
            if(laserCharge < maxLaserCharge * .98f)// * (99/100)))
            {
                laserNeedsRecharge = true;
                hasPlayedEmpty = false;
            }
            else
            {
                laserCharge = maxLaserCharge;
                laserNeedsRecharge = false; 
            }

            
        }


        if (objectHolder.transform.localPosition.z > 25)
        {
            objectHolder.transform.localPosition = new Vector3(objectHolder.transform.localPosition.x, objectHolder.transform.localPosition.y, 25);
        }
        if (objectHolder.transform.localPosition.z < 8)
        {
            objectHolder.transform.localPosition = new Vector3(objectHolder.transform.localPosition.x, objectHolder.transform.localPosition.y, 8);
        }

    }

    private void LateUpdate()
    {
        //Moves the held objects rigidbody to the specificed position
        if (grabbedRB)
        {

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            //grabbedTargetPosition = (ray.origin + ray.direction * grabbedDistance) - grabbedRB.transform.TransformVector
            Vector3 newObjectPos = ray.GetPoint(objectHolder.transform.localPosition.z);

            objectHolder.transform.LookAt(cam.transform.position, Vector3.up);

            grabbedRB.velocity = Vector3.zero;
            if(objectHolder.gameObject.GetComponent<OHCollisionCheck>().somethingBetween == false) 
            {
                grabbedRB.position = Vector3.Lerp(grabbedRB.transform.position, newObjectPos, Time.deltaTime * 10);
            }
           // grabbedRB.MovePosition(Vector3.Lerp(grabbedRB.transform.position, newObjectPos, Time.deltaTime * 10));
           // grabbedRB.rotation = Quaternion.Slerp(grabbedRB.transform.rotation, objectHolder.transform.rotation, Time.deltaTime * 10);



            /*
            for (int i = 0; i <= grabRender.positionCount - 1; i++)
            {
                if(i == 0)
                {
                    grabRender.SetPosition(0, transform.position);
                }
                else if(i == grabRender.positionCount - 1)
                {
                    grabRender.SetPosition(i, grabbedRB.position);
                }
                else
                {
                    float count = grabRender.positionCount;
                    float calculatedLinePoint = i * (1 / count);
                    print(calculatedLinePoint);
                    Vector3 grabRenderMidPoint = Vector3.Lerp(grabRender.GetPosition(i), ray.GetPoint(objectHolder.transform.localPosition.z * calculatedLinePoint), Time.deltaTime * 2.5f);
                    grabRender.SetPosition(i, grabRenderMidPoint);
                }
                
            }
            */


            grabRender.enabled = true;
            grabRender.SetPosition(0, firePoint.position);
            
            Vector3 currentGrab = new Vector3();
            currentGrab = grabRender.GetPosition(1);
            grabRender.SetPosition(1, Vector3.Lerp(currentGrab, grabbedRB.gameObject.GetComponent<Renderer>().bounds.center, Time.deltaTime * 80f));
            
            

        }
        else
        {
            grabRender.enabled = false;
            
        }

        if (isLasering)
        {
            LaserMode();
            DrawLaser();
            if (laserCharge > 0)
            {
                laserCharge -= Time.deltaTime * burnoutSpeed;
            }

        }


    }

    private void GrabObject()
    {

        if (grabbedRB)
        {
            //Drop the object if one is already being held
            ReleaseObject();
            isHolding = false;
        }
        else
        {
            //Pick up an object the player is pointing at with the mouse
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, maxGrabDistance, physicsInteractableObjectMask))
            {
                isHolding = true;
                grabbedRB = hit.collider.gameObject.GetComponent<Rigidbody>();
                if (grabbedRB)
                {
                    grabbedRB.useGravity = false;

                }
            }
        }
        
    }

    //this lets go of the current object grabbed by the player
    private void ReleaseObject()
    {
        grabbedRB.useGravity = true;
        grabbedRB = null;

        objectHolder.localPosition = objectHolder.localPosition - (objectHolder.localPosition - new Vector3(0,0,8));
    }


    private void ChangeObjectDistance()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && objectHolder.transform.localPosition.z < 25)
        {
            objectHolder.transform.localPosition = Vector3.Lerp(objectHolder.transform.localPosition, objectHolder.transform.localPosition + new Vector3(0, 0, 20), Time.deltaTime * 30);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && objectHolder.transform.localPosition.z > 8)
        {
            objectHolder.transform.localPosition = Vector3.Lerp(objectHolder.transform.localPosition, objectHolder.transform.localPosition - new Vector3(0, 0, 20), Time.deltaTime * 30);
        }
    }

    private void FlingObject()
    {
        if (grabbedRB)
        {

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 newObjectPos = ray.GetPoint(objectHolder.transform.localPosition.z);


            grabbedRB.useGravity = true;
            Vector3 direction = (newObjectPos - cam.transform.position).normalized;
            grabbedRB.AddForce(direction * (sliderNum * 100), ForceMode.Impulse);
            grabbedRB = null;

            sliderNum = 0;
            flingSlider.gameObject.SetActive(false);
        }


    }

    private void ObjectVelocity(float numbey)
    {
        if (grabbedRB)
        {
            sliderNum = numbey;
            if (sliderNum >= 100)
            {
                sliderNum = 100;
            }

            flingSlider.value = sliderNum;
            flingSlider.gameObject.SetActive(true);
        }

    }


    private void LaserMode()
    {
        
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxLaserDistance))
            {
                laserPoint = hit.point;
                if(lastThingLasered == null)
                {
                    lastThingLasered = hit.transform.gameObject;
                }

                if (hit.transform.gameObject != lastThingLasered)
                {
                    secondsLasered = 0;
                    lastThingLasered = hit.transform.gameObject;
                }

                  
                if (hit.transform.gameObject.tag == "laserReciever")
                 {
                     secondsLasered += Time.deltaTime;
                     if (secondsLasered >= 2.2)
                     {
                    
                        hit.transform.gameObject.GetComponent<LaserReciever>().DropGear(); 
                     }
                     
                }
                if (hit.transform.gameObject.tag == "Spider")
                {
                    secondsLasered += Time.deltaTime;
                    hit.transform.root.transform.GetComponent<SpiderEnemy>().sEnemyHealth -= secondsLasered * damageRate;

                }
                if (hit.transform.gameObject.tag == "Flyer")
                {
                secondsLasered += Time.deltaTime;
                hit.transform.root.transform.GetComponent<FlyingEnemy>().fEnemyHealth -= secondsLasered * damageRate;

                }

            if (hit.transform.root.transform.CompareTag("Boss"))
                {
                    hit.transform.root.transform.GetComponent<Boss>().SubtractBossHealth(0.5f);
                    
                }
               
                //float distanceFromAttach = Vector3.Distance(player.position, attachPoint);

                laserRender.positionCount = 2;
                currentShotPosition = firePoint.position;

                }

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 100f;

            laserRender.SetPosition(0, transform.position);
            laserRender.SetPosition(laserRender.positionCount - 1, cam.ScreenToWorldPoint(mousePos));
        
        
    }

   /* private void CheckRecieverTime()
    {
        float secondsHeld = 0;
        secondsHeld += Time.deltaTime;
    } */
    private void StopLaserMode()
    {
        laserRender.positionCount = 0;
    }

    private void DrawLaser()
    {
       // if (!lasering) return;

        currentShotPosition = Vector3.Lerp(currentShotPosition, laserPoint, Time.deltaTime * 800f);

        laserRender.SetPosition(0, firePoint.position);
        laserRender.SetPosition(1, currentShotPosition);

    }
}
