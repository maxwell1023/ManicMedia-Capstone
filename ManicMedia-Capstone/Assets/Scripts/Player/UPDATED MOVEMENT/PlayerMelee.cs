using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    [SerializeField]
    private GameObject hitbox;
    private bool canAttack = true;

    [SerializeField]
    private float meleeCoolDown = .7f;
    public int playerMelee = 70;

    // Start is called before the first frame update
    void Start()
    {
        hitbox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        if (canAttack == true)
        {
            canAttack = false;
            hitbox.SetActive(true);
            yield return new WaitForSeconds(.3f);
            hitbox.SetActive(false);
            yield return new WaitForSeconds(meleeCoolDown);
            canAttack = true;
        }

    }
}
