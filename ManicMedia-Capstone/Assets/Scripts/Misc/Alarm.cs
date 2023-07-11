using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class Alarm : MonoBehaviour
{
    
    [SerializeField] private GameObject[] connectedEnemies;
    [SerializeField]
    private GameObject door;
    [SerializeField] private bool doorController;
    [SerializeField]
    private GameObject spinner, lights, lightCenter, spotLight1, spotLight2;
    private bool playerCaught, doorClosed;
    private Vector3 initialPosition;
    private AudioSource alarmSound, doorSound;
    private bool alarmRinging;
    // Start is called before the first frame update
    void Start()
    {
        alarmSound = GetComponent<AudioSource>();
        doorSound = door.GetComponent<AudioSource>();
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
                if (alarmRinging == true)
                {
                    alarmSound.Stop();
                    alarmRinging = false;
                }


                if(doorController)
                {
                
                    Invoke("OpenDoor", 2);
                }
                
            }

            if (playerCaught && doorClosed == false && !AllSpiderDead())
            {
                doorClosed = true;
                if (alarmRinging == false)
                {
                    alarmSound.Play(0);
                    alarmRinging = true;
                }

                if (doorController)
                {
                    CloseDoor();
                    
                }
            }
        
    }

    private void CheckAlerted()
    {
        for (int i = 0; i < connectedEnemies.Length; i++)
        {
            if (connectedEnemies[i] != null)
            {
                if (connectedEnemies[i].gameObject.GetComponent<EnemyStats>().sawPlayer == true)
                {
                    playerCaught = true;
                }

            }

        }

    }

    private bool AllSpiderDead()
    {
        bool enemiesStillLiving = false;

        for (int i = 0; i < connectedEnemies.Length; i++)
        {
            
                if (connectedEnemies[i].gameObject.GetComponent<EnemyStats>().stillAlive == true)
                {
                    enemiesStillLiving = true;
                }

           
        }
       // print(spidersStillLiving);
        return !enemiesStillLiving;
        
    }

    private void CloseDoor() //closes the doors by putting them in their needed positions
    {

        if (door.CompareTag("Exit"))
        {
            doorSound.Play(0);
            door.GetComponent<DoorAnimation>().CloseDoor();
        }
        else if (door.CompareTag("SmallDoor"))
        {
            doorSound.Play(0);
            StartCoroutine(Move(-8f, 1f));
        }
        else if (door.CompareTag("BigDoor"))
        {
            doorSound.Play(0);
            StartCoroutine(Move(-8f, 1f));
        }
    }

    private void OpenDoor() //opens the doors
    {

        if (door.CompareTag("Exit"))
        {
            doorSound.Play(0);
            door.GetComponent<DoorAnimation>().OpenDoor();
        }
        else if (door.CompareTag("SmallDoor"))
        {
            doorSound.Play(0);
            StartCoroutine(Move(8f, 1f));
        }
        else if (door.CompareTag("BigDoor"))
        {
            doorSound.Play(0);
            StartCoroutine(Move(8f, 1f));
        }
    }

    IEnumerator Move(float heightAdd, float time) //moves the flat doors up and down over time
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
