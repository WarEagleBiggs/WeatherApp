using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Animations;
using Random = UnityEngine.Random;


public class MainMenu : MonoBehaviour
{
    public GameObject difficultySelect;
    public GameObject tutorial;
    public GameObject about;
    public GameObject initButtons;

    public Animator Aubie;

    private void Start()
    {
        difficultySelect.SetActive(false);
        tutorial.SetActive(false);
        about.SetActive(false);
        initButtons.SetActive(true);

        StartCoroutine(AubieAnim());
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

    public void Back()
    {
        //reset buttons
        difficultySelect.SetActive(false);
        tutorial.SetActive(false);
        about.SetActive(false);
        initButtons.SetActive(true);
    }
    
    
    //buttons for diff select
    public void Diff_Easy()
    {
        SceneManager.LoadScene("Game");
        Singleton.GetInstance.currMode = 0;
    }
    public void Diff_Normal()
    {
        SceneManager.LoadScene("Game");
        Singleton.GetInstance.currMode = 1;
    }
    public void Diff_Hard()
    {
        SceneManager.LoadScene("Game");
        Singleton.GetInstance.currMode = 2;
    }

    public IEnumerator AubieAnim()
    {
        while (true)
        {
            int randTime = Random.Range(7, 15);
            yield return new WaitForSeconds(randTime);
            int rand = Random.Range(0, 2);
            if (rand == 1)
            {
                Aubie.SetTrigger("Peek");
            }
            else
            {
                Aubie.SetTrigger("Show");
            }
        }
        
    }
}
