using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ObstacleAlertController : MonoBehaviour
{
    [SerializeField] private int currentLane;
    [SerializeField] private InputMode currentInputMode;
    private static float animationDuration = 2f;
    private static float maxTransparency = 0.5f;
    private static float animationIntensity = 4.7f;

    private SpawnObstacles obstacles;
    private Image rectImage;

    private void Start()
    {
        obstacles = SpawnObstacles.Instance;
        obstacles.OnObstacleSpawned += CheckToAnimate;

        rectImage = GetComponent<Image>();
        rectImage.color = new Color(rectImage.color.r, rectImage.color.g, rectImage.color.b, 0f);
    }

    private void OnDestroy()
    {
        obstacles.OnObstacleSpawned -= CheckToAnimate;
    }

    private void CheckToAnimate(int laneIndex, InputMode inputMode)
    {
        Debug.Log($"Checking to animate. I`m {currentLane} & {currentInputMode} while input values are {laneIndex} {inputMode}");
        if (laneIndex == currentLane && inputMode == currentInputMode) StartCoroutine(AnimateMe());
    }

    private float GetCurrentValueOfTransparency(float elapsedTime)
    {
        return Mathf.Abs(Mathf.Sin(elapsedTime * animationIntensity) * maxTransparency);
    }

    private IEnumerator AnimateMe()
    {
        Debug.Log($"I`m ({currentInputMode}) animating!");
        float elapsedTime = 0f;
        Color startColor = rectImage.color;
        
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            rectImage.color = new Color(startColor.r, startColor.g, startColor.b, GetCurrentValueOfTransparency(elapsedTime));
            yield return null;
        }

        rectImage.color = startColor;
    }
}
