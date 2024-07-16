using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    
    [SerializeField] Transform _parentTransform;
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] float _firerateSeconds;
    bool _canFire = true;
    SpriteRenderer _weaponSpriteRenderer;
    
    bool _isPaused = false;
    void Awake()
    {
        _weaponSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (_isPaused == false)
        {
            MoveGun();
            HandleFiring();
        }
        
    }

    // Make gun aim at crosshair & offset gun from player
    void MoveGun() 
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        double angle = (Math.Atan2(transform.parent.position.y - mousePos.y, transform.parent.position.x - mousePos.x) * (180 / Math.PI)) - 180;
        transform.rotation = Quaternion.Euler(0, 0, (float)angle);

        // Flips sprite accordingly
        _weaponSpriteRenderer.flipY = angle >= -270 && angle <= -90;

        // Position Code
        double gunPosX = Math.Cos(angle * Math.PI / 180)/1.3;
        double gunPosY = Math.Sin(angle * Math.PI / 180)/1.3 - 0.1;

        transform.position = new Vector2((float)gunPosX + transform.parent.position.x,(float)gunPosY + transform.parent.position.y);
    }
    void OnEnable()
    {
        UIController.OnGamePause += OnGamePause;
        PauseMenuButtons.OnResumeButtonClicked += OnResumeButtonClicked;
    }
    void OnDisable()
    {
        UIController.OnGamePause -= OnGamePause;
        PauseMenuButtons.OnResumeButtonClicked -= OnResumeButtonClicked;
    }
    void OnGamePause(bool paused)
    {
        _isPaused = paused;
    }

    void OnResumeButtonClicked()
    {
        _isPaused = false;
    }

    void HandleFiring()
    {
        if (Input.GetKey(KeyCode.Mouse0) == true)
        {
            if (_canFire == true)
            {
                Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
                StartCoroutine(WaitForNextShot());
            }
        }
    }
    //
    // Coroutines.
    //

    // Wait a certain amount of seconds before flipping _canFire.
    IEnumerator WaitForNextShot()
    {
        _canFire = false;
        float elapsedSeconds = 0;
        while (elapsedSeconds < _firerateSeconds)
        {
            elapsedSeconds += Time.deltaTime;
            yield return null;
        }
        _canFire = true;
    }
}
