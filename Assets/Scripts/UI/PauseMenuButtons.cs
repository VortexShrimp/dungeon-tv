using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuButtons : MonoBehaviour
{
    public delegate void ResumeButtonClickedHandler();
    public static ResumeButtonClickedHandler OnResumeButtonClicked;

    public void OnResumeClicked()
    {
        OnResumeButtonClicked?.Invoke();
    }

    public void OnMenuClicked()
    {
        SceneManager.LoadSceneAsync("Menu Start");
    }
}
