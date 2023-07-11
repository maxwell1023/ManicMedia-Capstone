using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OHCollisionCheck : MonoBehaviour
{
    [SerializeField]
    private GameObject playerTransform;

    [SerializeField]
    private LayerMask checkingMask, reverseMask;


    public bool somethingBetween;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(playerTransform.transform);

        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, 100f))
        {
            //print(hit.collider.gameObject.layer);
            if(hit.collider.gameObject.layer != 3 && hit.collider.gameObject.layer != 7)
            {
                somethingBetween = true;
            }
            else 
            {
                somethingBetween = false;
            }
            //print(somethingBetween);
        }
        
     
    }
}
