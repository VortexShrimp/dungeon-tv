using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float _moveSpeed;
        [SerializeField] Sprite[] _playerSprites;
        [SerializeField] GameObject _bulletPrefab;
        [SerializeField] SpriteRenderer _weaponSpriteRenderer;
        [SerializeField] Transform _weaponTransform;

        Rigidbody2D _rigidbody;
        SpriteRenderer _spriteRenderer;
        Animator _animator;

        bool _canFire;
        bool _isPaused;

        //
        //  Unity events.
        //

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();

            _canFire = true;
            _isPaused = false;
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

        void Update()
        {
            if (_isPaused == false)
            {
                HandleFiring();
            }
        }

        void FixedUpdate()
        {
            HandleMovement();
            MoveGun();
        }

        //
        // Private class methods.
        //

        // Make gun aim at crosshair & offset gun from player
        void MoveGun()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            double angle = (Math.Atan2(_rigidbody.position.y - mousePos.y, _rigidbody.position.x - mousePos.x) * (180 / Math.PI)) - 180;
            _weaponTransform.rotation = Quaternion.Euler(0, 0, (float)angle);

            // Flips sprite accordingly
            _weaponSpriteRenderer.flipY = angle >= -270 && angle <= -90;

            double gunPosX = Math.Cos(angle * Math.PI / 180);
            double gunPosY = Math.Sin(angle * Math.PI / 180);

            _weaponTransform.position = new Vector2((float)gunPosX + _rigidbody.position.x,(float)gunPosY + _rigidbody.position.y);
        }

        // Move the player & adjust their sprite.
        void HandleMovement()
        {
            var input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            //_rigidbody.MovePosition(_rigidbody.position + input * moveSpeed * Time.fixedDeltaTime);

            var vel = input * _moveSpeed * Time.fixedDeltaTime;
            _rigidbody.velocity = vel;

            // No movement.
            if (vel.magnitude == 0)
            {
                _animator.SetInteger("State", 0);
            }
            else
            {
                if (vel.x != 0)
                {
                    _animator.SetInteger("State", 2);

                    // Moving right.
                    if (vel.x > 0)
                    {
                        _spriteRenderer.flipX = true;
                    }
                    // Moving left.
                    else
                    {
                        _spriteRenderer.flipX = false;
                    }
                }

                if (vel.y != 0)
                {
                    _animator.SetInteger("State", 1);
                }
            }
        }

        void HandleFiring()
        {
            if (Input.GetKey(KeyCode.Mouse0) == true)
            {
                if (_canFire == true)
                {
                    Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
                    StartCoroutine(WaitForNextShot(0.3f));
                }
            }
        }

        //
        // Coroutines.
        //

        // Wait a certain amount of seconds before flipping _canFire.
        IEnumerator WaitForNextShot(float seconds)
        {
            _canFire = false;

            float elapsedSeconds = 0;

            while (elapsedSeconds < seconds)
            {
                elapsedSeconds += Time.deltaTime;
                yield return null;
            }

            _canFire = true;
        }

        //
        // Custom Events
        //

        void OnGamePause(bool paused)
        {
            _isPaused = paused;
        }

        void OnResumeButtonClicked()
        {
            _isPaused = false;
        }
    }
}
