using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayHintAnimationOnStart : MonoBehaviour
{
    private void Start()
    {
        IHint hint = GetComponent<IHint>();
        if (hint != null) hint.PlayAnimation();
    }
}
