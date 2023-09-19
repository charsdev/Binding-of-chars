using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _destroySfx;
    [SerializeField] private Transform _originSfx;
    private void Start()
    {
        Destroy(gameObject, _animator.GetCurrentAnimatorStateInfo(0).length);
    }

    private void OnDestroy()
    {
        if (_destroySfx != null)
        {
            Instantiate(_destroySfx, _originSfx.position, Quaternion.identity);
        }
    }
}
