using UnityEngine;

public class ObstacleInfo : MonoBehaviour
{
    public int LaneIndex { get; private set; }
    public void Init(int laneIndex)
    {
        LaneIndex = laneIndex;
    }
}
