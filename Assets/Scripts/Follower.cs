using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _speed = 5f;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _target.position - _offset, _speed * Time.deltaTime);
    }
}
