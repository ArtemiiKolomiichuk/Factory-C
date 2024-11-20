using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject menuObject;

    public GameObject recipeBook;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuObject.SetActive(!menuObject.activeInHierarchy);
            //Time.timeScale = menuObject.activeInHierarchy ? 0f : 1f;
        }



        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OpenRecipes();
        }
    }

    public void OpenRecipes()
    {
        recipeBook.SetActive(!recipeBook.activeInHierarchy);
    }

    public void ResumeGame()
    {
        menuObject.SetActive(false);
        //Time.timeScale = 1f;
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }



    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
