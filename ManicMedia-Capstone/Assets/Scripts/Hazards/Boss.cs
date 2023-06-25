using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject bossAttackBox;
    [SerializeField] private GameObject bossProjectile;
    [SerializeField] private GameObject projectileGunPoint;

    public static int bossHealth = 0;

    private bool meleeCooldownVar = false;
    private bool projectileCooldownVar = false;
    

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
        else if(DistanceFromPlayer() <= 30 && !projectileCooldownVar) //Attack distnace less then 30 or 31
        {
            ProjectileAttack();
        }
        else if(DistanceFromPlayer() <= 10 && !meleeCooldownVar) //Attack distance less then 10 or 11
        {
            MeleeAttack();
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, player.transform.position);
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

    private void Idle()
    {

    }

    private void ProjectileAttack()
    {
        //projectileGunPoint.transform.LookAt(player.transform.position);
        if(!projectileCooldownVar)
        {
            StartCoroutine(ProjectileCooldown());
            transform.LookAt(player.transform.position);
            GameObject newBossProjectile = Instantiate(bossProjectile, projectileGunPoint.transform.position, Quaternion.identity);
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
        yield return new WaitForSeconds(2);
        meleeCooldownVar = false;
    }

    IEnumerator ProjectileCooldown()
    {
        projectileCooldownVar = true;
        yield return new WaitForSeconds(8);
        projectileCooldownVar = false;
    }

}
