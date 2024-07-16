using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] int _maxHealth;
    [SerializeField] GameObject _dropPrefab;


    bool _needsMove;
    int _health;

    //
    // Unity events.
    //

    void Awake()
    {
        _needsMove = true;
        _health = _maxHealth;
    }

    void Update()
    {
        if (_needsMove == true)
        {
            StartCoroutine(MoveSideToSide());
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player Bullet"))
        {
            _health -= col.gameObject.GetComponent<BulletController>().damage;

            if (_health <= 0)
            {
                Instantiate(_dropPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }

            Destroy(col.gameObject);
        }
    }

    //
    // Coroutines.
    //

    IEnumerator MoveSideToSide()
    {
        yield return MoveThisTo(transform.position + new Vector3(2, 0, 0), 2);
        yield return MoveThisTo(transform.position + new Vector3(-4, 0, 0), 4);
        yield return MoveThisTo(transform.position + new Vector3(2, 0, 0), 2);
    }

    IEnumerator MoveThisTo(Vector2 newPos, float seconds)
    {
        _needsMove = false;
        float elapsedSeconds = 0;

        // Save pos when coroutine starts.
        Vector2 start = transform.position;

        while (elapsedSeconds <= seconds)
        {
            transform.position = Vector3.Lerp(start, newPos, elapsedSeconds / seconds);
            elapsedSeconds += Time.deltaTime;
            yield return null;
        }

        _needsMove = true;
    }
}
