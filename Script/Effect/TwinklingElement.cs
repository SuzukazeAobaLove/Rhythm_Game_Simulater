using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinklingElement : MonoBehaviour
{
    private RectTransform UIElement;
    private Ease ScaleEase = Ease.InOutQuad;
    private LoopType LoopType = LoopType.Yoyo;
    public float Scale = 1.2f;

    void Start()
    {
        UIElement = GetComponent<RectTransform>();
        UIElement.DOScale(Vector3.one * Scale, 1f)
                 .SetEase(ScaleEase)
                 .SetLoops(-1, LoopType);
    }

   
}
