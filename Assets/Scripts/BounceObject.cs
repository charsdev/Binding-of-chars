using UnityEngine;

public class BounceObject : MonoBehaviour
{
    public Vector2 TargetPosition; // Reference to the target object
    public float Speed = 5f; // Speed of the projectile
    public float DesiredHeight = 3f; // Desired height of the projectile's path
    public float ShadowOffset = 0.4f; // Offset distance between the shadow and the object
    public GameObject Shadow;
    public bool RandomHeight = false;
    public bool CanBounce;

    private Vector3 _initialPosition;
    [SerializeField] private float _initialSpeed;
    private float _distance;
    private Vector3 _lastTargetPosition = Vector3.zero;
    private float _initialDesiredHeight;
    Vector3 _direction = Vector3.zero;
    [SerializeField] float _bounceCount = 0;
    [SerializeField] private float _timeOfFlight;
    [SerializeField] private float _elapsedTime;
    private Vector3 _initialScale;

    private void Start()
    {
        _initialScale = transform.localScale;
        transform.localScale *= 0.15f;
    }

    public void Launch(Vector2 targetPosition)
    {
        TargetPosition = targetPosition;
        _initialDesiredHeight = DesiredHeight;
        Initialize();
    }

    private void FixedUpdate()
    {
        if (CanBounce && _elapsedTime > _timeOfFlight)
        {
            _bounceCount++;
            Initialize();
        }
        else if (_elapsedTime > _timeOfFlight || DesiredHeight == 0)
        {
            _lastTargetPosition = TargetPosition;
            return;
        }

        _currentTime = _elapsedTime / _timeOfFlight;
        _elapsedTime += Time.fixedDeltaTime;
        _horizontalDistance = Speed * _elapsedTime;
        _direction = (_lastTargetPosition - _initialPosition).normalized;
        UpdateProjectilePosition();
        UpdateShadowPosition();
    }

    private void Initialize()
    {
        _initialPosition = transform.position;
        _lastTargetPosition = TargetPosition;
        _distance = Vector3.Distance(_initialPosition, _lastTargetPosition);
        Speed = _initialSpeed;

        if (!CanBounce)
        {
            DesiredHeight = _initialDesiredHeight;
        }

        float limitHeight = DesiredHeight + 1;

        if (_distance < limitHeight)
        {
            Speed *= _distance / limitHeight;
        }

        _timeOfFlight = _distance / Speed;
        _elapsedTime = 0f;

        if (_initialScale == Vector3.zero)
        {
            _initialScale = Shadow.transform.localScale;
        }

        float bounceHeightMultiplier = Mathf.Pow(0.5f, _bounceCount);
        float reduceHeight = DesiredHeight * bounceHeightMultiplier;

        // Set vertical distance to 0 if it becomes too small
        if (Mathf.Abs(reduceHeight) < 0.01f)
        {
            reduceHeight = 0f;
            CanBounce = false;
        }

        DesiredHeight = reduceHeight;
    }

    float _currentTime = 0;
    float _horizontalDistance = 0;

    private void UpdateProjectilePosition()
    {
        float verticalDistance = Mathf.Lerp(0f, DesiredHeight, 4f * _currentTime * (1f - _currentTime));
        Vector3 newPosition = _initialPosition + (_direction * _horizontalDistance) + (Vector3.up * verticalDistance);
        transform.position = newPosition;

        if (_bounceCount < 1)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, _initialScale, _currentTime);
        }
    }

    private void UpdateShadowPosition()
    {
        Vector3 shadowTargetPosition = _initialPosition + (_direction * _horizontalDistance);
        Shadow.transform.position = shadowTargetPosition + (Vector3.down * ShadowOffset);
    }

}
