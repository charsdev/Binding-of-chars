using Assets.Scripts;
using Chars.Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BindingOfChars
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private float _speed;

        private Transform _target;
        private Health _targetHealth;

        private Health _health;
        private SpriteRenderer _spriteRenderer;

        public CharacterStats CharacterStats;
        public Damager Damager;
        public AStar PathFinding;
        public GridController gridController;

        private void Start()
        {
            PathFinding = new AStar(gridController.Grid);
            _target = GameObject.FindGameObjectWithTag("Player").transform;
            Damager.DamageValue = CharacterStats.Damage; //problem here with the singleton class

            if (_target != null)
            {
                _targetHealth = _target.GetComponent<Health>();
            }

            _health = GetComponent<Health>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _health.AddDamageEvent(_ => Flash());
            _health.AddDeadEvent(() => OnDead());
        }

        private UnityAction OnDead()
        {
            if (Random.Range(0, 1) > GameManager.Instance.PlayerStats.Lucky)
            {
                Instantiate(Prefabs.Coin, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
            return null;
        }

        private void LateUpdate()
        {
            FlipToTarget();
            FollowTarget();
        }

        private List<Node> path = new List<Node>();
        private Vector3 _lastTargetPosition;
        
        private void FollowTarget()
        {
            if (_target != null && !_targetHealth.IsDead)
            {
                if (path.Count == 0)// || _lastTargetPosition != _target.transform.position)
                {
                    //path.Clear();
                    //_lastTargetPosition = _target.position;
                    var start = gridController.Grid.FindNodeByWorldPosition(transform.position);
                    var end = gridController.Grid.FindNodeByWorldPosition(_target.position);
                    path = PathFinding.FindPath(start, end);
                }
                else {
                    var targetNode = path[0];

                    if (Vector2.Distance(transform.position, targetNode.WorldPosition) < 0.01f)
                    {
                        transform.position = targetNode.WorldPosition;
                        path.RemoveAt(0);

                        if (path.Count == 0)
                        {
                            path.Clear();
                        }
                    }
                    else
                    {
                        transform.position = Vector2.MoveTowards(transform.position, targetNode.WorldPosition, CharacterStats.Speed * Time.deltaTime);
                    }
                }
            }
        }

        private void FlipToTarget()
        {
            if (_target != null)
            {
                _spriteRenderer.flipX = transform.position.x < _target.position.x;
            }
        }

        public void Flash()
        {
            StartCoroutine(FlashCoroutine());
        }

        private IEnumerator FlashCoroutine()
        {
            for (int i = 0; i < 3; i++)
            {
                _spriteRenderer.color = Color.clear;
                yield return new WaitForSeconds(0.01f);
                _spriteRenderer.color = Color.white;
            }
        }
    }
}