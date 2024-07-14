using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIIngameController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _coinTextMesh;
    int _coinCount;

    void Awake()
    {
        _coinCount = 0;
        _coinTextMesh.text = _coinCount + "";
    }

    void OnEnable()
    {
        EnemyDropController.OnDropCollected += OnDropCollected;
    }

    void OnDisable()
    {
        EnemyDropController.OnDropCollected -= OnDropCollected;
    }

    void OnDropCollected()
    {
        _coinCount += 1;
        _coinTextMesh.text = _coinCount + "";
    }
}
