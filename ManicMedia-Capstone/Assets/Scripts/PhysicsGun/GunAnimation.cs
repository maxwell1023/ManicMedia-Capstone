using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator gunAnim;
    [SerializeField]
    private GameObject player;
    private bool isLooping, idleNeedsStart, gunInUse, gunIsShooting;
    // Start is called before the first frame update
    void Start()
    {
        isLooping = true;
        idleNeedsStart = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) ||  Input.GetMouseButtonDown(1) || Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            gunInUse = !gunInUse;
        }

        if(gunInUse || player.GetComponent<PhysicsGun>().isHolding)
        {
            gunAnim.Play("DoNothing");
            gunIsShooting = true;
        }

        if(!gunInUse && gunIsShooting && player.GetComponent<PhysicsGun>().isHolding == false)
        {
            gunIsShooting = false;
            StartIdle();
        }
        
        if (isLooping && idleNeedsStart && gunInUse == false)
        {
            StartIdle();


        }
    }

    public void StartIdle()
    {
        idleNeedsStart = false;
        gunAnim.Play("Idle");
        
    }

    public void Melee()
    {
        isLooping = false;
        idleNeedsStart = true;
        gunAnim.Play("melee");
        Invoke("BackToIdle", .9f);
    }
    public void Shooting()
    {
        isLooping = false;
        idleNeedsStart = true;
        gunAnim.Play("melee");
        Invoke("BackToIdle", .9f);
    }

    public void BackToIdle()
    {
        isLooping = true;
        StartIdle();
    }


}
