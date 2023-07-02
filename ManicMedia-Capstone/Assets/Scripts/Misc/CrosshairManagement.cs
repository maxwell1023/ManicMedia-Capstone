using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairManagement : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform cam;

    [SerializeField]
    private GameObject normalCrossHair, pullCrossHair, grabCrossHair;

    void Start() 
    {
        normalCrossHair.SetActive(false);
        pullCrossHair.SetActive(false);
        grabCrossHair.SetActive(false);
    }

    // Update is called once per frame
    void Update() 
    {
        CheckSights();
    }
    private void CheckSights() //sets the crosshair based on ammount of ammo & what is in sights
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, 300f, this.gameObject.GetComponent<PhysicsGun>().physicsInteractableObjectMask) && this.gameObject.GetComponent<PhysicsGun>().isLasering == false || this.gameObject.GetComponent<PhysicsGun>().isHolding == true)
        {
            grabCrossHair.SetActive(true);
            normalCrossHair.SetActive(false);
            pullCrossHair.SetActive(false);
        }
        else if (Physics.Raycast(cam.position, cam.forward, out hit, 300f, this.gameObject.GetComponent<Swinging>().grappleable) && this.gameObject.GetComponent<Swinging>().canGrapple && this.gameObject.GetComponent<PhysicsGun>().isLasering == false)
        {
            pullCrossHair.SetActive(true);
            normalCrossHair.SetActive(false);
            grabCrossHair.SetActive(false);
        }
        else
        {
            normalCrossHair.SetActive(true);
            pullCrossHair.SetActive(false);
            grabCrossHair.SetActive(false);
        }
        
    }
}
