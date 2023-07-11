using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{

    public bool stillAlive;
    public bool sawPlayer;

    // Start is called before the first frame update
    void Start()
    {
        stillAlive = true;
        sawPlayer = false;
    }

    
}
