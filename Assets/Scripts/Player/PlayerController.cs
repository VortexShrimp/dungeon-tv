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


        Rigidbody2D _rigidbody;
        SpriteRenderer _spriteRenderer;
        Animator _animator;
        bool _isPaused;

        //
        //  Unity events.
        //

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
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

        void FixedUpdate()
        {
            HandleMovement();
        }

        //
        // Private class methods.
        //

        // Move the player & adjust their sprite.
        void HandleMovement()
        {
            var input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            //_rigidbody.MovePosition(_rigidbody.position + input * moveSpeed * Time.fixedDeltaTime);

            var vel = input * _moveSpeed * Time.fixedDeltaTime;
            _rigidbody.velocity = vel;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            double angle = (Math.Atan2(transform.position.y - mousePos.y, transform.position.x - mousePos.x) * (180 / Math.PI)) - 180;
            Debug.Log(angle);
            // No movement.

            if (vel.magnitude == 0)
            {
                if (angle <= -45 && angle > -135) {
                    _animator.SetInteger("State", 0);
                }
                else if (angle <= -135 && angle > -225) {
                    _animator.SetInteger("State", 6);
                    _spriteRenderer.flipX = false;
                }
                else if (angle <= -225 && angle > -315) {
                    _animator.SetInteger("State", 5);
                } 
                else {
                    _animator.SetInteger("State", 6);
                    _spriteRenderer.flipX = true;
                }
            }
            if (vel.magnitude != 0)
            {
                if (angle <= -45 && angle > -135) {
                    _animator.SetInteger("State", 1);
                }
                else if (angle <= -135 && angle > -225) {
                    _animator.SetInteger("State", 2);
                    _spriteRenderer.flipX = false;
                }
                else if (angle <= -225 && angle > -315) {
                    _animator.SetInteger("State", 4);
                } 
                else {
                    _animator.SetInteger("State", 2);
                    _spriteRenderer.flipX = true;
                }
           }
           
            
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
