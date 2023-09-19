using UnityEngine;

public class RandomOverlay : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;
    public Sprite[] overlays;

    public void Start()
    {
        int randomIndex = Random.Range(0, overlays.Length);
        SpriteRenderer.sprite = overlays[randomIndex];
    }
}
