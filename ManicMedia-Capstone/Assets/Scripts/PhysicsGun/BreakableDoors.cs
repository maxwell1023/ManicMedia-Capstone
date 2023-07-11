using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableDoors : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject brokenDoor;
   public void shatterDoor()
    {
        GameObject brokenObject = Instantiate(brokenDoor, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }
}
