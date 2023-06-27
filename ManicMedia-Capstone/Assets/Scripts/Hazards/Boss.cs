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

    [SerializeField] private GameObject door1;
    [SerializeField] private GameObject door2;

    public static float bossHealth = 1000f;

    private bool meleeCooldownVar = false;
    private bool projectileCooldownVar = false;
    private bool rumbleMode = false;

    private bool disabledAttacks = false;

    private float rumbleTimer = 0;
    private float timeBeforeRumbleStarts = 2;

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
        if(door1 == null)
        {
            door1 = GameObject.Find("Door1");
        }
        if(door2 == null)
        {
            door2 = GameObject.Find("Door2");
        }


        bossAttackBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //print(DistanceFromPlayer());


        if(DistanceFromPlayer() >= 50)
        {
            Idle();
        }
        else if(rumbleTimer >= timeBeforeRumbleStarts)
        {
            RumbleModeAttack();
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
                rumbleTimer += Time.deltaTime;
            }
            
            
        }
        
        
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawLine(transform.position, player.transform.position);
    }

    private void Idle()
    {
        rumbleTimer = 0;
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

            //projectileGunPoint.transform.LookAt(player.transform.position);
            GameObject newBossProjectile = Instantiate(bossProjectile, projectileGunPoint.transform.position, Quaternion.identity);

            newBossProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * 80, ForceMode.Impulse);
        }
        
    }

    private void RumbleModeAttack()
    {
        rumbleTimer = 0;
        disabledAttacks = true;


        transform.position = new Vector3(door1.transform.position.x, door1.transform.position.y, door1.transform.position.z);
        transform.eulerAngles = new Vector3(0, 270, 0);
        transform.position = new Vector3(transform.position.x + 10, transform.position.y, transform.position.z);

        transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime, Space.World);
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
        yield return new WaitForSeconds(2);
        meleeCooldownVar = false;
    }

    IEnumerator ProjectileCooldown()
    {
        projectileCooldownVar = true;
        yield return new WaitForSeconds(8);
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
