using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject projectileShooter;

    private float speed = 0.5f;
    private float distance = 0f;
    private float flyHeight = 16f;
    private Vector3 goalPos;

    private bool canFire = true;
    private float timer = 0;
    private float timeBetweenFiring = 0.5f;

    private bool hoverSwitch = false;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        InvokeRepeating("Hovering", 0, 2);
    }

    // Update is called once per frame
    void Update()
    {
        distance = (transform.position - player.transform.position).magnitude;
        //print(distance);
    }

    private void FixedUpdate()
    {
        if (distance <= 30)
        {
            Attack();
        }
        else if (distance <= 50)
        {
            Follow();
        }
        else if (distance > 50)
        {
            Wandering();
        }


    }

    private void Hovering()
    {
        if (hoverSwitch)
        {
            hoverSwitch = false;
            flyHeight = 15f;
        }
        else if (hoverSwitch == false)
        {
            hoverSwitch = true;
            flyHeight = 17f;
        }
    }

    private void Wandering()
    {
        goalPos = new Vector3(startPos.x, flyHeight, startPos.z);
        transform.position = Vector3.Lerp(transform.position, goalPos, Time.deltaTime);
    }

    private void Follow()
    {

        goalPos = new Vector3(player.transform.position.x, flyHeight, player.transform.position.z);

        transform.LookAt(player.transform.position);
        transform.position = Vector3.Lerp(transform.position, goalPos, speed * Time.deltaTime);
    }

    private void Attack()
    {
        //Projectile cooldown
        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenFiring)
            {
                canFire = true;
                timer = 0;
            }
        }

        transform.LookAt(player.transform.position);

        if (canFire)
        {
            canFire = false;
            Vector3 direction = (transform.position - player.transform.position);
            transform.LookAt(player.transform.position);

            GameObject newProjectile = Instantiate(projectile, projectileShooter.transform.position, Quaternion.identity);
            newProjectile.GetComponent<Rigidbody>().AddForce(projectileShooter.transform.forward * 80, ForceMode.Impulse);
        }
        
    }
}
