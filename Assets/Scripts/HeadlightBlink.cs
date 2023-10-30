using System.Collections;
using UnityEngine;

public class HeadlightBlink : MonoBehaviour
{
    public Material headlightMaterial;
    public float blinkInterval = 1.0f;
    public float blinkDuration = 0.1f;

    private float _timer;
    private bool _isBlinking = true;
    private static readonly int emissionColor = Shader.PropertyToID("_EmissionColor");

    private void Start()
    {
        // initialize
        _timer = blinkInterval;

        if (headlightMaterial == null)
        {
            var renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                headlightMaterial = renderer.material;
            }
            else
            {
                enabled = false;
            }
        }
    }

    private void Update()
    {
        // Decrement the timer.
        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            // Toggle the emission on and off.
            _isBlinking = !_isBlinking;

            if (_isBlinking)
            {
                // Turn the headlight on (set emission to a non-zero value).
                headlightMaterial.EnableKeyword("_EMISSION");
                headlightMaterial.SetColor(emissionColor, Color.red);
                _timer = blinkDuration;
            }
            else
            {
                // Turn the headlight off (set emission to zero).
                headlightMaterial.DisableKeyword("_EMISSION");
                _timer = blinkInterval;
            }
        }
    }
}
