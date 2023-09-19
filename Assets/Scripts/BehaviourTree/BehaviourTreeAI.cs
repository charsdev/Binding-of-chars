using BehaviourTree;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeAI : MonoBehaviour
{
    private Node _root;
    [SerializeField] private Transform _target;
    [SerializeField] private float _proximityRange = 5f;
    [SerializeField] private Vector2 _direction = Vector2.zero;

    private void Start()
    {
        wanderTime = randomDirTime;
        BuildBehaviourTree();
    }

    private void BuildBehaviourTree()
    {
        var proximityNode = new ActionNode(() => CheckProximity());
        var chaseNode = new ActionNode(() => ChasePlayer());
        var wanderNode = new ActionNode(() => Wander());


        _root = new Selector(new List<Node>
        {
            new Sequence(new List<Node> {
                proximityNode,
                chaseNode
            }),
            wanderNode
        });
    }
  
    private void Update()
    {
        if (_target == null) _target = GameObject.FindGameObjectWithTag("Player").transform;

        _root.Evaluate();
    }

    [SerializeField] private float wanderTime = 0;
    float randomDirTime = 3f;
  
    private NodeState Wander()
    {
        if (wanderTime >= randomDirTime)
        {
            _direction = Random.insideUnitCircle.normalized;
            wanderTime = 0f;
        }

        wanderTime += Time.deltaTime;

        transform.Translate(_direction * Time.deltaTime);
        
        return NodeState.RUNNING;
    }

    private NodeState ChasePlayer()
    {
        Debug.Log("Chase");

        if (_target == null) 
            return NodeState.FAILURE;

        Vector2 playerDirection = (_target.position - transform.position).normalized;
        transform.Translate(playerDirection * Time.deltaTime);
        return NodeState.RUNNING;
    }

    private NodeState CheckProximity()
    {
        Debug.Log("Check");

        if (_target == null) 
            return NodeState.FAILURE;

        return Vector3.Distance(transform.position, _target.position) < _proximityRange ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
