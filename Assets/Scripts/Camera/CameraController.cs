using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float _followSpeed;
    [SerializeField] Transform _targetToFollow;

    void Update()
    {
        var newPos = new Vector3(_targetToFollow.position.x, _targetToFollow.position.y, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, _followSpeed * Time.deltaTime);
    }
}
