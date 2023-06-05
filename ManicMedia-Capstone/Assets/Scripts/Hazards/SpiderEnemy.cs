using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderEnemy : MonoBehaviour
{
    [SerializeField] UnityEngine.AI.NavMeshAgent spiderAgent; //NavMesh Agent that controls movement options for this enemy
    [SerializeField] Transform player;                       //Player's transform (to be followed within reason)
    [SerializeField] LayerMask groundMask, playerMask;      //Masks to look for places to wal towards
    private Vector3 walkPoint;                             //Position to walk towards
    bool walkPointSet;                                    //Has the position above been set to something new?
    [SerializeField] private float walkRange;            //Range from the spawner the spider will usually stay within
    [SerializeField] private float attackCooldown;      //How long to wait between attacks
    private bool justAttacked;                         //Do we still need to wait between attacks for that cooldown?\

    public float followRange, attackRange, attackTime;                     //How close the player must be to chase, how close they must be to attack, and how long we must attack for
    [SerializeField] private bool linkedToSpawner, inSight, inRange;      //Is the player in sight? Are they within Range?

    public bool isAlive = true;                                         //Is the spider alive?

    [SerializeField] private GameObject hitBox;                       //Enemy Hitbox
    [SerializeField] private int sEnemyHealth = 100;                 //Spider enemy's health  
    [SerializeField] private GameObject spawner;                    //Where the spider Spawns from

    void Awake()
    {
        hitBox.SetActive(false);
        player = GameObject.FindWithTag("player").transform;
        spiderAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        isAlive = true;

        if(linkedToSpawner == true)
        {
            this.gameObject.transform.position = spawner.gameObject.transform.position;
        }
        this.gameObject.transform.position = spawner.gameObject.transform.position;
        //   startLocation = this.gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive == true)
        {
            //Checks for player range..
            inSight = Physics.CheckSphere(transform.position, followRange, playerMask);
            inRange = Physics.CheckSphere(transform.position, attackRange, playerMask);
            //...and decides where to move

                if (!inSight)  
                {
                    Scatter();
                }
                if (inSight && !inRange)
                {
                    Chase();
                }
                if (inRange)
                {
                    Attack();
                }
            
        }
    }

    private void Scatter()
    {
        if (!walkPointSet)
        {
            RandomizeWalk();
        }

        if (walkPointSet)
        {
            spiderAgent.SetDestination(walkPoint);
        }
        Vector3 distanceToPoint = transform.position - walkPoint;

        if (distanceToPoint.magnitude < 1f)
        {
            StartCoroutine(PauseWalk());
        }
    }
    private void RandomizeWalk()
    {
        float randomVert = Random.Range(-walkRange, walkRange);
        float randomHoriz = Random.Range(-walkRange, walkRange);

        walkPoint = new Vector3(spawner.transform.position.x + randomHoriz, transform.position.y, spawner.transform.position.z + randomVert);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundMask))
        {
            walkPointSet = true;
        }
    }
    private void Attack()
    {
        spiderAgent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!justAttacked)
        {
            hitBox.SetActive(true);
            Invoke("hitBoxVanish", attackTime);



            justAttacked = true;
            Invoke("WaitForAttack", attackCooldown); //honestly just learned this, will be using coroutine a lot less in the future!

        }

    }

    //private void takeDamage;
    private void hitBoxVanish()
    {
        hitBox.SetActive(false);
    }
    private void WaitForAttack()
    {
        justAttacked = false;
    }


    private void Chase()
    {
        spiderAgent.SetDestination(player.position);
    }

    private IEnumerator PauseWalk()
    {
        yield return new WaitForSeconds(3f);
        walkPointSet = false;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "playerslash")
        {
            sEnemyHealth = sEnemyHealth - player.gameObject.GetComponent<PlayerMovement>().playerMelee;
            if (sEnemyHealth <= 0)
            {
                this.gameObject.transform.position = spawner.gameObject.transform.position;
                //this.gameObject.transform.rotation = startRotation;       
                this.gameObject.SetActive(false);
                isAlive = false;
                //need to FIX
            }
        }
    }
}
