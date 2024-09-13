using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleElement : MonoBehaviour
{

    private RectTransform UIElement;
    public float SetTime = 4f;
    
    void Start()
    {
        UIElement = GetComponent<RectTransform>();
        UIElement.DOLocalRotate(new Vector3(0, 0, 360), SetTime,RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1,LoopType.Restart)
            .SetRelative();
    }

    
}
