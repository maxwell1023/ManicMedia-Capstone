using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearCollection : MonoBehaviour
{
    [SerializeField]
    private float gearsCollected = 0;
    [SerializeField]
    private GameObject placeGearsTxt;

    private GameObject currentGearAcceptor;

    private bool canPlaceGears;
    void Start()
    {
        canPlaceGears = false;
        placeGearsTxt.SetActive(false);

    }

    void Update()
    {
        if (canPlaceGears) 
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                PlaceGears();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Gear") 
        {
            Destroy(other.gameObject);
            gearsCollected += 1;
        }
        if(other.gameObject.tag == "GearHolder")
        {
            if(other.gameObject.GetComponent<GearHolder>().doorIsClosed == true) 
            {
                canPlaceGears = true;
                currentGearAcceptor = other.gameObject;
                placeGearsTxt.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "GearHolder")
        {
            canPlaceGears = false;
            placeGearsTxt.SetActive(false);
        }
    }

    private void PlaceGears()
    {
        float gearsLeft = 3 - currentGearAcceptor.gameObject.GetComponent<GearHolder>().gearsPlaced;

        if (gearsLeft >= gearsCollected)
        {

            if (gearsCollected <= 3 && currentGearAcceptor.gameObject.GetComponent<GearHolder>().doorIsClosed == true)
            {
                currentGearAcceptor.gameObject.GetComponent<GearHolder>().addGear(gearsCollected);
                gearsCollected = 0;

                if (currentGearAcceptor.gameObject.GetComponent<GearHolder>().doorIsClosed == false)
                {
                    placeGearsTxt.SetActive(false);
                }
            }
            else if (currentGearAcceptor.gameObject.GetComponent<GearHolder>().doorIsClosed == true)
            {
                currentGearAcceptor.gameObject.GetComponent<GearHolder>().addGear(3f);
                gearsCollected -= 3;

                if (currentGearAcceptor.gameObject.GetComponent<GearHolder>().doorIsClosed == false)
                {
                    placeGearsTxt.SetActive(false);
                }
            }
        }
        else
        {
            currentGearAcceptor.gameObject.GetComponent<GearHolder>().addGear(gearsLeft);
            gearsCollected -= gearsLeft;

            if (currentGearAcceptor.gameObject.GetComponent<GearHolder>().doorIsClosed == false)
            {
                placeGearsTxt.SetActive(false);
            }
        }

       
    }
}
