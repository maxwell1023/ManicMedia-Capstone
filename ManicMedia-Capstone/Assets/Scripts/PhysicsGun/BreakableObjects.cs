using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.ProBuilder;

public class BreakableObjects : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject brokenVersion;
    [SerializeField] private float breakVelocity = 2;
    [SerializeField] private bool isMetal;
    private AudioSource thud;
    void Start()
    {
        thud = GetComponent<AudioSource>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 velocity = collision.relativeVelocity;
        //print(velocity.magnitude);
        if (velocity.magnitude >= breakVelocity && isMetal == false)
        {
            GameObject brokenObject = Instantiate(brokenVersion, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }
        else if(isMetal && velocity.magnitude >= breakVelocity)
        {
            //print("metal hit!");
            if (collision.gameObject.GetComponent<BreakableDoors>() != null)
            {
                //print("doorSHOULDshatter!");
                collision.gameObject.GetComponent<BreakableDoors>().shatterDoor();
            }
        }
        else if(velocity.magnitude >= 4)
        {
            thud.volume = Mathf.Clamp01(velocity.magnitude / 30);
            thud.Play();
        }
    }
}
