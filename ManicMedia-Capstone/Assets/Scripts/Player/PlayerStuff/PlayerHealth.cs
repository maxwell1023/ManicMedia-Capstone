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
    private float fadeInSpeed, fadeOutSpeed, deathTextIn, deathTextFade, deathTextSizeSpeed;
    [SerializeField]
    private CanvasGroup blackScreen, deathText;

    private bool playerHasDied;
    private Vector3 deathTextStartSize;

    public bool dead, startFading, playerRespawned;

    [SerializeField]
    private int maxHealth;
    private Vector3 respawnPoint;
    private Quaternion respawnRotation;
    // Start is called before the first frame update
    void Start()
    {
        playerHasDied = false;
        healthSlider.maxValue = maxHealth;
        respawnPoint = this.gameObject.transform.position;
        respawnRotation = this.gameObject.transform.rotation;
        print(respawnPoint); 
        blackScreen.alpha = 0;
        deathText.alpha = 0;
        deathTextStartSize = deathText.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = health;

        if (health <= 0 && playerHasDied == false)
        {
            dead = true;
            playerHasDied = true;
        }

        if (dead)
        {
            if (blackScreen.alpha < 1 && startFading == false)
            {
                blackScreen.alpha += Time.deltaTime * fadeInSpeed;
            }
            if (deathText.alpha < 1 && startFading == false)
            {
                deathText.alpha += Time.deltaTime * deathTextIn;
                Vector3 deathTextScaler = new Vector3(1, 1, 1);
                deathText.transform.localScale += Time.deltaTime * deathTextScaler * deathTextSizeSpeed;
            }

            if (deathText.alpha >= 1 && blackScreen.alpha >= 1)
            {
                health = maxHealth;
                this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                this.gameObject.transform.position = respawnPoint;
                //this.gameObject.transform.rotation = respawnRotation;
                this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                print(respawnPoint);
                startFading = true;
                playerHasDied = false;
            }

            if (deathText.alpha > 0 && startFading)
            {
                deathText.alpha -= Time.deltaTime * deathTextFade;
            }
            if (blackScreen.alpha > 0 && startFading)
            {
                blackScreen.alpha -= Time.deltaTime * fadeOutSpeed;
            }
            
            if(startFading && deathText.alpha <= 0 && blackScreen.alpha <= 0)
            {
                dead = false; startFading = false;
                deathText.transform.localScale = deathTextStartSize;
            }

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
            respawnPoint = other.gameObject.transform.position;
            respawnRotation = other.gameObject.transform.rotation;
            other.gameObject.SetActive(false);
        }
        if(other.gameObject.tag == "instantDeath")
        {

            dead = true;
        }
        if (other.gameObject.tag == "spiderHit")
        {
            PlayerHit(10);
        }

    }

    

   
    
}
