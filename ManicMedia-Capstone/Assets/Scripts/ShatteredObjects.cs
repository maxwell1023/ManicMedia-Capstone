using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatteredObjects : MonoBehaviour
{
    [SerializeField] private float stayingTime = 30;
    // Start is called before the first frame update
    void OnEnable()
    {
        Invoke("Cleanup", stayingTime);
    }

    // Update is called once per frame
    private void Cleanup()
    {
        Destroy(this.gameObject);
    }
}
