using System.Collections;
using UnityEngine;

public class RespawnPointVisual : MonoBehaviour
{
    private Material _linkedMaterial;
    [SerializeField] private SpawnPoint _linkedSpawnPoint;

    private bool _isFilled = false;
    [SerializeField] private Color ColorIfLinked = Color.green;
    [SerializeField] private Color ColorIfNotLinked = Color.gray;
    [SerializeField] private float _animationDuration = 1f;

    private void Start()
    {
        _linkedMaterial = GetComponent<SpriteRenderer>().material;

        _linkedMaterial.SetColor("_Color", ColorIfNotLinked);
        _linkedSpawnPoint.OnPlayerEnter += HandlePlayerEnter;
    }

    private void OnDestroy()
    {
        _linkedSpawnPoint.OnPlayerEnter -= HandlePlayerEnter;
    }

    private void HandlePlayerEnter()
    {
        if (_isFilled) return;
        _isFilled = true;

        StartCoroutine(ColorChange());
    }

    private IEnumerator ColorChange()
    {
        if (_linkedMaterial == null)
        {
            yield break;
        }

        // Текущий и целевой цвет
        Color startColor = ColorIfNotLinked;
        Color targetColor = ColorIfLinked;

        float elapsed = 0f;
        while (elapsed < _animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _animationDuration);

            Color current = Color.Lerp(startColor, targetColor, t);
            _linkedMaterial.SetColor("_Color", current);
            yield return null;
        }

        _linkedMaterial.SetColor("_Color", targetColor);
    }
}
