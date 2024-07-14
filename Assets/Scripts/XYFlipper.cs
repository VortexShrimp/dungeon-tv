using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XYFlipper : MonoBehaviour
{
    [SerializeField] float timeBetweenCyclesSeconds;

    SpriteRenderer _spriteRenderer;
    bool _needsCycle;
    bool _flip;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _needsCycle = true;
        _flip = false;
    }

    void Update()
    {
        if (_needsCycle == true)
        {
            _spriteRenderer.flipX = _flip;
            _flip = !_flip;

            StartCoroutine(SpriteFlipCooldown());
        }
    }

    IEnumerator SpriteFlipCooldown()
    {
        _needsCycle = false;

        float elapsedSeconds = 0;

        while (elapsedSeconds < timeBetweenCyclesSeconds)
        {
            elapsedSeconds += Time.deltaTime;
            yield return null;
        }

        _needsCycle = true;
    }
}
