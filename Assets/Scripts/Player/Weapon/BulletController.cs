using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    Rigidbody2D _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        var mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var direction = mouseWorldPosition - transform.position;
        direction.z = 0;
        transform.up = direction;
    }

    void FixedUpdate()
    {
        // Convert it to Vector2.
        var up = new Vector2(transform.up.x, transform.up.y);
        _rigidbody.MovePosition(_rigidbody.position + (up * moveSpeed * Time.fixedDeltaTime));
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "Walls")
        {
            Destroy(gameObject);
        }
    }
}
