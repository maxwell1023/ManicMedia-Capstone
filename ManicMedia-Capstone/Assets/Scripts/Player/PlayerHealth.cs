using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    public float health;

    [SerializeField]
    private Slider healthSlider;
    [SerializeField]
    private float fadeInSpeed, fadeOutSpeed, blackScreenDelay, deathTextDelay, deathTextSizeSpeed;
    [SerializeField]
    private CanvasGroup blackScreen, deathText;

    private bool playerHasDied;
    private Vector3 deathTextStartSize;

    [SerializeField]
    private int maxHealth;
    private Transform respawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        healthSlider.maxValue = maxHealth;
        respawnPoint = this.transform;
        blackScreen.alpha = 0;
        deathText.alpha = 0;
        deathTextStartSize = deathText.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = health;

        if(health<=0 && playerHasDied == false)
        {
            OnDeath();
            playerHasDied = true;
        }
    }

    public void PlayerHit(float damagePoints)
    {
        health -= damagePoints;
        if(health<0)
        {
            health = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "checkpoint")
        {
            respawnPoint = other.gameObject.transform;
        }
        if(other.gameObject.tag == "instantDeath")
        {
            OnDeath();
                
        }
    }

    private void OnDeath()
    {
        while (blackScreen.alpha < 1)
        {
            blackScreen.alpha += Time.deltaTime * fadeInSpeed;
        }
        this.gameObject.transform.position = respawnPoint.transform.position;
        this.gameObject.transform.rotation = respawnPoint.transform.rotation;
        health = maxHealth;
        while (deathText.alpha < 1)
        {
            deathText.alpha += Time.deltaTime * fadeInSpeed;
            Vector3 deathTextScaler = new Vector3(1, 1, 1);
            deathText.transform.localScale += Time.deltaTime * deathTextScaler * deathTextSizeSpeed;
        }
        Invoke("DropDeathText", deathTextDelay);
   
        playerHasDied = false;

    }

    private void DropDeathText()
    {
        while (deathText.alpha > 0)
        {
            deathText.alpha -= Time.deltaTime * fadeOutSpeed;
        }
        deathText.transform.localScale = deathTextStartSize;
        Invoke("FadeIn", blackScreenDelay);
    }


    private void FadeIn()
    {
        while (blackScreen.alpha > 0)
        {
            blackScreen.alpha -= Time.deltaTime * fadeOutSpeed;
        }
    }
}
