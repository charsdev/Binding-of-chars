using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;
    public BoxCollider2D BoxCollider;
    [SerializeField] private Vector2 _offset;

    public Room Owner;
    public Door Target;
    private Vector3 _deltaRoom;
    private bool CanTeleport = true;

    private float _delay;
    private float _initialDelay = 1.5f;

    private void Start()
    {
        _delay = _initialDelay;
        _cameraController = Camera.main.GetComponent<CameraController>();
    }

    public void Block()
    {
        if (BoxCollider != null)
            BoxCollider.enabled = false;
    }

    public void UnBlock()
    {
        if (BoxCollider != null)
            BoxCollider.enabled = true; 
    }

    public void SetTargetDoor(Door door)
    {
        Target = door;
    }

    public void SetOwnerRoom(Room owner)
    {
        Owner = owner;
    }

    public Room GetTargetDoorOnwer()
    {
        return Target?.Owner;
    }

    private void Update()
    {
        if (!CanTeleport)
        {
            _delay -= Time.deltaTime;

            if (_delay <= 0)
            {
                CanTeleport = true;
                _delay = _initialDelay;
            }
        }   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (CanTeleport)
            {
                //collision.GetComponent<PlayerController>().Freeze();
                if (Target && Target.Owner != null)
                {
                    _deltaRoom = Target.Owner.transform.position;
                    _deltaRoom.z = _cameraController.transform.position.z;
                    _cameraController.TargetPosition = _deltaRoom;
                    collision.transform.position = Target.BoxCollider.bounds.center;
                    CanTeleport = false;
                    Target.CanTeleport = false;
                }
                
                //collision.GetComponent<PlayerController>().UnFreeze();
            }
        }
    }
}