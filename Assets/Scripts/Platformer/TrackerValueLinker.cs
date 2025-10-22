using UnityEngine;

public class TrackerValueLinker : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private DynamicValueTracker _tracker;

    private void Start()
    {
        _tracker.Initialize(_health);
    }
}
