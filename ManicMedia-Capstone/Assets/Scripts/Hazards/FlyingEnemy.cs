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
    private Vector3 playerTargetHeight;

    private bool canFire = true;
    private float timer = 0;
    private float timeBetweenFiring = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        distance = (transform.position - player.transform.position).magnitude;
        print(distance);
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

    private void Wandering()
    {

    }

    private void Follow()
    {
        playerTargetHeight = player.transform.position;

        //playerTargetHeight.y = transform.position.y;
        playerTargetHeight.y = flyHeight;

        transform.LookAt(player.transform.position);
        transform.position = Vector3.Lerp(transform.position, playerTargetHeight, speed * Time.deltaTime);
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
