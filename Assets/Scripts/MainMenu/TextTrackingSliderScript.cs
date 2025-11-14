using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextTrackingSliderScript : MonoBehaviour
{
    [SerializeField] private Slider _linkedSlider;
    private TMP_Text _text;

    private void Start()
    {
        _text = GetComponent<TMP_Text>();

        _linkedSlider.onValueChanged.AddListener(ChangeText);
        ChangeText(_linkedSlider.value);
    }

    private void OnDestroy()
    {
        _linkedSlider.onValueChanged.RemoveListener(ChangeText);
    }

    private void ChangeText(float value)
    {
        _text.text = $"{value*100:F0}%";
    }
}
