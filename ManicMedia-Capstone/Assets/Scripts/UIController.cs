using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{

    private bool optionsIsActive = false;
    [SerializeField] private Animator optionsAnimator;

    static private bool isFullscreen = false;


    private void Start()
    {
        
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

    public void FullscreenMode()
    {
        if(isFullscreen)
        {
            //Not fullscreen
            isFullscreen = false;
            Screen.fullScreen = isFullscreen;
            
        }
        else
        {
            //Fullscreen it
            isFullscreen = true;
            Screen.fullScreen = isFullscreen;
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

    


}
