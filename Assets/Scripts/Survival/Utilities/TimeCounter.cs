using UnityEngine;

public class TimeCounter : MonoBehaviour
{
    public static TimeCounter Instance { get; private set; }
    public float TimeCount = 30f;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (GlobalFlags.GetFlag(Flags.GameOver)) return;
        TimeCount -= Time.deltaTime;
        TimeCount = Mathf.Max(TimeCount, 0f);
    }
}
