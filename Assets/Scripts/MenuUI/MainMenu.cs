using System;
using System.Collections;
using System.Collections.Generic;
using Class;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject ContinueButton;

    private void Awake()
    {
        if (FileManager.IsSaveFileExist())
        {
            ContinueButton.SetActive(true);
            transform.position += Vector3.down * 50;
        }
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void NewGame()
    {
        FileManager.DeleteSaveFile();
        SceneManager.LoadScene("Main");
    }
    public void QuitGame()
    {
        Debug.Log("QUIT!");
        // only happen in built project
        Application.Quit();
    }
}
