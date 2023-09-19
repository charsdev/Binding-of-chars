using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _speed = 0.5f;
    public Vector3 TargetPosition;
    public float ZOffset;

    private void Start()
    {
    }

    private void LateUpdate()
    {
        if (transform.position != TargetPosition)
        {
            var tempTarget = TargetPosition;
            tempTarget.z = ZOffset;
            transform.position = Vector3.Lerp(transform.position, tempTarget, _speed);
        }
    }
}
