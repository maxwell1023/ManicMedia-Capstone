using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpiderProceduralScript : MonoBehaviour
{

    [SerializeField]
    private float maxLegDistance = 2f;
    [SerializeField]
    private float matchLimit = 2f;
    [SerializeField]
    private float legSpeed = 20f;
    [SerializeField]
    private float legVertSpeed = 60f;
    [SerializeField]
    private float legStepHeight = 2f;

    [SerializeField]
    private Transform posCheck01, posCheck02, posCheck03, posCheck04, posCheck05, posCheck06;
  
    [SerializeField]
    private Transform[] checkArray = new Transform[6]; 

    [SerializeField]
    private Transform[] targetArray = new Transform[6]; 

    private Vector3[] lastLegPosArray = new Vector3[6];

    private bool[] shouldMove = new bool[6];

    
    //public LayerM IgnoreMe;

    private void Start()
    {

        for (int i = 0; i < shouldMove.Length; i++)
        {
            shouldMove[i] = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
      

        for (int i = 0; i< targetArray.Length; i++) 
        {
            lastLegPosArray[i] = targetArray[i].position;
            checkLegOffset(checkArray[i], targetArray[i], lastLegPosArray[i], shouldMove[i]);
        } 

        this.gameObject.layer = Physics.IgnoreRaycastLayer;

    }

   private void checkLegOffset(Transform thisLeg, Transform thisTarget, Vector3 thisLastPosition, bool shouldMove)
    {
        RaycastHit hit;
        if (Physics.Raycast(thisLeg.position, -(transform.TransformDirection(Vector3.up)), out hit))
        {
            
            float distance = Vector3.Distance(thisLeg.position, thisTarget.position);
            if (distance > maxLegDistance) 
            {
                shouldMove = true;
                

            }
            if (distance > matchLimit && shouldMove == true)
            {

                float step = legSpeed * Time.deltaTime;
                thisTarget.position = Vector3.MoveTowards(thisTarget.position, hit.point, step);

            }

            else if(distance < matchLimit)
            {

                thisTarget.position = thisLastPosition;
                shouldMove = false;

            } 
        }
    }
}
