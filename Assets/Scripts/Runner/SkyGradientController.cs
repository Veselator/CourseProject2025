using UnityEngine;

[System.Serializable]
public class SkyGradientPreset
{
    public string name;
    public Color topColor = new Color(0.4f, 0.7f, 1f);
    public Color middleColor = new Color(0.8f, 0.9f, 1f);
    public Color bottomColor = new Color(1f, 0.95f, 0.8f);
    public float middlePosition = 0.5f;
    public float brightness = 1f;
}

public class SkyGradientController : MonoBehaviour
{
    private Material skyMaterial;

    [Header("Current Settings")]
    [SerializeField] private Color topColor = new Color(0.4f, 0.7f, 1f);
    [SerializeField] private Color middleColor = new Color(0.8f, 0.9f, 1f);
    [SerializeField] private Color bottomColor = new Color(1f, 0.95f, 0.8f);

    [Space]
    [SerializeField, Range(0f, 1f)] private float middlePosition = 0.5f;
    [SerializeField, Range(0f, 1f)] private float topBlend = 0.3f;
    [SerializeField, Range(0f, 1f)] private float bottomBlend = 0.3f;

    [Header("Effects")]
    [SerializeField, Range(0f, 2f)] private float brightness = 1f;
    [SerializeField, Range(0f, 2f)] private float contrast = 1f;
    [SerializeField, Range(0f, 2f)] private float saturation = 1f;

    [Header("Animation")]
    [SerializeField, Range(-2f, 2f)] private float scrollSpeed = 0f;
    [SerializeField, Range(0f, 0.1f)] private float waveAmplitude = 0f;
    [SerializeField, Range(0f, 10f)] private float waveFrequency = 1f;

    [Header("Presets")]
    [SerializeField]
    private SkyGradientPreset[] presets = new SkyGradientPreset[]
    {
        new SkyGradientPreset
        {
            name = "Day",
            topColor = new Color(0.4f, 0.7f, 1f),
            middleColor = new Color(0.8f, 0.9f, 1f),
            bottomColor = new Color(1f, 0.95f, 0.8f),
            middlePosition = 0.4f,
            brightness = 1.2f
        },
        new SkyGradientPreset
        {
            name = "Sunset",
            topColor = new Color(1f, 0.3f, 0.1f),
            middleColor = new Color(1f, 0.8f, 0.3f),
            bottomColor = new Color(1f, 0.9f, 0.7f),
            middlePosition = 0.6f,
            brightness = 1.1f
        },
        new SkyGradientPreset
        {
            name = "Night",
            topColor = new Color(0.05f, 0.05f, 0.2f),
            middleColor = new Color(0.1f, 0.1f, 0.3f),
            bottomColor = new Color(0.2f, 0.15f, 0.3f),
            middlePosition = 0.3f,
            brightness = 0.8f
        }
    };

    void Start()
    {
        if (skyMaterial == null)
            skyMaterial = GetComponent<Renderer>().material;

        UpdateMaterial();
    }

    void Update()
    {
        UpdateMaterial();
    }

    void UpdateMaterial()
    {
        if (skyMaterial == null) return;

        skyMaterial.SetColor("_TopColor", topColor);
        skyMaterial.SetColor("_MiddleColor", middleColor);
        skyMaterial.SetColor("_BottomColor", bottomColor);

        skyMaterial.SetFloat("_MiddlePosition", middlePosition);
        skyMaterial.SetFloat("_TopBlend", topBlend);
        skyMaterial.SetFloat("_BottomBlend", bottomBlend);

        skyMaterial.SetFloat("_Brightness", brightness);
        skyMaterial.SetFloat("_Contrast", contrast);
        skyMaterial.SetFloat("_Saturation", saturation);

        skyMaterial.SetFloat("_ScrollSpeed", scrollSpeed);
        skyMaterial.SetFloat("_WaveAmplitude", waveAmplitude);
        skyMaterial.SetFloat("_WaveFrequency", waveFrequency);
    }

    public void ApplyPreset(int presetIndex)
    {
        if (presetIndex < 0 || presetIndex >= presets.Length) return;

        SkyGradientPreset preset = presets[presetIndex];
        topColor = preset.topColor;
        middleColor = preset.middleColor;
        bottomColor = preset.bottomColor;
        middlePosition = preset.middlePosition;
        brightness = preset.brightness;

        UpdateMaterial();
    }

    public void ApplyPreset(string presetName)
    {
        for (int i = 0; i < presets.Length; i++)
        {
            if (presets[i].name == presetName)
            {
                ApplyPreset(i);
                break;
            }
        }
    }

    // ћетоды дл€ плавного перехода между пресетами
    public void TransitionToPreset(int presetIndex, float duration)
    {
        if (presetIndex >= 0 && presetIndex < presets.Length)
        {
            StartCoroutine(TransitionCoroutine(presets[presetIndex], duration));
        }
    }

    private System.Collections.IEnumerator TransitionCoroutine(SkyGradientPreset targetPreset, float duration)
    {
        Color startTop = topColor;
        Color startMiddle = middleColor;
        Color startBottom = bottomColor;
        float startMiddlePos = middlePosition;
        float startBrightness = brightness;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            topColor = Color.Lerp(startTop, targetPreset.topColor, t);
            middleColor = Color.Lerp(startMiddle, targetPreset.middleColor, t);
            bottomColor = Color.Lerp(startBottom, targetPreset.bottomColor, t);
            middlePosition = Mathf.Lerp(startMiddlePos, targetPreset.middlePosition, t);
            brightness = Mathf.Lerp(startBrightness, targetPreset.brightness, t);

            yield return null;
        }

        ApplyPreset(System.Array.FindIndex(presets, p => p == targetPreset));
    }
}