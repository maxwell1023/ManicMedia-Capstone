using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{


    [SerializeField] public float fEnemyHealth = 50;



    [SerializeField] UnityEngine.AI.NavMeshAgent flyerAgent;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject attractor;
    [SerializeField] private GameObject projectileShooter;
   // [SerializeField] private GameObject rotationSetter;
    [SerializeField] private GameObject antenna, frontCheck, backCheck;
    [SerializeField] private float followDist, attackDist;
    [SerializeField] private float maxHeight, minHeight, antennaRange, cielingCheckRange, vertSpeed;
    private bool isStill, shouldReach;
    [SerializeField] private float speed;
    private float distance = 0f;
   // private float flyHeight = 16f;
    private Vector3 goalPos;
    private bool inSights, obstaclesInAir, obstaclesAboveFront, obstaclesAboveRear;
    private bool begunRerouting, halt, breakingForObstacles;

    private bool hasSeenPlayer;
    private bool followThrough;
    private bool minWasReachedLast;
    private bool pauseStarted;
    private GameObject lastHoneyPoint;
    [SerializeField] private float flyRange;
    private bool flyPointSet;
    [SerializeField] LayerMask groundMask, obstacleMask;

    private bool canFire = true;
    private float timer = 0;
    private float timeBetweenFiring = 1f;

    private bool hoverSwitch = false;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        followThrough = true;
        startPos = this.transform.position;
        
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

       // InvokeRepeating("Hovering", 0, 0.8f);
    }

    // Update is called once per frame
    void Update()
    {
        distance = (this.transform.position - player.transform.position).magnitude;
        //print(distance);
        this.gameObject.GetComponent<EnemyStats>().sawPlayer = hasSeenPlayer;
    }

    private void FixedUpdate()
    {
        if (fEnemyHealth > 0)
        {
            print(breakingForObstacles);
            print(flyerAgent.speed);
            print(goalPos);
            if (this.transform.localPosition.y <= minHeight)
            {
                minWasReachedLast = true;
                print("at min heaight!");
            }
            if (this.transform.localPosition.y >= maxHeight)
            {
                minWasReachedLast = false;
                print("at max heaight!");
            }

            halt = checkAirObstacles();
            if (distance <= attackDist)
            {
                Attack();
            }
            else if (distance <= followDist && distance > attackDist)
            {
                Follow();
                hasSeenPlayer = true;
            }
            else if (distance > followDist && isStill == false)
            {
                Wandering();
            }
        }

        else if(fEnemyHealth <= 0)
        {
            this.gameObject.GetComponent<EnemyStats>().stillAlive = false;
            this.gameObject.SetActive(false);
        }



    }

   /* private void Hovering()
    {
        if (isStill)
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
    } */

    private void Wandering()
    {
        if (!flyPointSet && begunRerouting == false)
        {
            // spiderAgent.speed = defaultSpeed;
            RandomizePoint(flyRange, flyRange);
        }
        else if(begunRerouting && !flyPointSet)
        {
            RandomizePoint(flyRange * 1.2f, 0);
        }

        if (flyPointSet)
        {
            
            if (breakingForObstacles == false)
            {

                flyerAgent.speed = speed;
                flyerAgent.SetDestination(goalPos);
                if (this.transform.localPosition.y > minHeight && this.transform.localPosition.y < maxHeight && followThrough) //&& Mathf.Abs(this.transform.position.y - goalPos.y) > .5f)
                {
                    this.transform.localPosition = new Vector3(this.transform.localPosition.x, Mathf.Lerp(this.transform.localPosition.y, goalPos.y+3, (1 / vertSpeed) * Time.deltaTime), this.transform.localPosition.z);
                    print("adjustingheight");
                }
            }

           // flyerAgent.SetDestination(goalPos);

        }
            Vector3 distanceToPoint = new Vector3(this.transform.position.x, 0, this.transform.position.z) - new Vector3(goalPos.x, 0, goalPos.z);
            print(distanceToPoint.magnitude);



        if (distanceToPoint.magnitude < 1f)
        {
            begunRerouting = false;

            if (pauseStarted == false)
            {
                StartCoroutine(PauseFly());
            }
            followThrough = true;
            print("POINT FOUND!");
            Destroy(lastHoneyPoint.gameObject);
        }
    }

    private void RandomizePoint(float rearRange, float frontRange)
    {
        print("searchin For Point");
        float randomVert = Random.Range(-rearRange, frontRange);
        float randomHoriz = Random.Range(-rearRange, frontRange);
        float randY = Random.Range(-rearRange * 0.5f, frontRange * 0.5f);

        goalPos = new Vector3(transform.localPosition.x + randomHoriz, this.transform.position.y + randY, transform.localPosition.z + randomVert);

        RaycastHit hit;
        if (Physics.Raycast(goalPos, -transform.up, out hit, this.transform.position.y+12))//, groundMask))
        {
            if (hit.collider.gameObject.layer == 6)
            {
                flyPointSet = true;
                print("Honey spawned");
                GameObject newHoney = Instantiate(attractor, new Vector3(goalPos.x, goalPos.y+3, goalPos.z), Quaternion.identity);
                print(goalPos);
                lastHoneyPoint = newHoney;
                pauseStarted = false;
                //RandomizePoint(rearRange, frontRange);
            }
            
            
            // print("POINT FOUND!");
            // flyPointSet = true;
            // GameObject newHoney = Instantiate(attractor, goalPos, Quaternion.identity);
            // lastHoneyPoint = newHoney;
        }

     
        //goalPos = new Vector3(startPos.x, flyHeight, startPos.z);
        //this.transform.position = Vector3.Lerp(transform.position, goalPos, Time.deltaTime);
    }

    private void Follow()
    {
        begunRerouting = false;
        //goalPos = new Vector3(player.transform.position.x, flyHeight, player.transform.position.z);
        flyerAgent.SetDestination(player.transform.position);
        this.transform.LookAt(new Vector3(player.transform.position.x, this.transform.localPosition.y, player.transform.position.z));
        //rotationSetter.transform.LookAt(player.transform.position);
        //transform.eulerAngles = new Vector3(rotationSetter.transform.rotation.x, rotationSetter.transform.rotation.y, rotationSetter.transform.rotation.z);
        //this.transform.position = Vector3.Lerp(transform.position, goalPos, speed * Time.deltaTime);
    }

    private void Attack()
    {
        begunRerouting = false;
        flyerAgent.SetDestination(flyerAgent.transform.position);
        projectileShooter.transform.LookAt(new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z));

        RaycastHit hit;
        if (Physics.Raycast(projectileShooter.transform.position, projectileShooter.transform.forward, out hit, 100f))
        {
            //print(hit.collider.gameObject.layer);
            if (hit.collider.gameObject.layer != 3)
            {
                inSights = false;
            }
            else
            {
                inSights = true;
            }
            
        }

        //Projectile cooldown
        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenFiring)
            {
                canFire = true;
                
            }
        }
        this.transform.LookAt(new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z));
        //rotationSetter.transform.LookAt(player.transform.position);
        

        if (canFire && inSights)
        {
            timer = 0;
            canFire = false;
            //Vector3 direction = (this.transform.position - player.transform.position);
            //transform.LookAt(player.transform.position);
            //transform.eulerAngles = new Vector3(rotationSetter.transform.rotation.x, rotationSetter.transform.rotation.y, rotationSetter.transform.rotation.z);
            projectileShooter.transform.LookAt(new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z));

            GameObject newProjectile = Instantiate(projectile, projectileShooter.transform.position, Quaternion.identity);
            newProjectile.GetComponent<Rigidbody>().AddForce(projectileShooter.transform.forward * 80, ForceMode.Impulse);
        }
        
    }

    private IEnumerator PauseFly()
    {
        pauseStarted = true;
        flyerAgent.speed = 0f;
        isStill = true;
        yield return new WaitForSeconds(2f);
        flyPointSet = false;
        isStill = false;
    }

    private bool checkAirObstacles()
    {
        bool obstaclesStoppingMovement = false;
        obstaclesInAir = Physics.CheckSphere(antenna.transform.position, antennaRange, obstacleMask);
        obstaclesAboveFront = Physics.CheckSphere(frontCheck.transform.position, cielingCheckRange, obstacleMask);
        obstaclesAboveRear = Physics.CheckSphere(backCheck.transform.position, cielingCheckRange, obstacleMask);

        if (((obstaclesAboveFront && obstaclesInAir || obstaclesAboveRear && obstaclesInAir) &&  minWasReachedLast) || ((obstaclesAboveFront && (goalPos.y + 3)>this.transform.localPosition.y)  || (obstaclesAboveRear && (goalPos.y + 3) > this.transform.localPosition.y)))
        {
            obstaclesStoppingMovement = true;
            flyerAgent.SetDestination(flyerAgent.transform.position);
            if (begunRerouting == false)
            {
                print("Rerouting");
                Reroute();
            }
        }

        else if ((obstaclesAboveFront || obstaclesAboveRear) && this.transform.localPosition.y > minHeight && begunRerouting == false) 
        {

            breakingForObstacles = true;
            flyerAgent.speed = 0f;
            this.transform.localPosition -= new Vector3(0, Time.deltaTime * vertSpeed, 0);
            breakingForObstacles = true;
            followThrough = false;
            print("something Above!");
            //obstaclesStoppingMovement = true;
        }
        else if (obstaclesInAir && begunRerouting == false)// && goalPos.y-3 < this.transform.position.y)   minWasReachedLast == false
        { 
            //obstaclesStoppingMovement = true;

            if(minWasReachedLast)// || goalPos.y + 3 >= this.transform.position.y)
            {
                this.transform.localPosition += new Vector3(0, Time.deltaTime * vertSpeed, 0);
                breakingForObstacles = true;
                flyerAgent.speed = 0f;
                print("something In Front LOW!");
            }
            else
            {
                this.transform.localPosition -= new Vector3(0, Time.deltaTime * vertSpeed, 0);
                breakingForObstacles = true;
                flyerAgent.speed = 0f;
                print("something In Front HIGH!");
            }
        }
       
       
       
        else
        {
            breakingForObstacles = false;
            flyerAgent.speed = speed;
        }

        return obstaclesStoppingMovement;
    }
    private void Reroute()
    {
        Destroy(lastHoneyPoint.gameObject);
        begunRerouting = true;
        flyPointSet = false;
        flyerAgent.speed = speed;
        breakingForObstacles = false;
    
    }
}
