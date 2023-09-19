using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Input = UnityEngine.Input;
using UnityEngine.Pool;


namespace BindingOfChars
{
    [System.Serializable]
    public struct HeadFaces
    {
        public Sprite Left, LeftBlink;
        public Sprite Right, RightBlink;
        public Sprite Up;
        public Sprite Down, DownBlink;
    }

    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private Transform _firepoint;
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private SpriteRenderer _head, _body;
        [SerializeField] private GameObject _deadAnim;
        [SerializeField] private HeadFaces _headFaces;
        [SerializeField] private Animator _bodyAnimator;
        [SerializeField] private GameObject _bomb;
        [SerializeField] private GameObject _danceAnimation;

        private Health _health;
        private Rigidbody2D _rigidbody;
        private BoxCollider2D _boxCollider;

        private bool _isShooting = false;
        private bool _isBlinking = false;
        private bool _isMoving = false;

        private Vector3 _movementDirection;
        private Vector3 _shootDirection = Vector3.zero;
        private Dictionary<Vector3, Sprite> _headFaceByDirection = new Dictionary<Vector3, Sprite>();
        private Dictionary<Vector3, Sprite> _headBlinkByDirection = new Dictionary<Vector3, Sprite>();
        private ObjectPool<Projectile> _projectilePool;

        public CharacterStats PlayerStats;
        public LayerMask WallLayer;

        private Vector2 _fakeVelocity;
        private Vector2 _lastFramePosition;
        private bool _canMove;
        private PlayerInventory _playerInventory;

        private void Start()
        {
            _canMove = true;
            _rigidbody = GetComponent<Rigidbody2D>();
            _boxCollider = GetComponent<BoxCollider2D>();
            _health = GetComponent<Health>();
            _health.AddDeadEvent(() => StartCoroutine(DeadCoroutine()));

            _headFaceByDirection.Add(Vector3.up, _headFaces.Up);
            _headFaceByDirection.Add(Vector3.down, _headFaces.Down);
            _headFaceByDirection.Add(Vector3.right, _headFaces.Right);
            _headFaceByDirection.Add(Vector3.left, _headFaces.Left);

            _headBlinkByDirection.Add(Vector3.up, _headFaces.Up);
            _headBlinkByDirection.Add(Vector3.down, _headFaces.DownBlink);
            _headBlinkByDirection.Add(Vector3.right, _headFaces.RightBlink);
            _headBlinkByDirection.Add(Vector3.left, _headFaces.LeftBlink);

            _projectilePool = new ObjectPool<Projectile>(CreateProjectile,
                    OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
                    false, 10, 20);

            _health.AddDeadEvent(StopPlayer);

            _fakeVelocity = Vector2.zero;
            _lastFramePosition = transform.position;
            _playerInventory = gameObject.AddComponent<PlayerInventory>();
        }

        // invoked when creating an item to populate the object pool
        private Projectile CreateProjectile()
        {
            Projectile projectileInstance = Instantiate(_projectilePrefab);
            projectileInstance.SetPool(_projectilePool);
            return projectileInstance;
        }

        // invoked when returning an item to the object pool
        private void OnReleaseToPool(Projectile pooledObject)
        {
            pooledObject.gameObject.SetActive(false);
        }

        // invoked when retrieving the next item from the object pool
        private void OnGetFromPool(Projectile pooledObject)
        {
            pooledObject.gameObject.SetActive(true);
        }

        // invoked when we exceed the maximum number of pooled items (i.e. destroy the pooled object)
        private void OnDestroyPooledObject(Projectile pooledObject)
        {
            Destroy(pooledObject.gameObject);
        }

        private void StopPlayer()
        {
            if (_rigidbody == null)
                return;

            _rigidbody.velocity = Vector2.zero;
            _rigidbody.angularVelocity = 0;
            _rigidbody.angularDrag = 0;
            _rigidbody.position = transform.position;
        }

        private void FixedUpdate()
        {
            var displacement = (Vector2)transform.position - _lastFramePosition;
            _fakeVelocity = displacement / Time.deltaTime;
            _lastFramePosition = transform.position;
        }

