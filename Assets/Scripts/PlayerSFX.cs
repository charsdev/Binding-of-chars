using Assets.Scripts;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private SpriteRenderer _head, _body;
    [SerializeField] private Health _health;

    private void Start()
    {
        //_health.OnDamage.AddListener(_ => StopAllCoroutines());
        //_health.OnDamage.AddListener(Knockback);
        _health.OnDamage.AddListener(_ => Flash());
    }

    private void OnDestroy()
    {
        _health.OnDamage.RemoveAllListeners();
    }

    public void Flash()
    {
        StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        for (int i = 0; i < 3; i++)
        {
            _head.color = Color.clear;
            _body.color = Color.clear;
            yield return new WaitForSeconds(0.01f);
            _head.color = Color.white;
            _body.color = Color.white;
        }
    }

}