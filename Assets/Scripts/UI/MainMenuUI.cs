using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private string startScene = "World 1";

    public void PlayGame()
    {
        SceneManager.LoadScene(startScene);
    }

    public void QuitGame()
    {
        Debug.Log("Quit the game.");
        Application.Quit();
    }
}
