using System;
using UnityEngine;

[ExecuteInEditMode]
public class Lookat : MonoBehaviour
{
    [SerializeField] private Transform _target;
    private void Update()
    {
        if (_target)
        {
            transform.LookAt(_target);
        }
    }
}
