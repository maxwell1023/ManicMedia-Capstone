using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{

    private bool optionsIsActive = false;
    private Animator optionsAnimator;

    [Header("Fullscreen")]
    [SerializeField] private Toggle fullscreenToggle;

    [Header("Audio")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private AudioMixer sfxMixer;

    [Header("Sensitivity")]
    [SerializeField] private TextMeshProUGUI sensitivityText;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private PlayerCamera playerCamera;
    

    //Default Values
    static private bool isFullscreen = false;
    static private float musicVolume = .5f;
    static private float sfxVolume = .5f;
    static private float mouseSensitivity = 1f;

    // Start is called before the first frame update
    void Start()
    {
        optionsAnimator = GetComponent<Animator>();
        if(playerCamera == null)
        {
            playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerCamera>();
        }

        //Set the fullscreen toggle to the correct color
        fullscreenToggle.isOn = isFullscreen;

        //Set the volume
        musicMixer.SetFloat("Volume", Mathf.Log10(musicVolume) * 20);
        sfxMixer.SetFloat("Volume", Mathf.Log10(sfxVolume) * 20);
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;

        //Set the sensitivity
        sensitivitySlider.value = mouseSensitivity;
        sensitivityText.text = "Mouse Sensitivity: " + mouseSensitivity.ToString("F2");
        if(SceneManager.GetActiveScene().name != "Menu")
        {
            playerCamera.camSensityX = 400 * mouseSensitivity;
            playerCamera.camSensityY = 400 * mouseSensitivity;
        }
        

        //Options UI doesn't show up in editor unless the game object is turned off and on for some reason??
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            ActivateOptionsMenu();
        }
    }

    //Options Menu
    public void FullscreenMode(bool setFullscreen)
    {
        if (setFullscreen)
        {
            //Becomes fullscreen
            Screen.fullScreen = setFullscreen;
            isFullscreen = setFullscreen;
        }
        else
        {
            //Becomes windowed
            Screen.fullScreen = setFullscreen;
            isFullscreen = setFullscreen;
        }

    }

    public void ActivateOptionsMenu()
    {
        if (optionsIsActive)
        {
            //Close
            optionsIsActive = false;
            optionsAnimator.SetBool("isActive", optionsIsActive);
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
        }
        else
        {
            //Open
            optionsIsActive = true;
            optionsAnimator.SetBool("isActive", optionsIsActive);
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0f;
        }

        
        if(SceneManager.GetActiveScene().name == "Menu")
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        
    }

    public void SetMusicVolume(float sliderValue)
    {
        musicMixer.SetFloat("Volume", Mathf.Log10(sliderValue) * 20);
        musicVolume = sliderValue;
    }

    public void SetSFXVolume(float sliderValue)
    {
        sfxMixer.SetFloat("Volume", Mathf.Log10(sliderValue) * 20);
        sfxVolume = sliderValue;
    }

    public void ChangeMouseSensitivity(float sensitivity)
    {
        //Vector2 mouseMovement = new Vector2(Input.GetAxisRaw("Mouse X") * sensitivity, Input.GetAxisRaw("Mouse Y") * sensitivity);
        sensitivityText.text = "Mouse Sensitivity: " + sensitivity.ToString("F2");
        mouseSensitivity = sensitivity;

        if (SceneManager.GetActiveScene().name != "Menu")
        {
            playerCamera.camSensityX = 400 * mouseSensitivity;
            playerCamera.camSensityY = 400 * mouseSensitivity;
        }
        

    }
}
