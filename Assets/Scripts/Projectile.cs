using Assets.Scripts;
using UnityEngine;
using UnityEngine.Pool;

namespace BindingOfChars
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private int _damage;
        [SerializeField] private float _curvePower;
        [SerializeField] private GameObject _sfx;

        private Rigidbody2D _rigidbody2D;
        private Vector3 _direction;
        private IObjectPool<Projectile> _objectPool;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void SetDirection(Vector3 direction) 
        {
            _direction = direction; 
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        private void Update()
        {
            _rigidbody2D.velocity = _direction * _speed;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                collision.GetComponent<Health>()?.ReceiveDamage(gameObject, _damage);
                Instantiate(_sfx, transform.position, Quaternion.identity);
                _objectPool.Release(this);
            }
            else if(collision.CompareTag("Wall"))
            {
                _objectPool.Release(this);
                Instantiate(_sfx, transform.position, Quaternion.identity);
            }
        }

        internal void SetPool(IObjectPool<Projectile> projectilePool)
        {
            _objectPool = projectilePool;
        }
    }
}