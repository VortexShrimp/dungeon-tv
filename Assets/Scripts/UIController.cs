using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public delegate void GamePauseHandler(bool paused);
    public static GamePauseHandler OnGamePause;

    [SerializeField] Canvas _pauseMenuCanvas;

    bool _isPaused;

    //
    // Unity events.
    //

    void Awake()
    {
        // Not sure if this is needed.
        SetPausedState(false);
    }

    void OnEnable()
    {
        PauseMenuButtons.OnResumeButtonClicked += OnResumeButtonClicked;
    }

    void OnDisable()
    {
        PauseMenuButtons.OnResumeButtonClicked -= OnResumeButtonClicked;
    }

    void Update()
    {
        if (_isPaused == false)
        {        
            if (Input.GetKeyDown(KeyCode.Escape) == true)
            {
                SetPausedState(true);

                // Let everyone know.
                OnGamePause?.Invoke(true);
            }
        }
    }

    //
    // Private class methods.
    //

    void SetPausedState(bool paused)
    {
        _isPaused = paused;
        Time.timeScale = paused == true ? 0 : 1;
        _pauseMenuCanvas.gameObject.SetActive(paused);
    }

    //
    // Custom events.
    //

    void OnResumeButtonClicked()
    {
        // HACK: Force resume.
        SetPausedState(false);
    }
}
