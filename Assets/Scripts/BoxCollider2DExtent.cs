using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BoxCollider2DExtent : MonoBehaviour
{
    public Collider2D Collider2D;
    public string TargetTag;
    private SpriteRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(TargetTag))
        {
            Collider2D = collider;
            _renderer.color = Color.red;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag(TargetTag))
        {
            Collider2D = null;
            _renderer.color = Color.green;
        }
    }
}
