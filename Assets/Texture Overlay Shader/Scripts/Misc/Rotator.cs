using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 _angularVelocity;
    [SerializeField] private Space _space = Space.Self;

    private void Update()
    {
        transform.Rotate(_angularVelocity * Time.deltaTime, _space);
    }
}
