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
    private Transform posCheck01, posCheck02, posCheck03, posCheck04, posCheck05, posCheck06;
    //private Transform predictedPos01, predictedPos01, predictedPos01, predictedPos01, predictedPos01, predictedPos01,

    private Transform[] checkArray = new Transform[6]; //we could declare these as arrays first rather than a bunch of individual transforms to array

    public Transform Leg01_Target; //can be serialized
    public Transform Leg02_Target;
    public Transform Leg03_Target;
    public Transform Leg04_Target;
    public Transform Leg05_Target;
    public Transform Leg06_Target;

    private Transform[] targetArray = new Transform[6]; //we could declare these as arrays first rather than a bunch of individual transforms to array

    public Vector3 initLeg01Pos; //can probably also be serialized
    public Vector3 initLeg02Pos;
    public Vector3 initLeg03Pos;
    public Vector3 initLeg04Pos;
    public Vector3 initLeg05Pos;
    public Vector3 initLeg06Pos;

   /* public Vector3 lastLeg01Pos; //same here
    public Vector3 lastLeg02Pos;
    public Vector3 lastLeg03Pos;
    public Vector3 lastLeg04Pos;
    public Vector3 lastLeg05Pos;
    public Vector3 lastLeg06Pos; */

    private Vector3[] lastLegPosArray = new Vector3[6];



    // Start is called before the first frame update
    void Start()
    {
        targetArray[0] = Leg01_Target;
        targetArray[1] = Leg02_Target;
        targetArray[2] = Leg03_Target;
        targetArray[3] = Leg04_Target;
        targetArray[4] = Leg05_Target;
        targetArray[5] = Leg06_Target;

        checkArray[0] = posCheck01;
        checkArray[1] = posCheck02;
        checkArray[2] = posCheck03;
        checkArray[3] = posCheck04;
        checkArray[4] = posCheck05;
        checkArray[5] = posCheck06;


        initLeg01Pos = Leg01_Target.localPosition;
        initLeg02Pos = Leg02_Target.localPosition;
        initLeg03Pos = Leg03_Target.localPosition;
        initLeg04Pos = Leg04_Target.localPosition;
        initLeg05Pos = Leg05_Target.localPosition;
        initLeg06Pos = Leg06_Target.localPosition;

        lastLegPosArray[0] = Leg01_Target.position;
        lastLegPosArray[1] = Leg02_Target.position;
        lastLegPosArray[2] = Leg03_Target.position;
        lastLegPosArray[3] = Leg04_Target.position;
        lastLegPosArray[4] = Leg05_Target.position;
        lastLegPosArray[5] = Leg06_Target.position;
    }

    // Update is called once per frame
    void Update()
    {
      /*Leg01_Target.position = lastLegPosArray[0];
        Leg02_Target.position = lastLegPosArray[1];
        Leg03_Target.position = lastLegPosArray[2];
        Leg04_Target.position = lastLegPosArray[3];
        Leg05_Target.position = lastLegPosArray[4];
        Leg06_Target.position = lastLegPosArray[5]; */

       /* lastLegPosArray[0] = Leg01_Target.position;
        lastLegPosArray[1] = Leg02_Target.position;
        lastLegPosArray[2] = Leg03_Target.position;
        lastLegPosArray[3] = Leg04_Target.position;
        lastLegPosArray[4] = Leg05_Target.position;
        lastLegPosArray[5] = Leg06_Target.position; */

        for (int i = 0; i< targetArray.Length; i++) 
        {
            lastLegPosArray[i] = targetArray[i].position;
            checkLegOffset(checkArray[i], targetArray[i], lastLegPosArray[i]);
        } 

    }

   private void checkLegOffset(Transform thisLeg, Transform thisTarget, Vector3 thisLastPosition)
    {
        RaycastHit hit;
        if (Physics.Raycast(thisLeg.position, -Vector3.up, out hit))
        {
            float distance = Vector3.Distance(thisLeg.position, thisTarget.position);
            if (distance > maxLegDistance) 
            {
                float step = legSpeed * Time.deltaTime;
                thisTarget.position = Vector3.MoveTowards(thisTarget.position, hit.point, step);
            }
            else if(distance < 6)
            {

                thisTarget.position = thisLastPosition;

                
            } 

            Debug.DrawLine(thisLeg.position, hit.point, Color.cyan);
        }
    }
}
