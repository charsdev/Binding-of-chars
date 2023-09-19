using Assets.Scripts;
using UnityEngine;

public class Damager : MonoBehaviour
{
    public float DamageValue;
    [SerializeField] private string _tag;
    [SerializeField] private bool _enableOnCollisionEnter;
    [SerializeField] private bool _enableOnTriggerEnter;
    [SerializeField] private float _damageCooldown;

    private float _lastDamageTime;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!_enableOnCollisionEnter) return;

        if (collision.gameObject.CompareTag(_tag) 
            && collision.gameObject.TryGetComponent(out Health health) 
            && _lastDamageTime <= Time.time)
        {
            health?.ReceiveDamage(gameObject, DamageValue);
            _lastDamageTime = Time.time + _damageCooldown;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!_enableOnTriggerEnter) return;

        if (collision.CompareTag(_tag) 
            && collision.TryGetComponent(out Health health) 
            && _lastDamageTime <= Time.time)
        {
            health?.ReceiveDamage(gameObject, DamageValue);
            _lastDamageTime = Time.time + _damageCooldown;
        }
    }
}
