using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SpiderEnemy : MonoBehaviour
{
    [SerializeField] UnityEngine.AI.NavMeshAgent spiderAgent; //NavMesh Agent that controls movement options for this enemy
    [SerializeField] Transform player;                       //Player's transform (to be followed within reason)
    [SerializeField] LayerMask groundMask, playerMask;      //Masks to look for places to wal towards
    private Vector3 walkPoint;                             //Position to walk towards
    private bool walkPointSet;                                    //Has the position above been set to something new?
    [SerializeField] private float walkRange;            //Range from the spawner the spider will usually stay within
    [SerializeField] private float attackCooldown;      //How long to wait between attacks
    private bool justAttacked;                         //Do we still need to wait between attacks for that cooldown?\

    public float followRange, attackRange, attackTime;                     //How close the player must be to chase, how close they must be to attack, and how long we must attack for
    [SerializeField] private bool linkedToSpawner, inSight, inRange;      //Is the player in sight? Are they within Range?

    public bool isAlive = true;                                         //Is the spider alive?
    public bool hasSeenPlayer = false;

    [SerializeField] private GameObject hitBox;                       //Enemy Hitbox
    public float sEnemyHealth = 100;                 //Spider enemy's health  
    [SerializeField] private GameObject spawner;                    //Where the spider Spawns from
    private Transform startLocation;
    [SerializeField] private float defaultSpeed, chaseSpeed;
    private bool isPaused, hasPaused; //temporary test bool
    private AudioSource zap;
    void Awake()
    {
        zap = GetComponent<AudioSource>();
        hitBox.SetActive(false);
        player = GameObject.FindWithTag("Player").transform;
        spiderAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        //spiderAgent.speed = defaultSpeed;
        isAlive = true;

        if(linkedToSpawner == true)
        {
            this.gameObject.transform.position = spawner.gameObject.transform.position;
        }
        startLocation = this.gameObject.transform;
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

                if (inSight == false && inRange == false)  
                {
                    Scatter();
                }
                else if (inSight == true && inRange == false)
                {
                    Chase();
                    hasSeenPlayer = true;
                }

                if (inRange)
                {
                    Attack();
                } 
            
        }

        if(sEnemyHealth <= 0) 
        {
            this.gameObject.GetComponent<EnemyStats>().stillAlive = false;
            isAlive = false;
            this.gameObject.SetActive(false);
        }

        this.gameObject.GetComponent<EnemyStats>().sawPlayer = hasSeenPlayer;
    }

    private void Scatter()
    {
        if (!walkPointSet && isPaused == false)
        {
           // spiderAgent.speed = defaultSpeed;
            RandomizeWalk();
        }

        if (walkPointSet && isPaused == false)
        {
            spiderAgent.speed = defaultSpeed;
            hasPaused = false;
            spiderAgent.SetDestination(walkPoint);
            
        }
        Vector3 distanceToPoint = transform.position - walkPoint;

        if (distanceToPoint.magnitude < 2f && hasPaused == false)
        {
            StartCoroutine(PauseWalk());
        }
    }
    private void RandomizeWalk()
    {
        float randomVert = Random.Range(-walkRange, walkRange);
        float randomHoriz = Random.Range(-walkRange, walkRange);
        
        walkPoint = new Vector3(transform.localPosition.x + randomHoriz, transform.localPosition.y + 1, transform.localPosition.z + randomVert);

        if (Physics.Raycast(walkPoint, -transform.up, 3f, groundMask))
        {
            walkPointSet = true;
            
        }
    }
    private void Attack()
    {
        spiderAgent.SetDestination(transform.position);
        transform.LookAt(new Vector3(player.position.x, this.transform.position.y, player.position.z));


        if (!justAttacked)
        {
            hitBox.SetActive(true);
            zap.Play(0);
            Invoke("hitBoxVanish", attackTime);



            justAttacked = true;
            Invoke("WaitForAttack", attackCooldown); 

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
        spiderAgent.speed = chaseSpeed;
        spiderAgent.SetDestination(player.position);
    }

    private IEnumerator PauseWalk()
    {
        spiderAgent.speed = 0f;
        isPaused = true;
        hasPaused = true;
        float randomPause = Random.Range(3, 10);
        yield return new WaitForSeconds(randomPause);
        isPaused = false;
        walkPointSet = false;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "PlayerMelee")
        {
            sEnemyHealth = sEnemyHealth - player.gameObject.GetComponent<PlayerController>().playerMelee;
            
        } 
    }
}
