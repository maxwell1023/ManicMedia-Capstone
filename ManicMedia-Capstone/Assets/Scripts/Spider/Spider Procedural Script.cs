using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderProceduralScript : MonoBehaviour
{
    public Transform Leg01_Target;
    public Transform Leg02_Target;
    public Transform Leg03_Target;
    public Transform Leg04_Target;
    public Transform Leg05_Target;
    public Transform Leg06_Target;

    public Vector3 initLeg01Pos;
    public Vector3 initLeg02Pos;
    public Vector3 initLeg03Pos;
    public Vector3 initLeg04Pos;
    public Vector3 initLeg05Pos;
    public Vector3 initLeg06Pos;

    public Vector3 lastLeg01Pos;
    public Vector3 lastLeg02Pos;
    public Vector3 lastLeg03Pos;
    public Vector3 lastLeg04Pos;
    public Vector3 lastLeg05Pos;
    public Vector3 lastLeg06Pos;



    // Start is called before the first frame update
    void Start()
    {
        initLeg01Pos = Leg01_Target.localPosition;
        initLeg02Pos = Leg02_Target.localPosition;
        initLeg03Pos = Leg03_Target.localPosition;
        initLeg04Pos = Leg04_Target.localPosition;
        initLeg05Pos = Leg05_Target.localPosition;
        initLeg06Pos = Leg06_Target.localPosition;

        lastLeg01Pos = Leg01_Target.position;
        lastLeg02Pos = Leg02_Target.position;
        lastLeg03Pos = Leg03_Target.position;
        lastLeg04Pos = Leg04_Target.position;
        lastLeg05Pos = Leg05_Target.position;
        lastLeg06Pos = Leg06_Target.position;
    }

    // Update is called once per frame
    void Update()
    {
        Leg01_Target.position = lastLeg01Pos;
        Leg02_Target.position = lastLeg02Pos;
        Leg03_Target.position = lastLeg03Pos;
        Leg04_Target.position = lastLeg04Pos;
        Leg05_Target.position = lastLeg05Pos;
        Leg06_Target.position = lastLeg06Pos;

        lastLeg01Pos = Leg01_Target.position;
        lastLeg02Pos = Leg02_Target.position;
        lastLeg03Pos = Leg03_Target.position;
        lastLeg04Pos = Leg04_Target.position;
        lastLeg05Pos = Leg05_Target.position;
        lastLeg06Pos = Leg06_Target.position;



    }
}
