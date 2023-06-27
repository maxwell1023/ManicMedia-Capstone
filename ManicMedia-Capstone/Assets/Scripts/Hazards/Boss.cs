using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject bossAttackBox;
    [SerializeField] private GameObject bossProjectile;
    [SerializeField] private GameObject projectileGunPoint;
    [SerializeField] private Slider bossHealthSlider;

    [SerializeField] private GameObject startDoor;
    [SerializeField] private GameObject door1;
    [SerializeField] private GameObject door2;

    public static float bossHealth = 1000f;

    private bool meleeCooldownVar = false;
    private bool projectileCooldownVar = false;
    private bool rumbleMode = false;

    private bool disabledAttacks = false;

    private float rumbleTimer = 0;
    private float timeBeforeRumbleStarts = 16;
    private bool rumbleModeLeftMovement = true;

    private float meleeCooldownTime = 2;
    private float projectileCooldownTime = 7;

    // Start is called before the first frame update
    void Start()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if(bossAttackBox == null)
        {
            bossAttackBox = GameObject.Find("BossAttackBox");
        }
        if(bossProjectile == null)
        {
            bossProjectile = GameObject.Find("BossProjectile");
        }
        if(projectileGunPoint == null)
        {
            projectileGunPoint = GameObject.Find("ProjectileGunPoint");
        }
        if(bossHealthSlider == null)
        {
            bossHealthSlider = GameObject.Find("BossHP").GetComponent<Slider>();
        }
        if (startDoor == null)
        {
            startDoor = GameObject.Find("StartDoor");
        }
        if (door1 == null)
        {
            door1 = GameObject.Find("Door1");
        }
        if(door2 == null)
        {
            door2 = GameObject.Find("Door2");
        }


        bossAttackBox.SetActive(false);

        transform.position = new Vector3(startDoor.transform.position.x, 0, startDoor.transform.position.z);
        transform.eulerAngles = new Vector3(0, 180, 0);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 10);
    }

    // Update is called once per frame
    void Update()
    {
        //print(DistanceFromPlayer());

        if(DistanceFromPlayer() >= 50 && !rumbleMode)
        {
            Idle();
        }
        else if(rumbleTimer >= timeBeforeRumbleStarts)
        {
            if(!rumbleMode)
            {
                RumbleModeAttack();
            }
            else
            {
                RumbleModeMovement();
            }
            
        }
        else if(DistanceFromPlayer() <= 30 && !projectileCooldownVar) //Attack distance less then 30 or 31
        {
            if(!disabledAttacks)
            {
                ProjectileAttack();
            }
            
        }
        else if(DistanceFromPlayer() <= 10 && !meleeCooldownVar) //Attack distance less then 10 or 11
        {
            if(!disabledAttacks)
            {
                MeleeAttack();
            }
            
        }
        else if(DistanceFromPlayer() <= 30)
        {
            if(!disabledAttacks)
            {
                transform.LookAt(player.transform.position);
            }

            rumbleTimer += Time.deltaTime;
        }
        
        
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawLine(transform.position, player.transform.position);
    }

    private void Idle()
    {
        if(rumbleMode)
        {

        }
        else
        {
            rumbleTimer = 0;
        }
        
    }

    private void MeleeAttack()
    {
        
        if(!meleeCooldownVar)
        {
            StartCoroutine(MeleeCooldown());
            StartCoroutine(MeleeCleanup());

            transform.LookAt(player.transform.position);
            bossAttackBox.SetActive(true);
            
        }
    }
    

    private void ProjectileAttack()
    {
        if(!projectileCooldownVar)
        {
            StartCoroutine(ProjectileCooldown());
            transform.LookAt(player.transform.position);

            projectileGunPoint.transform.LookAt(player.transform.position);
            GameObject newBossProjectile = Instantiate(bossProjectile, projectileGunPoint.transform.position, Quaternion.identity);

            newBossProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * 80, ForceMode.Impulse);
        }
        
    }

    private void RumbleModeAttack()
    {
        if(rumbleTimer >= timeBeforeRumbleStarts)
        {
            rumbleMode = true;
            disabledAttacks = true;
        }
        
        transform.position = new Vector3(door1.transform.position.x, 0, door1.transform.position.z);
        transform.eulerAngles = new Vector3(0, 270, 0);
        transform.position = new Vector3(transform.position.x - 5, transform.position.y, transform.position.z);
        
    }

    private void RumbleModeMovement()
    {
        if(rumbleModeLeftMovement)
        {
            //Left movement
            transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * 30, Space.World);

            if(transform.position.x <= -80)
            {
                rumbleModeLeftMovement = false;

                transform.position = new Vector3(door2.transform.position.x, 0, door2.transform.position.z);
                transform.eulerAngles = new Vector3(0, -270, 0);
                transform.position = new Vector3(transform.position.x - 5, transform.position.y, transform.position.z);
            }
        }
        else
        {
            //Right movement
            transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime * 30, Space.World);

            if (transform.position.x >= 80)
            {
                rumbleModeLeftMovement = true;

                transform.position = new Vector3(door1.transform.position.x, 0, door1.transform.position.z);
                transform.eulerAngles = new Vector3(0, 270, 0);
                transform.position = new Vector3(transform.position.x + 5, transform.position.y, transform.position.z);
            }
        }

        //Checks if it is time to reset        
        ResetRumbleMode();
    }

    private void ResetRumbleMode()
    {
        rumbleTimer += Time.deltaTime;

        if (rumbleTimer >= 32)
        {
            rumbleTimer = 0;
            rumbleMode = false;
            disabledAttacks = false;

            transform.position = new Vector3(startDoor.transform.position.x, 0, startDoor.transform.position.z);
            transform.eulerAngles = new Vector3(0, 180, 0);
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 10);
        }
    }

    private float DistanceFromPlayer()
    {
        float playerDistance = 0;

        playerDistance = (transform.position - player.transform.position).magnitude;

        return playerDistance;
    }

    IEnumerator MeleeCleanup()
    {
        yield return new WaitForSeconds(1);
        bossAttackBox.SetActive(false);
    }

    IEnumerator MeleeCooldown()
    {
        meleeCooldownVar = true;
        yield return new WaitForSeconds(meleeCooldownTime);
        meleeCooldownVar = false;
    }

    IEnumerator ProjectileCooldown()
    {
        projectileCooldownVar = true;
        yield return new WaitForSeconds(projectileCooldownTime);
        projectileCooldownVar = false;
    }


    public void SubtractBossHealth(float amountToSubtract)
    {
        bossHealth = bossHealth - amountToSubtract;
        if(bossHealth <= 0)
        {
            bossHealth = 0;
        }
        bossHealthSlider.value = bossHealth;
    }

}
