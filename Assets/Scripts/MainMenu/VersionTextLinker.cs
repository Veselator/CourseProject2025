using TMPro;
using UnityEngine;

public class VersionTextLinker : MonoBehaviour
{
    [SerializeField] private TMP_Text _linkedText;

    private void Start()
    {
        _linkedText.text = "Версія " + Application.version;
    }
}
