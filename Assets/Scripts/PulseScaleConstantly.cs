using UnityEngine;

public class PulseScaleConstantly : MonoBehaviour
{
    [SerializeField] private float minScale = 0.98f;
    [SerializeField] private float maxScale = 1.02f;
    [SerializeField] private float pulseSpeed = 5f;

    private Vector3 baseScale;

    private void Awake()
    {
        baseScale = transform.localScale;
    }

    private void Update()
    {
        float scale = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);
        transform.localScale = baseScale * scale;
    }
}
