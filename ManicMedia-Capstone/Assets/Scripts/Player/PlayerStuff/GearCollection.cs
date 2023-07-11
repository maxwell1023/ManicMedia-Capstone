using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GearCollection : MonoBehaviour
{
    [SerializeField]
    private float gearsCollected = 0;
    [SerializeField]
    private GameObject placeGearsTxt, gearUI;

    [SerializeField]
    private TMP_Text gearsHeldText;

    private GameObject currentGearAcceptor;

    private bool canPlaceGears;
    void Start()
    {
        canPlaceGears = false;
        placeGearsTxt.SetActive(false);

    }

    void Update()
    {
        if(gearsCollected == 0)
        {
            gearsHeldText.gameObject.SetActive(false);
            gearUI.gameObject.SetActive(false);
        }
        gearsHeldText.text = gearsCollected.ToString();

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
            gearsHeldText.gameObject.SetActive(true);
            gearUI.gameObject.SetActive(true);
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
        if (gearsCollected == 0)
        {
            placeGearsTxt.GetComponent<TextShake>().ShakeText();
        }

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
