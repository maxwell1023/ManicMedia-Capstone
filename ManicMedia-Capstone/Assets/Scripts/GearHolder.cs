using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearHolder : MonoBehaviour
{
    [SerializeField]
    public float gearsPlaced;

    [SerializeField]
    private GameObject gear1Pt1, gear1Pt2, gear1Pt3, gear2Pt1, gear2Pt2, gear2Pt3, gear3Pt1, gear3Pt2, gear3Pt3;

    [SerializeField]
    private GameObject attachedDoor;

    public bool doorIsClosed;
    // Start is called before the first frame update
    void Start()
    {
        doorIsClosed = true;
        updateGearPlacement();
    }

    // Update is called once per frame

    public void addGear(float gearsAdded)
    {
        gearsPlaced = gearsPlaced + gearsAdded;
        updateGearPlacement();

    }

    private void updateGearPlacement()
    {
        if (gearsPlaced == 1)
        {
            gear1Pt1.SetActive(true);
            gear1Pt2.SetActive(true);
            gear1Pt3.SetActive(true);
            gear2Pt1.SetActive(false);
            gear2Pt2.SetActive(false);
            gear2Pt3.SetActive(false);
            gear3Pt1.SetActive(false);
            gear3Pt2.SetActive(false);
            gear3Pt3.SetActive(false);
        }
        else if (gearsPlaced == 2)
        {
            gear1Pt1.SetActive(true);
            gear1Pt2.SetActive(true);
            gear1Pt3.SetActive(true);
            gear2Pt1.SetActive(true);
            gear2Pt2.SetActive(true);
            gear2Pt3.SetActive(true);
            gear3Pt1.SetActive(false);
            gear3Pt2.SetActive(false);
            gear3Pt3.SetActive(false);
        }
        else if(gearsPlaced >= 3)
        {
            gear1Pt1.SetActive(true);
            gear1Pt2.SetActive(true);
            gear1Pt3.SetActive(true);
            gear2Pt1.SetActive(true);
            gear2Pt2.SetActive(true);
            gear2Pt3.SetActive(true);
            gear3Pt1.SetActive(true);
            gear3Pt2.SetActive(true);
            gear3Pt3.SetActive(true);
            doorIsClosed = false;
            Invoke("UpdateDoor", 0.5f);
        }
        else
        {
            gear1Pt1.SetActive(false);
            gear1Pt2.SetActive(false);
            gear1Pt3.SetActive(false);
            gear2Pt1.SetActive(false);
            gear2Pt2.SetActive(false);
            gear2Pt3.SetActive(false);
            gear3Pt1.SetActive(false);
            gear3Pt2.SetActive(false);
            gear3Pt3.SetActive(false);
        }
    }

    private void UpdateDoor()
    {
        if (attachedDoor.tag == "Exit")
        {

        }
        else if(attachedDoor.tag == "SmallDoor") 
        {
            StartCoroutine(Move(8f, 1f));
        }
        else if (attachedDoor.tag == "BigDoor")
        {
            StartCoroutine(Move(8f, 1f));
        }
    }
    IEnumerator Move(float heightAdd, float time)
    {
        Vector3 initialPosition = attachedDoor.transform.position;
        Vector3 newPosition = new Vector3(attachedDoor.transform.position.x, attachedDoor.transform.position.y + heightAdd, attachedDoor.transform.position.z);
        for (float t = 0; t < 1; t += Time.deltaTime / time)
        {
            attachedDoor.transform.position = Vector3.Lerp(initialPosition, newPosition, t);
            yield return null;
        }
    }
}
