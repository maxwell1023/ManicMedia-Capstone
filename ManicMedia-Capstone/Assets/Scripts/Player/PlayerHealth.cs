using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    public int health;

    [SerializeField]
    private Slider healthSlider;
    [SerializeField]
    private int maxHealth;
    // Start is called before the first frame update
    void Start()
    {
        healthSlider.maxValue = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = health;
    }
}
