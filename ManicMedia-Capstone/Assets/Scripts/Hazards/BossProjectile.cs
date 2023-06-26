using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private Vector3 goalPos;

    // Start is called before the first frame update
    void Start()
    {
        /*
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        

        goalPos = player.transform.position;
        transform.LookAt(goalPos);
        */

        Invoke("CleanUp", 3);

    }

    private void FixedUpdate()
    {
        /*
        transform.position = Vector3.Lerp(transform.position, goalPos, Time.deltaTime * 3.5f);

        if (Vector3.Distance(transform.position, goalPos) <= 0.1f)
        {
            Destroy(gameObject);
        }
        */
    }

    private void CleanUp()
    {
        Destroy(gameObject);
    }

}
