using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressVisualTracker : MonoBehaviour
{
    [SerializeField] private TMP_Text[] _labels;
    [SerializeField] private Image _linkedImage;

    private void Start()
    {
        float currentProgess = GameSaveManager.Instance.CompletedPercentage;
        int currentCompletedGames = GameSaveManager.Instance.CompletedLevelCount;

       foreach (var label in _labels)
        {
            label.text = $"{currentCompletedGames} / 7 ({currentProgess * 100:F0}%)!";
        }

        _linkedImage.fillAmount = currentProgess;
    }
}
