using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject difficultySelect;
    public GameObject tutorial;
    public GameObject about;
    public GameObject initButtons;

    private void Start()
    {
        difficultySelect.SetActive(false);
        tutorial.SetActive(false);
        about.SetActive(false);
        initButtons.SetActive(true);
    }

    public void StartGame()
    {
        difficultySelect.SetActive(true);
        tutorial.SetActive(false);
        about.SetActive(false);
        initButtons.SetActive(false);
    }

    public void Tutorial()
    {
        difficultySelect.SetActive(false);
        tutorial.SetActive(true);
        about.SetActive(false);
        initButtons.SetActive(false);
    }

    public void About()
    {
        difficultySelect.SetActive(false);
        tutorial.SetActive(false);
        about.SetActive(true);
        initButtons.SetActive(false);
    }
    
    
    //buttons for diff select
    public void Diff_Easy()
    {
        SceneManager.LoadScene("Game");
    }
    public void Diff_Normal()
    {
        SceneManager.LoadScene("Game");
    }
    public void Diff_Hard()
    {
        SceneManager.LoadScene("Game");
    }
}
