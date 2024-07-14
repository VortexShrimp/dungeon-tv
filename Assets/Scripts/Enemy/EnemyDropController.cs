using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDropController : MonoBehaviour
{
    public delegate void DropCollectedHandler();
    public static DropCollectedHandler OnDropCollected;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") == true)
        {
            OnDropCollected?.Invoke();
            Destroy(gameObject);
        }
    }
}
