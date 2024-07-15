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
                        _weaponSpriteRenderer.flipX = false;
                        _spriteRenderer.flipX = true;
                    }
                    // Moving left.
                    else
                    {
                        _weaponSpriteRenderer.flipX = true;
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
