using Palmmedia.ReportGenerator.Core;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{

    private bool optionsIsActive = false;
    [SerializeField] private Animator optionsAnimator;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private AudioMixer sfxMixer;
    [SerializeField] private TextMeshProUGUI sensitivityText;

    static private bool isFullscreen = false;
    static private float musicVolume = 1;
    static private float sfxVolume = 1;

    private void Start()
    {
        //Set the volume at the begining of the scene
        musicMixer.SetFloat("Volume", Mathf.Log10(musicVolume) * 20);
        sfxMixer.SetFloat("Volume", Mathf.Log10(sfxVolume) * 20);

        fullscreenToggle.isOn = isFullscreen;

        
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MenuButton()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitButton()
    {
        print("Is quitting...");
        Application.Quit();
    }

    private void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ActivateOptionsMenu();
        }


    }


    //Options Menu
    public void FullscreenMode(bool setFullscreen)
    {
        if(setFullscreen)
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
        if(optionsIsActive)
        {
            //Close
            optionsIsActive = false;
            optionsAnimator.SetBool("isActive", optionsIsActive);
        }
        else
        {
            //Open
            optionsIsActive = true;
            optionsAnimator.SetBool("isActive", optionsIsActive);
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
        Vector2 mouseMovement = new Vector2(Input.GetAxisRaw("Mouse X") * sensitivity, Input.GetAxisRaw("Mouse Y") * sensitivity);
        sensitivityText.text = "Mouse Sensitivity: " + sensitivity.
        
    }


}
