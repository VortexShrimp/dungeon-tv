using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuButtons : MonoBehaviour
{
    public void OnStartClicked()
    {
        SceneManager.LoadSceneAsync("Dungeon 1", LoadSceneMode.Single);
    }

    public void OnExitClicked()
    {
        Application.Quit();
    }
}
