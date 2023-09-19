using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrightnessController : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;
    public float Brightness = 1.0f;
    private Material _material;
    [SerializeField] private Color _color;

    private void Start()
    {
        _material = SpriteRenderer.material;
        UpdateBrightness();
    }
    private void Update()
    {
        UpdateBrightness();
    }

    private void UpdateBrightness()
    {
        if (SpriteRenderer == null)
        {
            Debug.LogWarning("Sprite Renderer reference not set!");
            return;
        }

        // Set the brightness value in the MaterialPropertyBlock
        _material.SetFloat("_Brightness", Brightness);
        _material.SetColor("_Color", _color);
    }
}
