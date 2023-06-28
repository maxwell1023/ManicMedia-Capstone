using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    [SerializeField]
    private GameObject hitbox, gun;
    private bool canAttack = true;

    [SerializeField]
    private float meleeCoolDown = .7f;
    public int playerMelee = 70;
    private bool gunInUse;
    // Start is called before the first frame update
    void Start()
    {
        hitbox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            gunInUse = !gunInUse;
        }

        if (Input.GetKeyDown(KeyCode.R) && canAttack && gunInUse == false && this.gameObject.GetComponent<PhysicsGun>().isHolding == false)
        {
            gun.GetComponent<GunAnimation>().Melee();
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        canAttack = false;
        yield return new WaitForSeconds(.385f);
        hitbox.SetActive(true);
        yield return new WaitForSeconds(.17f);
        hitbox.SetActive(false);
        yield return new WaitForSeconds(meleeCoolDown);
        canAttack = true;
    }

    
}
