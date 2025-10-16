using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PuzzleSolvedTextAnimation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _topText;
    [SerializeField] private TextMeshProUGUI _bottomText;
    [SerializeField] private string[] _texts;
    [SerializeField] private ParticleSystem _particleSystem;
    private Animator _animator;
    private Gm _gm;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator component not found on the GameObject.");
        }

        if (_texts.Length == 0)
        {
            Debug.LogError("No texts assigned in the _texts array.");
        }

        _gm = Gm.Instance;
        _gm.OnPuzzleSolved += HandlePuzzleSolved;
    }

    private void OnDestroy()
    {
        if (_gm != null)
        {
            _gm.OnPuzzleSolved -= HandlePuzzleSolved;
        }
    }

    private string GetRandomText()
    {
        return _texts[Random.Range(0, _texts.Length)];
    }

    private void SetRandomText()
    {
        string text = GetRandomText();
        _topText.text = text;
        _bottomText.text = text;
    }

    private void HandlePuzzleSolved()
    {
        SetRandomText();
        _particleSystem.Play();
        _animator.SetTrigger("PlayAnim");
    }
}
