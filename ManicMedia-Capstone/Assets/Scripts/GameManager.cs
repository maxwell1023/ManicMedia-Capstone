using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private AudioSource musicAudio;
    [SerializeField] private AudioSource sfxAudio;

    public static GameManager instance;

    public enum GameState
    {
        Menu,
        Paused,
        Playing
    }

    public GameState gameState { get; set; }

    private void Start()
    {
        instance = this;
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


    }

}
