using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : MonoBehaviour
{
    [SerializeField]
    private GameObject[] connectedSpiders;
    [SerializeField]
    private GameObject door;
    [SerializeField]
    private GameObject spinner, lights, lightCenter, spotLight1, spotLight2;
    private bool playerCaught, doorClosed;
    private Vector3 initialPosition;
    // Start is called before the first frame update
    void Start()
    {
        lights.SetActive(false);
        initialPosition = door.transform.position;

         OpenDoor();
        doorClosed = false;
        //spidersStillLiving = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (doorClosed)
        {
            lights.SetActive(true);
            spinner.transform.RotateAround(lightCenter.transform.position, Vector3.right, 200 * Time.deltaTime);
        }
        
    }

    private void FixedUpdate()
    {
        CheckAlerted();

        if (AllSpiderDead() && doorClosed == true)
        {
            doorClosed = false;
            lights.SetActive(false);
            Invoke("OpenDoor", 2);
        }

        if(playerCaught && doorClosed == false && !AllSpiderDead()) 
        {
            doorClosed = true;
            CloseDoor();
        }
    }

    private void CheckAlerted()
    {
        for (int i = 0; i < connectedSpiders.Length; i++)
        {
            if (connectedSpiders[i] != null)
            {
                if (connectedSpiders[i].gameObject.GetComponent<SpiderEnemy>().hasSeenPlayer == true)
                {
                    playerCaught = true;
                }

            }

        }

    }

    private bool AllSpiderDead()
    {
        bool spidersStillLiving = false;

        for (int i = 0; i < connectedSpiders.Length; i++)
        {
            
                if (connectedSpiders[i].gameObject.GetComponent<SpiderEnemy>().isAlive == true)
                {
                    spidersStillLiving = true;
                }

           
        }
       // print(spidersStillLiving);
        return !spidersStillLiving;
        
    }

    private void CloseDoor()
    {

        if (door.tag == "Exit")
        {
            door.GetComponent<DoorAnimation>().CloseDoor();
        }
        else if (door.tag == "SmallDoor")
        {
            door.transform.position = initialPosition;
        }
        else if (door.tag == "BigDoor")
        {
            door.transform.position = initialPosition;
        }
    }

    private void OpenDoor()
    {
        
        if (door.tag == "Exit")
        {
            door.GetComponent<DoorAnimation>().OpenDoor();
        }
        else if (door.tag == "SmallDoor")
        {
            StartCoroutine(Move(8f, 1f));
        }
        else if (door.tag == "BigDoor")
        {
            StartCoroutine(Move(8f, 1f));
        }
    }

    IEnumerator Move(float heightAdd, float time)
    {
        Vector3 initialPosition = door.transform.position;
        Vector3 newPosition = new Vector3(door.transform.position.x, door.transform.position.y + heightAdd, door.transform.position.z);
        for (float t = 0; t < 1; t += Time.deltaTime / time)
        {
            door.transform.position = Vector3.Lerp(initialPosition, newPosition, t);
            yield return null;
        }
    }
}
