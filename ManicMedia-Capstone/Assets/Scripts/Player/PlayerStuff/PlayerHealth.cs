using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    [SerializeField] private AudioManager playerAudio;

    [SerializeField]
    private int maxHealth;
    private Vector3 respawnPoint;
    private Quaternion respawnRotation;

    private bool inWaste, deathSoundPlayed;
    private float timeInWaste;
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
        if (inWaste == true)
        {
            health -= (Time.deltaTime * 30);
        }

        healthSlider.value = health;
       
        if (health <= 0 && playerHasDied == false)
        {
            dead = true;
            playerHasDied = true;
        }

        if (dead)
        {
            if(!deathSoundPlayed) { playerAudio.Play("Player Death Sound"); deathSoundPlayed = true; }
            if (blackScreen.alpha < 1 && startFading == false)
            {
                blackScreen.alpha += Time.deltaTime * deathTextIn;
            }
            if (deathText.alpha < 1 && startFading == false)
            {
                deathText.alpha += Time.deltaTime * deathTextIn;
                Vector3 deathTextScaler = new Vector3(1, 1, 1);
                deathText.transform.localScale += Time.deltaTime * deathTextScaler * deathTextSizeSpeed;
            }

            if (deathText.alpha >= 1 || blackScreen.alpha >= 1)
            {
                health = maxHealth;
                //this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                this.gameObject.transform.position = respawnPoint;
                this.gameObject.transform.rotation = respawnRotation;
               // this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                print(respawnPoint);
                startFading = true;
                playerHasDied = false;
            }

            if (deathText.alpha > 0 && startFading)
            {
                deathSoundPlayed = false;
                deathText.alpha -= Time.deltaTime * deathTextFade;
            }
            if (blackScreen.alpha > 0 && startFading)
            {
                deathSoundPlayed = false;
                blackScreen.alpha -= Time.deltaTime * fadeOutSpeed;
            }

            else if (startFading && deathText.alpha <= 0)
            {
                dead = false; startFading = false;
                deathText.transform.localScale = deathTextStartSize;
            }

        }
    }

    public void PlayerHit(float damagePoints)
    {
        health -= damagePoints;
        if (health < 0)
        {
            health = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "checkpoint")
        {
            respawnPoint = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
            print(respawnPoint);
            respawnRotation = this.gameObject.transform.rotation;
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.tag == "instantDeath")
        {

            inWaste = true;
            playerAudio.Play("Toxic Damage");
        }
        if (other.gameObject.tag == "spiderHit")
        {
            PlayerHit(10);
        }
        if (other.gameObject.tag == "Exit")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        if (other.gameObject.tag == "Bullet")
        {
            PlayerHit(20);
            Destroy(other.gameObject);
        }




    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "instantDeath")
        {
            inWaste = false;
            playerAudio.Stop("Toxic Damage");
        }
    }
}
