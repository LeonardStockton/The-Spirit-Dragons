using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuControler : MonoBehaviour
{
    [Header("level to load")]
    public string _newGameLevel;
    private string levelToLoad;
    [SerializeField] private GameObject noSaveFile = null;

    public void NewGameDialog()
    {
        SceneManager.LoadScene(_newGameLevel);
    }

    public void LoadGameDialog()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            noSaveFile.SetActive(true);
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }

}
