using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BasicSpiderScript : MonoBehaviour
{
    public float _speed = 1f;
    private Rigidbody _rigidbody;
    // Start is called before the first frame update

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        transform.position += Vector3.forward * Time.deltaTime * _speed;

    }
}