        private void Update()
        {
            if (_health.IsDead) return;
            if (!_canMove) return;

            HandleHeadDirection();
            HandleFire();
            HandleBomb();

            float horizontalAxis = Input.GetAxis("Horizontal");
            float verticalAxis = Input.GetAxis("Vertical");

            Vector3 direction = new Vector2(horizontalAxis, verticalAxis);
            _rigidbody.velocity = direction * PlayerStats.Speed;

            _isMoving = Mathf.Abs(direction.sqrMagnitude) > 0 && Mathf.Abs(_fakeVelocity.sqrMagnitude) > 0;

            _body.flipX = horizontalAxis < 0;

            _bodyAnimator.SetBool("IsMoving", _isMoving);
            _bodyAnimator.SetFloat("Horizontal", horizontalAxis);
            _bodyAnimator.SetFloat("Vertical", verticalAxis);

            if (horizontalAxis == 0 && verticalAxis == 0)
            {
                _movementDirection = Vector3.zero;
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                _body.gameObject.SetActive(!_body.gameObject.activeInHierarchy);
                _head.gameObject.SetActive(!_head.gameObject.activeInHierarchy);
                _danceAnimation.SetActive(!_danceAnimation.activeInHierarchy);
            }

            if (_danceAnimation.activeInHierarchy && (horizontalAxis != 0 || verticalAxis != 0))
            {
                _body.gameObject.SetActive(true);
                _head.gameObject.SetActive(true);
                _danceAnimation.SetActive(false);
            }

        }

        private void HandleBomb()
        {
            if (Input.GetKeyDown(KeyCode.E) && _playerInventory.Bombs.Value > 0)
            {
                Instantiate(_bomb, transform.position, Quaternion.identity);
                _playerInventory.Bombs.Value--;
            }
        }

        private void HandleHeadDirection()
        {
            if (_isShooting)
            {
                if (_shootDirection != Vector3.zero && !_isBlinking)
                {
                    _head.sprite = _headFaceByDirection[_shootDirection];
                }
            }
            else if (_movementDirection != Vector3.zero)
            {
                _head.sprite = _headFaceByDirection[_movementDirection];
            }
            else
            {
                _head.sprite = _headFaces.Down;
            }
        }

        private IEnumerator DeadCoroutine()
        {
            _head.gameObject.SetActive(false);
            _body.gameObject.SetActive(false);
            _deadAnim.SetActive(true);
            _boxCollider.enabled = false;
            yield return new WaitForSeconds(1);
            _gameOverPanel.SetActive(true);
        }

        private void HandleMovementAxis(float inputAxis, Vector3 look)
        {
            if (inputAxis != 0)
            {
                var direction = inputAxis * look;
                _movementDirection = direction.normalized;
                transform.position += PlayerStats.Speed * Time.deltaTime * direction;
            }
        }

        private void HandleFire()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _isShooting = true;
                _shootDirection = Vector3.left;
                ShootProjectile();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _isShooting = true;
                _shootDirection = Vector3.up;
                ShootProjectile();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _isShooting = true;
                _shootDirection = Vector3.down;
                ShootProjectile();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _isShooting = true;
                _shootDirection = Vector3.right;
                ShootProjectile();
            }
        }

        private void ShootProjectile()
        {
            StartCoroutine(ShootProjectileCoroutine());
        }

        private IEnumerator ShootProjectileCoroutine()
        {
            yield return new WaitForSeconds(PlayerStats.RateOfFire);
            StopAllCoroutines();
            var projectile = _projectilePool.Get();
            projectile.transform.position = _firepoint.position;
            projectile.transform.rotation = Quaternion.identity;
            projectile.SetDirection(_shootDirection);
            projectile.SetSpeed(PlayerStats.ProjectileSpeed);
            StartCoroutine(Blink(_shootDirection));
            StartCoroutine(ResetShooting());
            yield return null;
        }

        private IEnumerator ResetShooting()
        {
            yield return new WaitForSeconds(1);
            _isShooting = false;
        }

        private IEnumerator Blink(Vector3 direction)
        {
            _isBlinking = true;
            _head.sprite = _headBlinkByDirection[direction];
            yield return new WaitForSeconds(0.1f);
            _head.sprite = _headFaceByDirection[direction];
            _isBlinking = false;
        }

        internal void Freeze()
        {
            _canMove = false;
        }

        internal void UnFreeze()
        {
            _canMove = true;
        }
    }
}