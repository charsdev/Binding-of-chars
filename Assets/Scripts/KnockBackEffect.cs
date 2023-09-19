using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class KnockBackEffect : MonoBehaviour
{
    [SerializeField] private float _knockbackForce;
    [SerializeField] private float _delay;
    [SerializeField] private bool _enableOnCollisionEnter;
    [SerializeField] private bool _enableOnTriggerEnter;
    [SerializeField] private List<string> _tagColliders;

    private Rigidbody2D _rigidbody2D;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Knockback(Vector2 direction)
    {
        StopCoroutine(KnockbackCoroutine(direction));
        StartCoroutine(KnockbackCoroutine(direction));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_enableOnCollisionEnter) return;

        if (IsTargetTag(collision.gameObject, _tagColliders))
        {
            var delta = CalculateNormalizedDelta(transform.position, collision.transform.position);
            Knockback(delta);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_enableOnTriggerEnter) return;

        if (IsTargetTag(collision.gameObject, _tagColliders))
        {
            var delta = CalculateNormalizedDelta(transform.position, collision.transform.position);
            Knockback(delta);
        }
    }

    private Vector3 CalculateNormalizedDelta(Vector3 pointA, Vector3 pointB)
    {
        var delta = pointB - pointA;
        delta.Normalize();
        return delta;
    }

    private IEnumerator KnockbackCoroutine(Vector2 direction)
    {
        _rigidbody2D.AddForce(-direction * _knockbackForce, ForceMode2D.Impulse);
        float elapsed = 0;

        while (elapsed < _delay)
        {
            elapsed += Time.deltaTime;
            _rigidbody2D.velocity = Vector2.Lerp(_rigidbody2D.velocity, Vector2.zero, 2 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        _rigidbody2D.velocity = Vector2.zero;
    }

    private bool IsTargetTag(GameObject go, ICollection<string> tags)
    {
        foreach (var tag in tags)
        {
            if (go.CompareTag(tag))
            {
                return true;
            }
        }
        return false;
    }
}