using System.Collections;
using UnityEngine;

public class HintJustWaiting : BaseHint
{
    protected override IEnumerator Animation()
    {
        yield return new WaitForSeconds(_animationDuration);
    }
}
