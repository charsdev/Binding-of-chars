using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private BounceObject _dropItem;
    [SerializeField] private Room _currentRoom;
    [SerializeField] private int _itemCount;
    [SerializeField] private int _maxItems = 5;
    [SerializeField] private bool _isOpen = false;

    private void Start()
    {
        _itemCount = Random.Range(1, _maxItems);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_isOpen)
        {
            Open();
        }
    }

    private void Open()
    {
        _animator?.SetBool("Open", true);

        for (int i = 0; i < _itemCount; i++)
        {
            BounceObject dropItem = Instantiate(_dropItem, transform.position, Quaternion.identity);
            var min = _currentRoom.Walls.bounds.min;
            var max = _currentRoom.Walls.bounds.max;
            var randomPos = new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
            dropItem.Launch(randomPos);
            _isOpen = true;
        }
    }

}
